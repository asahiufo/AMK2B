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
This test verifies that the binary representation of Open Sound Control types
that are delivered in packets are properly deserialized into appropriate Python
types.

"""

import struct
import time
import unittest

from oscd import Deserializer


class DeserializerTestCase(unittest.TestCase):
    def setUp(self):
        self.d = Deserializer()

    def test_deserialize_blob(self):
        """Test deserializing a blob."""
        size = b'\x00\x00\x00\x04'
        data = b'\x00\x00\x00\x00'
        remainder = b'\x00\x00\x00\x02'
        self.assertEqual(self.d.deserialize_blob(size + data + remainder),
                         (data, remainder))

    def test_deserialize_bundle(self):
        """Test deserializing a bundle."""
        data = b'#bundle\x00\x00\x00\x00\x00\x00\x00\x00\x01\x00\x00\x00\x10/test\x00\x00\x00,i\x00\x00\x00\x00\x00\x05'
        self.assertListEqual(self.d.deserialize_bundle(data),
                             ['#bundle', 0.0, ['/test', ',i', 5]])

    def test_deserialize_float32(self):
        """
        Test deserializing the next 4 bytes of a packet as a 32-bit float.

        """
        data = b'A\x00\x00\x00'
        remainder = b'\x00\x00\x00\x02'
        self.assertEqual(self.d.deserialize_float32(data + remainder),
                         (8.0, remainder))

    def test_deserialize_float32_invalid_size(self):
        """
        Test deserializing the next 4 bytes of an invalid packet as a 32-bit
        float.

        """
        self.assertRaises(ValueError, self.d.deserialize_float32, b'')

    def test_deserialize_int32(self):
        """
        Test deserializing the next 4 bytes of a packet as a 32-bit integer.

        """
        data = b'\x00\x00\x00\x04'
        remainder = b'\x00\x00\x00\x02'
        self.assertEqual(self.d.deserialize_int32(data + remainder),
                         (4, remainder))

    def test_deserialize_int32_invalid_size(self):
        """
        Test deserializing the next 4 bytes of an invalid packet as a 32-bit
        integer.

        """
        self.assertRaises(ValueError, self.d.deserialize_int32, b'')

    #def test_deserialize_message(self):
        """Test deserializing a packet into a message."""
        data = b'/test\x00\x00\x00,i\x00\x00\x00\x00\x00\x05'
        self.assertListEqual(self.d.deserialize_message(data),
                             ['/test', ',i', 5])

    #def test_deserialize_message_invalid_typetag(self):
        """
        Test deserializing a packet into a message when a comma is missing
        in the typetag.

        """
        data = b'/test\x00\x00\x00i\x00\x00\x00\x00\x00\x00\x05'
        self.assertRaises(ValueError, self.d.deserialize_message, data)

    def test_deserialize_string(self):
        """Test deserializing a string."""
        data = b'test\x00\x00\x00\x00'
        remainder = b'\x00\x00\x00\x02'
        self.assertEqual(self.d.deserialize_string(data + remainder),
                         ('test', remainder))

    def test_deserialize_timetag_immediate(self):
        """
        Test deserializing the next 8 bytes of a packet as a timetag when it is
        specified to be resolved immediately.

        """
        data = b'\x00\x00\x00\x00\x00\x00\x00\x01'
        remainder = b'\x00\x00\x00\x02'
        self.assertEqual(self.d.deserialize_timetag(data + remainder),
                         (0.0, remainder))


if __name__ == '__main__':
    unittest.main()
