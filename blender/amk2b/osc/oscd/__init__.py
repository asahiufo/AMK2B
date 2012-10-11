# python-osc
# Copyright (C) 2010 Ryan Coyner
#
# This program is free software: you can redistribute it and/or modify it under
# the terms of the GNU General Public License as published by the Free Software
# Foundation, either version 3 of the License, or (at your option) any later
# version.
#
# This program is distributed in the hope that it will be useful, but WITHOUT
# ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
# FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
# details.
#
# You should have received a copy of the GNU General Public License along with
# this program. If not, see <http://www.gnu.org/licenses/>.

"""
This module defines classes for parsing and dispatching Open Sound Control
packets.

"""

import abc
import logging
import math
import re
import socket
import socketserver
import struct
import time

__all__ = ['Deserializer', 'RequestHandler', 'Server', 'ForkingServer',
           'ThreadingServer', 'Method']


logger = logging.getLogger('oscd')
logger.setLevel(logging.INFO)
logger.propagate = False

if len(logger.handlers) == 0:
    logger.addHandler(logging.NullHandler())


BUNDLE_HEADER = '#bundle'
SECONDS_OFFSET = 2208988800 # Seconds from 1900 to 1970


class Deserializer(object):
    """Deserializes OSC packets into units of bundle/message."""
    def deserialize_packet(self, packet, type=None):
        """Deserializes the specified packet."""

        # Remove the last 4-bytes, which is size of the packet
        if type is socket.SOCK_DGRAM:
            size = self.deserialize_int32(packet[-4:])[0]

            if size != (len(packet) - 4):
                logger.warning("Malformed packet size '%d'; expected %d" %
                               (len(packet), size))
            else:
                packet = packet[:-4]

        data = []

        if packet.startswith(b'#'):
            data.extend(self.deserialize_bundle(packet))
        else: # OSCMessage
            data.extend(self.deserialize_message(packet))

        return data

    def deserialize_blob(self, packet):
        """Deserializes the packet as an OSC Blob."""
        size = struct.unpack('>i', packet[0:4])[0]
        index = int(math.ceil((size + 1) / 4.0) * 4)
        return (packet[4:size + 4], packet[index:])

    def deserialize_bundle(self, packet):
        """Deserializes the packet as an OSC Bundle."""
        data = []
        total_size = 0

        address, packet = self.deserialize_string(packet) # remove #bundle
        data.append(address)

        timetag, packet = self.deserialize_timetag(packet)
        data.append(timetag)

        while len(packet) > 0: # deserialize bundle elements
            size, packet = self.deserialize_int32(packet)
            data.append(self.deserialize_packet(packet[:size]))
            packet = packet[size:]

        return data

    def deserialize_float32(self, packet):
        """Deserializes the next 4 bytes of the packet as a 32-bit float."""
        if len(packet) < 4:
            raise ValueError('The packet size (%d) must be at least 4 bytes' %
                             len(packet))

        return (struct.unpack('>f', packet[0:4])[0], packet[4:])

    def deserialize_int32(self, packet):
        """Deserializes the next 4 bytes of the packet as a 32-bit integer."""
        if len(packet) < 4:
            raise ValueError('The packet size (%d) must be at least 8 bytes' %
                             len(packet))

        return (struct.unpack('>i', packet[0:4])[0], packet[4:])

    def deserialize_message(self, packet):
        """Deserializes the packet as an OSC Message."""
        data = []
        address, packet = self.deserialize_string(packet)
        data.append(address)

        typetags, packet = self.deserialize_string(packet)

        data.append(typetags)
        if typetags.startswith(','):
            for tag in typetags[1:]:
                argument, packet = self.TYPE_MAP[tag](self, packet)
                data.append(argument)
        else:
            raise ValueError("Typetag '%s' must begin with a comma" % typetags)

        return data

    def deserialize_string(self, packet):
        """Deserializes the packet as an OSC String."""
        # Simple split does't work b/c need to get rid of all null bytes
        length = packet.find(b'\0')
        index = int(math.ceil((length + 1) / 4.0) * 4)
        return (str(packet[0:length], 'utf8'), packet[index:])

    def deserialize_timetag(self, packet):
        """Deserializes the next 8 bytes of the packet as a timetag."""
        if len(packet) < 8:
            raise ValueError('The packet size (%d) must be at least 8 bytes' %
                             len(packet))

        seconds, fractional = struct.unpack('>II', packet[0:8])
        if seconds == 0 and fractional <= 1:
            data = 0.0
        else:
            data = seconds + (fractional / 1e9)
        return (data, packet[8:])

    TYPE_MAP = {
        'i': deserialize_int32,
        'f': deserialize_float32,
        's': deserialize_string,
        'b': deserialize_blob
    }


