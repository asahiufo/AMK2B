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
This test verifies that Open Sound Control bundles correctly adds bundle
elements into its internal list.

"""

import unittest

from osclib import Bundle, Message


class BundleTestCase(unittest.TestCase):
    def setUp(self):
        self.bundle = Bundle()

    def test_append_bundle(self):
        """Test appending a bundle."""
        data = Bundle()
        self.bundle.append(data)
        self.assertListEqual(self.bundle.elements, [data])

    def test_append_message(self):
        """Test appending a message."""
        data = Message()
        self.bundle.append(data)
        self.assertListEqual(self.bundle.elements, [data])

    def test_append_invalid_element(self):
        """Test appending an invalid element."""
        self.assertRaises(TypeError, self.bundle.append, {})


if __name__ == '__main__':
    unittest.main()
