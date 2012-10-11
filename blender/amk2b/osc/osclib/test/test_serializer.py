#! /usr/bin/env python

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
This test verifies that representation of Open Soun Control types in Python are
properly serialized into its binary representation in order to be delivered in
packets.

"""

import struct
import time
import unittest

from osclib import Bundle, Message, Serializer, TimeTag


class SerializerTestCase(unittest.TestCase):
    def setUp(self):
        self.serial = Serializer()

    def test_serialize_bundle(self):
        """Test serializing a bundle."""
        message = Message('/test', 5)
        bundle = Bundle(None, message)
        self.assertEqual(self.serial.serialize_bundle(bundle),
        b'#bundle\x00\x00\x00\x00\x00\x00\x00\x00\x01\x00\x00\x00\x10/test\x00\x00\x00,i\x00\x00\x00\x00\x00\x05')

    def test_serialize_message(self):
        """Test serializing a message."""
        message = Message('/test', 5)
        self.assertEqual(self.serial.serialize_message(message),
                         b'/test\x00\x00\x00,i\x00\x00\x00\x00\x00\x05')

    def test_serialize_blob_str(self):
        """Test serializing a blob with a string input."""
        self.assertEqual(self.serial.serialize_blob('test'),
                         b'\x00\x00\x00\x04test')

    def test_serialize_blob_int(self):
        """Test serializing a blob with an integer input."""
        self.assertEqual(self.serial.serialize_blob(5),
                         b'\x00\x00\x00\x04\x00\x00\x00\x05')

    def test_serialize_float32(self):
        """Test serializing a float32."""
        self.assertEqual(self.serial.serialize_float32(5.0), b'@\xa0\x00\x00')

    def test_serialize_int32(self):
        """Test serializing an int."""
        self.assertEqual(self.serial.serialize_int32(5), b'\x00\x00\x00\x05')

    def test_serialize_string(self):
        """Test serializing a string."""
        self.assertEqual(self.serial.serialize_string('test'),
                         b'test\x00\x00\x00\x00')

    def test_serialize_timetag(self):
        """
        Test serializing a timetag. Impossible to check the actual data without
        using the implementation. The length of the data is checked instead.

        """
        data = time.time()
        timetag = TimeTag(data)
        self.assertEqual(len(self.serial.serialize_timetag(timetag)), 8)

    def test_serialize_timetag_immediate(self):
        """Test serializing a timetag for immediate."""
        self.assertEqual(self.serial.serialize_timetag(None),
                         b'\x00\x00\x00\x00\x00\x00\x00\x01')


if __name__ == '__main__':
    unittest.main()