class RequestHandler(socketserver.DatagramRequestHandler):
    # maketrans is not used for all of them b/c they need to be of equal length
    trans = str.maketrans('{,}', '(|)')

    # for datagram services, self.request is a pair of string and socket.
    def dispatch(self, address, typetags, data):
        """Dispatch an OSC Message to the corresponding OSC Method."""
        if len(typetags) != len(data):
            raise ValueError('Invalid OSC Message; number of typetags (%d) '
                            'must match number of arguments (%d)')

        # Convert the target address into a regular expression
        # http://opensoundcontrol.org/spec-1_0
        pattern = address.replace('.', '\.') # Escape all periods
        pattern = pattern.replace('(', '\(') # Escape parenthesis
        pattern = pattern.replace(')', '\)')
        pattern = pattern.replace('*', '.*') # * matches any sequence
        pattern = pattern.replace('?', '.') # ? matches any character
        pattern = pattern.replace('[!', '[^') # [! performs omission
        pattern = pattern.translate(self.trans)
        regex = re.compile(pattern)

        # Iterate through each method to see if it matches the address
        for method in self.server.methods:
            match = regex.match(method.address)
            if match and match.end() == len(method.address):
                method(address, typetags, data)

    def finish(self):
        """Performs cleanup actions."""
        pass

    def handle(self):
        """Service the request."""
        data = self.deserial.deserialize_packet(self.packet, socket.SOCK_DGRAM)

        # Dispatch each message separately.
        self._unbundle(data)

    def setup(self):
        """Performs initialization actions."""
        self.deserial = Deserializer()
        self.packet, self.socket = self.request

    def _unbundle(self, data):
        """Retrieve the contents of an OSC Bundle."""
        if data[0] == BUNDLE_HEADER:
            timetag = data[1]
            now = time.time() + SECONDS_OFFSET
            if timetag > 0 and timetag > now:
                time.sleep(timetag - now)

            for element in data[2:]:
                self._unbundle(element)
        else: # OSC Message
            self.dispatch(data[0], data[1][1:], data[2:]) # rm comma w/ [1:]


class Server(socketserver.UDPServer):
    """A synchronous server which handles incoming requests."""
    # XXX: add timeout support?
    def __init__(self, server_address, handler=RequestHandler):
        socketserver.UDPServer.__init__(self, server_address, handler)

        self.methods = []

    def add_method(self, method):
        if not hasattr(method, '__call__'):
            raise TypeError("'%s' is not callable" % method)

        self.methods.append(method)

    def remove_method(self, method):
        self.methods.remove(method)


class ForkingServer(socketserver.ForkingMixIn, Server):
    """
    An asynchronous server which forks a new process to handle incoming
    requests.

    """
    pass


class ThreadingServer(socketserver.ThreadingMixIn, Server):
    """
    An asynchronous server which starts a new thread to handle incoming
    requests.

    """
    pass


class Method(metaclass=abc.ABCMeta):
    invalid_chars = '#*,?[]{}'

    def __init__(self, address):
        self._address = address

    @abc.abstractmethod
    def __call__(self, address, typetags, data):
        raise NotImplementedError

    @property
    def address(self):
        return self._address

    @address.setter
    def address(self, address):
        for char in self.invalid_chars:
            if char in address:
                raise ValueError("A Method may not have '%c' in its "
                                 ' address space.')
