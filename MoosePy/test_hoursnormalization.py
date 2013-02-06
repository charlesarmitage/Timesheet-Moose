import unittest
import datetime
import hours_normalization
import workinghours

class TestNormalization(unittest.TestCase):

    def setUp(self):
        self.hours = workinghours.WorkingHours()

    def test_a_workingday_that_starts_at_9_00_starts_at_9am(self):
        self.hours.start = datetime.time(9, 0, 0)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.start == datetime.time(9, 0, 0)

    def test_a_workingday_that_ends_at_15_00_ends_at_5pm(self):
        self.hours.end = datetime.time(17, 0, 0)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.end == datetime.time(17, 0, 0)

    def test_workingday_starting_at_9_03_starts_at_9am(self):
        self.hours.start = datetime.time(9, 3, 0)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.start == datetime.time(9, 0, 0)

    def test_workingday_ending_at_16_43_ends_at_16_30pm(self):
        self.hours.end = datetime.time(16, 33, 0)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.end == datetime.time(16, 30, 0)

    def test_workingday_ending_at_17_14_ends_at_5_15pm(self):
        self.hours.end = datetime.time(17, 14)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.end == datetime.time(17, 15)

    def test_workingday_ending_at_16_58_ends_at_5pm(self):
        self.hours.end = datetime.time(16, 58)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.end == datetime.time(17, 0)

    def test_workingday_starting_at_9_04_starts_at_9_15am(self):
        self.hours.start = datetime.time(9, 04)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.start == datetime.time(9, 15)

    def test_workingday_starting_at_8_50_starts_at_9am(self):
        self.hours.start = datetime.time(8, 50)

        hours = hours_normalization.normalizehours(self.hours)

        assert hours.start == datetime.time(9, 00)
