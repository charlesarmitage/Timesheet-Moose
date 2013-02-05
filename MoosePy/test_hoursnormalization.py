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

def nextquarter(workingtime):
    quarter = (workingtime.minute / 15) + 1
    quarter = (quarter * 15) % 60
    rolloverhour = workingtime.hour
    if quarter == 0:
        rolloverhour = workingtime.hour + 1
    return workingtime.replace(hour=rolloverhour, minute=quarter)

def is_close_to_nextquarter(nextquarter, currenttime):
    return nextquarter.minute - currenttime.minute <= 3

def is_close_to_previousquarter(previousquarter, currenttime):
    return currenttime.minute - previousquarter.minute <= 3

def normalizehours(hours):
    nextq = nextquarter(hours.end)
    previousq = previousquarter(hours.start)

    if is_close_to_nextquarter(nextq, hours.end):
        hours.end = nextq
    else:
        hours.end = previousquarter(hours.end)

    if is_close_to_previousquarter(previousq, hours.start):
        hours.start = previousquarter(hours.start)
    else:
        hours.start = nextquarter(hours.start)

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

    def test_workingday_ending_at_17_14_ends_at_5_15pm(self):
        self.hours.end = datetime.time(17, 14)

        hours = normalizehours(self.hours)

        assert hours.end == datetime.time(17, 15)

    def test_workingday_ending_at_16_58_ends_at_5pm(self):
        self.hours.end = datetime.time(16, 58)

        hours = normalizehours(self.hours)

        assert hours.end == datetime.time(17, 0)

    def test_workingday_starting_at_9_04_starts_at_9_15am(self):
        self.hours.start = datetime.time(9, 04)

        hours = normalizehours(self.hours)

        assert hours.start == datetime.time(9, 15)

    def test_workingday_starting_at_8_50_starts_at_9am(self):
        self.hours.start = datetime.time(8, 50)

        hours = normalizehours(self.hours)

        assert hours.start == datetime.time(9, 00)
