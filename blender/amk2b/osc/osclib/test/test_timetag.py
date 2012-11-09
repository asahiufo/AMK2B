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
This test verifies that the Open Sound Control timetags' behavior matches the
specification of Open Sound Control.

"""

import math
import time
import unittest

from osclib import timetag, SECONDS_OFFSET


class TimetagTestCase(unittest.TestCase):
    def test_init(self):
        data = time.time()
        fractional, seconds = math.modf(data)

        tag = timetag(data)
        self.assertEquals(tag.seconds, seconds + SECONDS_OFFSET)
        self.assertEquals(tag.fractional, fractional * 1e9)


if __name__ == '__main__':
    unittest.main()
