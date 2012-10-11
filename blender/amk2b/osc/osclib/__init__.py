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
This module defines classes which are used to construct Open Sound Control
packets and send them to a server capable of handling the packets.

"""

import math
import socket
import struct
import time

__all__ = ['Bundle', 'Message', 'Serializer', 'Client', 'timetag']


BUNDLE_HEADER = '#bundle'
SECONDS_OFFSET = 2208988800 # Seconds from 1900 to 1970


VALID_TYPETAGS = [
    'i',
    'f',
    's',
    'b'
]

PYTHON_TYPETAGS = {
    int: 'i',
    float: 'f',
    str: 's',
}


class Message(object):
    def __init__(self, pattern=None, *args):
        """Accepts tuple or single value as argument"""
        # Initialize properties
        self._pattern = None

        self.arguments = []
        self.typetags = []

        if pattern is not None:
            self.pattern = pattern

        if args:
            self.append(*args)

    def append(self, *args):
        for arg in args:
            if isinstance(arg, tuple):
                value, typetag = arg

                if typetag not in VALID_TYPETAGS:
                    raise TypeError("Invalid argument type '%s'." % typetag)
            else:
                value = arg
                typetag = PYTHON_TYPETAGS[type(value)] # XXX: try/catch?

            self.arguments.append(value)
            self.typetags.append(typetag)

    @property
    def pattern(self):
        return self._pattern

    @pattern.setter
    def pattern(self, pattern):
        if pattern[0] != '/':
            raise ValueError('Message pattern %s requires a forward-slash.'
                             % pattern)
        self._pattern = pattern


class Bundle(object):
    def __init__(self, timetag=None, *elements):
        self.timetag = timetag
        self.elements = []

        if elements:
            self.append(*elements)

    def append(self, *elements):
        for element in elements:
            if isinstance(element, Message):
                self.elements.append(element)
            elif isinstance(element, Bundle):
                    # When bundles contain other bundles, the OSC Time Tag of
                    # the enclosed bundle must be greater than or equal to the
                    # OSC Time Tag of the enclosing bundle.
                    # XXX: What if self is immediate and element is set?
                if element.timetag is not None and \
                   self.timetag is not None and \
                   element.timetag < self.timetag:
                    raise Exception # XXX
                self.elements.append(element)
            else:
                raise TypeError("Invalid element type '%s'." % element)


class Serializer(object):
    """Serialize into an OSC Packet, which is contents/size..."""
    # XXX: struct.pack_into instead?
    def serialize_packet(self, data):
        if isinstance(data, Bundle):
            packet = self.serialize_bundle(data)
        elif isinstance(data, Message):
            packet = self.serialize_message(data)
        else:
            raise TypeError("Invalid packet type '%s'." % data)

        packet += struct.pack('>i', len(packet))

        return packet

    def serialize_bundle(self, bundle):
        buffer = self.serialize_string(BUNDLE_HEADER)
        buffer += self.serialize_timetag(bundle.timetag)

        for element in bundle.elements:
            if isinstance(element, Bundle):
                data = self.serialize_bundle(element)
            elif isinstance(element, Message):
                data = self.serialize_message(element)
            else:
                raise TypeError("Invalid element type '%s'." % element)

            buffer += self.serialize_int32(len(data))
            buffer += data

        return buffer

    def serialize_message(self, message):
        buffer = self.serialize_string(message.pattern)
        buffer += self.serialize_string(',' + ''.join(message.typetags))

        args = zip(message.typetags, message.arguments)

        for typetag, value in args:
            if typetag == 'i':
                buffer += self.serialize_int32(value)
            elif typetag == 'f':
                buffer += self.serialize_float(value)
            elif typetag == 's':
                buffer += self.serialize_string(value)
            elif typetag == 'b':
                buffer += self.serialize_blob(value)
            else:
                raise TypeError("Invalid argument type '%s'." % typetag)

        return buffer

    def serialize_blob(self, data):
        return struct.pack('>%dp' % (math.ceil((len(data) + 1) / 4.0) * 4), data)

    def serialize_float32(self, data):
        return struct.pack('>f', data)

    def serialize_int32(self, data):
        return struct.pack('>i', data)

    def serialize_string(self, data):
        """Essentially OSCString"""
        return struct.pack('>%ds' % (math.ceil((len(data) + 1) / 4.0) * 4), data)

    def serialize_timetag(self, timetag):
        if timetag is None:
            buffer = struct.pack('>II', 0, 1)
        else:
            buffer = struct.pack('>II', int(timetag.seconds),
                                 int(timetag.fractional))

        return buffer


class Client(object):
    # 0xffff - (sizeof(IP Header) + sizeof(UDP Header)) = 65535-(20+8) = 65507
    MAX_SIZE = 65507
    # XXX: MTU limits this theoretical maximum. If packets are being sent over
    # IPv4, the safest maximum size whould be 576 bytes, including headers. 548
    # for just data. 1500 for IPv6

    def __init__(self):
        self._serializer = Serializer()
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.socket.setsockopt(socket.SOL_SOCKET, socket.SO_SNDBUF,
                               self.MAX_SIZE)

    def connect(self, address):
        """Establishes a connection to (host, port)."""
        try:
            self.socket.connect(address)
        except socket.error as e:
            self.close()
            # XXX: Exception?

    def close(self):
        """Disconnect and close the socket."""
        if self.socket is not None:
            self.socket.close()
            self.socket = None

    def send(self, packet):
        """Send the specified message to a server."""
        try:
            self.socket.sendall(self._serializer.serialize_packet(packet))
        except socket.error as e:
            raise e


class timetag(object):
    # XXX: property time and auto?, or fract/seconds?
    def __init__(self, time):
        self.fractional, self.seconds = math.modf(time)
        self.seconds += SECONDS_OFFSET
        self.fractional *= 1e9 # XXX: confirm
