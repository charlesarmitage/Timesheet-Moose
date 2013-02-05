import unittest
import datetime
import time

class WorkingHours():

    def __init__(self):
        self.date = datetime.date.min
        self.start = datetime.datetime.min
        self.end = datetime.datetime.min

def previousquarter(workingtime):
    quarter = workingtime.minute / 15
    quarter = quarter * 15
    return workingtime.replace(minute=quarter)

def normalizehours(hours):
    hours.start = previousquarter(hours.start)
    hours.end = previousquarter(hours.end)

    return hours

class TestNormalization(unittest.TestCase):

    def setUp(self):
        self.hours = WorkingHours()

    def test_a_workingday_that_starts_at_9_00_starts_at_9am(self):
        self.hours.start = datetime.time(9, 0, 0)

        hours = normalizehours(self.hours)

        assert hours.start == datetime.time(9, 0, 0)

    def test_a_workingday_that_ends_at_15_00_ends_at_5pm(self):
        self.hours.end = datetime.time(17, 0, 0)

        hours = normalizehours(self.hours)

        assert hours.end == datetime.time(17, 0, 0)

    def test_workingday_starting_at_9_03_starts_at_9am(self):
        self.hours.start = datetime.time(9, 3, 0)

        hours = normalizehours(self.hours)

        assert hours.start == datetime.time(9, 0, 0)

    def test_workingday_ending_at_16_43_ends_at_16_30pm(self):
        self.hours.end = datetime.time(16, 33, 0)

        hours = normalizehours(self.hours)

        assert hours.end == datetime.time(16, 30, 0)
