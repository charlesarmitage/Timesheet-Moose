import reportoutput
import unittest
from System import DateTime
import datetime
import workinghours

def new_workinghours(start, end):
    hours = workinghours.WorkingHours()
    hours.start = datetime.datetime.strptime(start, '%H:%M').time()
    hours.end = datetime.datetime.strptime(end, '%H:%M').time()
    return hours 

class MockReportAccessor():

	def __init__(self):
		self.read_start_time = DateTime.MinValue
		self.read_end_time = DateTime.MinValue
		self.hours_written = []

	def WriteStartTime(self, time):
		time = DateTime.Now.Date + time.TimeOfDay
		self.hours_written.append(time)

	def WriteEndTime(self, time):
		time = DateTime.Now.Date + time.TimeOfDay
		self.hours_written.append(time)

	def ReadStartTime(self, date):
		return self.read_start_time

	def ReadEndTime(self, date):
		return self.read_end_time

class TestReportOutput(unittest.TestCase):

	def setUp(self):
		self.accessor = MockReportAccessor()
		self.writer = reportoutput.ReportWriter(self.accessor)

	def test_should_write_set_of_working_hours_to_report(self):
		self.writer.write(new_workinghours("08:15", "16:15"))

		assert len(self.accessor.hours_written) == 2
		assert self.accessor.hours_written[0] == DateTime.Parse("8:15")
		assert self.accessor.hours_written[1] == DateTime.Parse("16:15")

	def test_should_not_write_starting_hours_when_hours_exist(self):
		self.accessor.read_start_time = DateTime.Parse("09:00")

		self.writer.write(new_workinghours("08:15", "16:15"))

		assert len(self.accessor.hours_written) == 1 
		assert self.accessor.hours_written[0] == DateTime.Parse("16:15")

	def test_should_not_write_end_time_when_already_exists_in_report(self):
		self.accessor.read_end_time = DateTime.Parse("17:00")

		self.writer.write(new_workinghours("08:15", "16:15"))

		assert len(self.accessor.hours_written) == 1
		assert self.accessor.hours_written[0] == DateTime.Parse("8:15")
