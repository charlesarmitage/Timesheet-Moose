import collections
import unittest
import re
import datetime
import time

def parsedate(line):
    pattern = r"../../.."
    match = re.search(pattern, line)
    date = datetime.datetime.strptime(match.group(0), "%m/%d/%Y")
    return date

def parsestarttime(line):
    pattern = r"In:......"
    match = re.search(pattern, line)
    t = match.group(0).lstrip('In: ')
    starttime = datetime.datetime.strptime(t, "%H:%M")
    return starttime.time()

def parseendtime(line):
    pattern = r"Out:.*"
    match = re.search(pattern, line)
    t = match.group(0).lstrip('Out:')
    starttime = datetime.datetime.strptime(t, "%H:%M")
    return starttime.time()

class TestLogReader(unittest.TestCase):

    def test_can_read_all_lines_from_log_file(self):
        logfile = open("Tests\SimpleLog.log", 'r')
        lines = logfile.readlines()
        assert lines[0] == "12/06/12 In: 09:00 Out: 17:00\n"
        assert lines[1] == "13/06/12 In: 08:00 Out: 16:00\n"
        assert lines[2] =="Invalid line\n"
        assert lines[3] == "14/06/12 In: 07:00 Out: \n"
        assert lines[4] == "   \n"

    def test_can_parse_date_from_single_line(self):
        line = "12/18/12 In: 09:00 Out: 17:00"
        date = parsedate(line)
        assert date.date() == datetime.date(2012, 12, 18)

    def test_can_parse_starttime_from_single_line(self):
        line = "12/18/12 In: 09:00 Out: 17:00"
        starttime = parsestarttime(line)
        assert starttime == datetime.time(9, 0, 0)

    def test_can_parse_endtime_from_single_line(self):
        line = "12/18/12 In: 09:00 Out: 17:00"
        endtime = parseendtime(line)
        assert endtime == datetime.time(17, 0, 0)