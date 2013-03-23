import clr
import System
from System import DateTime
import reportoutput

def build_text_writer(filename):
	xls_report = TextReportAccessor(filename)
	writer = reportoutput.ReportWriter(xls_report)
	return writer

class TextReportAccessor():

	def __init__(self, filename):
		pass

	def Dispose(self):
		#Close filename
		pass

	def WriteStartTime(self, startime):
		pass

	def WriteEndTime(self, endtime):
		pass

	def ReadStartTime(self, date):
		return DateTime.MinValue

	def ReadEndTime(self, date):
		return DateTime.MinValue
