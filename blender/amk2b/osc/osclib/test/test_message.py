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
This test verifies that Open Sound Control messages correctly adds Open Sound
control types into its internal list.

"""

import unittest

from osclib import Message


class MessageTestCase(unittest.TestCase):
    def setUp(self):
        self.message = Message()

    def test_pattern_valid(self):
        """Test setting the pattern property."""
        pattern = '/test'
        self.message.pattern = pattern
        self.assertEqual(self.message.pattern, pattern)

    def test_pattern_no_slash(self):
        """Test setting the pattern property with no forward-slash."""
        pattern ='test'
        with self.assertRaises(ValueError):
            self.message.pattern = pattern

    def test_append_integer(self):
        """Test appending an integer to the message."""
        data = 5
        self.message.append(data)
        self.assertListEqual(self.message.arguments, [data])
        self.assertListEqual(self.message.typetags, ['i'])

    def test_append_float(self):
        """Test appending a float to the message."""
        data = 5.0
        self.message.append(data)
        self.assertListEqual(self.message.arguments, [data])
        self.assertListEqual(self.message.typetags, ['f'])

    def test_append_string(self):
        """Test appending a string to the message."""
        data = 'test'
        self.message.append(data)
        self.assertListEqual(self.message.arguments, [data])
        self.assertListEqual(self.message.typetags, ['s'])

    def test_append_blob(self):
        """Test appending a blob to the message."""
        data = ('blob', 'b')
        self.message.append(data)
        self.assertListEqual(self.message.arguments, [data[0]])
        self.assertListEqual(self.message.typetags, ['b'])

    def test_append_invalid_tuple(self):
        """Test appending an invalid sequence in a tuple."""
        self.assertRaises(Exception, self.message.append, ('a', 'b', 'c'))

    def test_append_invalid_python_type(self):
        """Test appending an invalid Python type to the message."""
        self.assertRaises(KeyError, self.message.append, {})

    def test_append_invalid_type(self):
        """Test appending an invalid type to the message."""
        self.assertRaises(TypeError, self.message.append, ('test', 'invalid'))


if __name__ == '__main__':
    unittest.main()
