import Moose
import System
from System import DateTime

class ReportWriter():

	def __init__(self, report_accessor):
		self.accessor = report_accessor

	def converttoDateTime(self, date, hours):
		day = DateTime(date.year,
			date.month,
			date.day,
			hours.hour,
			hours.minute,
			hours.second)
		return day	

	def write(self, hours):
		if self.is_start_time_empty(hours):
			day = self.converttoDateTime(hours.date, hours.start)
			self.accessor.WriteStartTime(day)

		if self.is_end_time_empty(hours):
			day = self.converttoDateTime(hours.date, hours.end)
			self.accessor.WriteEndTime(day)

	def is_start_time_empty(self, date):
		day = DateTime(date.date.year, date.date.month, date.date.day)
		return self.accessor.ReadStartTime(day) == DateTime.MinValue

	def is_end_time_empty(self, date):
		day = DateTime(date.date.year, date.date.month, date.date.day)
		return self.accessor.ReadEndTime(day) == DateTime.MinValue