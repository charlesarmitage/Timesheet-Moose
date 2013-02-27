import unittest
import os
from datetime import date, datetime
import win32com.client as win32
import workbooknav
import xlsreport
	

class TestXLSReportWriter(unittest.TestCase):
	#def test_can_read_from_test_timesheet(self):
	#	assert os.path.isfile("TestTimesheet.xlsx")
#		val = self.getvaluefromcell("TestTimesheet.xlsx", "January", "C5")
#		assert val == datetime(2012, 11, 19)

	def test_should_write_time_to_correct_cell_for_Monday_starttime(self):
		report = xlsreport.xlsreport("TestTimesheet.xlsx")

		mondayStartTime = datetime(2012, 12, 17, 9, 30, 0)
		report.write_start(mondayStartTime)
		report.close()

		#val = self.getvaluefromcell("TestTimesheet.xlsx", "January", "C7")
		#assert val == monday_start
		
	def getvaluefromcell(self, book, sheet, cell):
		print "%s %s" % (sheet, cell)

		excel = win32.gencache.EnsureDispatch('Excel.Application')
		wb = excel.Workbooks.Open(book)
		excel.Visible = True

		ws = wb.Sheets(sheet)
		value =  ws.Range(cell, cell)
		d = datetime.strptime(str(value), "%y/%d/%m %H:%M:%S")
		wb.Close(SaveChanges = False)
		return d

if __name__ == '__main__':
    unittest.main()