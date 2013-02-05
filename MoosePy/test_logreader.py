import unittest
import re
import datetime
import time

def parsedate(line):
    match = re.search(r"../../..", line)
    if match != None:
        return datetime.datetime.strptime(match.group(0), "%m/%d/%Y")
    return None

def extracttime(match, prefix):
    t = match.lstrip(prefix)
    starttime = datetime.datetime.strptime(t, "%H:%M")
    return starttime.time()    

def parsestarttime(line):
    match = re.search(r"In:......", line)
    if match != None:
        return extracttime(match.group(0), 'In: ')
    return None

def parseendtime(line):
    match = re.search(r"Out:.*", line)
    if match != None:
        return extracttime(match.group(0), 'Out:')
    return None

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

    def test_can_parse_startandendtime_from_line(self):
        line = "12/18/12 In: 09:23 Out: 17:45"
        starttime = parsestarttime(line)
        endtime = parseendtime(line)
        assert starttime == datetime.time(9, 23, 0)
        assert endtime == datetime.time(17, 45, 0)

    def test_returns_none_for_invalid_line(self):
        line = "Invalid line"
        date = parsedate(line)
        starttime = parsestarttime(line)
        endtime = parseendtime(line)

        assert date == None
        assert starttime == None
        assert endtime == None
