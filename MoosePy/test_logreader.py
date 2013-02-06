import unittest
import re
import datetime
import time
import hours_input

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
        line = "08/12/12 In: 09:00 Out: 17:00"
        date = hours_input.parsedate(line)
        assert date == datetime.date(2012, 12, 8)

    def test_can_parse_starttime_from_single_line(self):
        line = "12/18/12 In: 09:00 Out: 17:00"
        starttime = hours_input.parsestarttime(line)
        assert starttime == datetime.time(9, 0, 0)

    def test_can_parse_endtime_from_single_line(self):
        line = "12/18/12 In: 09:00 Out: 17:00"
        endtime = hours_input.parseendtime(line)
        assert endtime == datetime.time(17, 0, 0)

    def test_can_parse_startandendtime_from_line(self):
        line = "12/18/12 In: 09:23 Out: 17:45"
        starttime = hours_input.parsestarttime(line)
        endtime = hours_input.parseendtime(line)
        assert starttime == datetime.time(9, 23, 0)
        assert endtime == datetime.time(17, 45, 0)

    def test_returns_none_for_invalid_line(self):
        line = "Invalid line"
        date = hours_input.parsedate(line)
        starttime = hours_input.parsestarttime(line)
        endtime = hours_input.parseendtime(line)

        assert date == None
        assert starttime == None
        assert endtime == None

    def test_parsing_a_line_should_produce_a_list_of_working_hours(self):
        line = "18/12/12 In: 09:23 Out: 17:45"

        hourslist = hours_input.parse([line])

        assert len(hourslist) == 1
        assert hourslist[0].date == datetime.date(2012, 12, 18)
        assert hourslist[0].start == datetime.time(9, 23)
        assert hourslist[0].end == datetime.time(17, 45)

    def test_parsing_a_list_of_lines_produces_a_list_of_working_periods(self):
        lines = ["18/12/12 In: 09:23 Out: 17:45",
                "12/19/12 In: 09:45 Out: 17:30"]

        hourslist = hours_input.parse(lines)

        assert len(hourslist) == 2
        assert hourslist[0].date == datetime.date(2012, 12, 18)
        assert hourslist[0].start == datetime.time(9, 23)
        assert hourslist[0].end == datetime.time(17, 45)
        assert hourslist[1].date == datetime.date(2012, 12, 19)
        assert hourslist[1].start == datetime.time(9, 45)
        assert hourslist[1].end == datetime.time(17, 30)

    def test_parsing_a_list_with_an_invalid_date_should_ignore_the_invalid_line(self):
        lines = ["Invalid date: In: 09:23 Out: 17:45",
                "19/12/12 In: 09:45 Out: 17:30"]

        hourslist = hours_input.parse(lines)

        assert len(hourslist) == 1
        assert hourslist[0].date == datetime.date(2012, 12, 19)
        assert hourslist[0].start == datetime.time(9, 45)
        assert hourslist[0].end == datetime.time(17, 30)

    def test_parsing_a_list_with_an_invalid_start_should_ignore_the_invalid_line(self):
        lines = ["12/18/12 Invalid Out: 17:45",
                "19/12/12 In: 09:45 Out: 17:30"]

        hourslist = hours_input.parse(lines)

        assert len(hourslist) == 1
        assert hourslist[0].date == datetime.date(2012, 12, 19)
        assert hourslist[0].start == datetime.time(9, 45)
        assert hourslist[0].end == datetime.time(17, 30)

    def test_parsing_list_with_invalid_end_time_should_not_ignore_the_invalid_endtime(self):
        lines = ["18/12/12 In: 09:23",
                "19/12/12 In: 09:45 Out: 17:30"]

        hourslist = hours_input.parse(lines)

        assert len(hourslist) == 2
        assert hourslist[0].date == datetime.date(2012, 12, 18)
        assert hourslist[0].start == datetime.time(9, 23)
        assert hourslist[0].end == None
        assert hourslist[1].date == datetime.date(2012, 12, 19)
        assert hourslist[1].start == datetime.time(9, 45)
        assert hourslist[1].end == datetime.time(17, 30)       
