import Moose
from System import DateTime

class ReportWriter():

	def __init__(self, report_accessor):
		self.accessor = report_accessor
		pass

	def write(self, hours):
		if self.is_start_time_empty(hours):
			self.accessor.WriteStartTime(hours.StartTime)

		if self.is_end_time_empty(hours):
			self.accessor.WriteEndTime(hours.EndTime)

	def is_start_time_empty(self, date):
		return self.accessor.ReadStartTime(date.StartTime) == DateTime.MinValue

	def is_end_time_empty(self, date):
		return self.accessor.ReadEndTime(date.EndTime) == DateTime.MinValue