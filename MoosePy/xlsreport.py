import os
from datetime import date, datetime
import win32com.client as win32
import workbooknav

class xlscell:
	def __init__(self):
		self.column = 'c'
		self.row = 1

class xlsreport:
	def __init__(self, filename):
		print filename
		self.filename = filename
		self.cells = []
		self.excel = win32.gencache.EnsureDispatch('Excel.Application')
		self.workbook = self.excel.Workbooks.Open(filename)
		self.excel.Visible = True

	def write_start(self, time):
		cell = self.getstarttimecell(time)
		cell.Value = 1000.0 #time
		print cell.Value

	def close(self):
		print "Close"
		self.workbook.Save()
		#self.workbook.Close(SaveChanges=True)
		#self.excel.Quit()

	def getstarttimecell(self, time):
		return self.gettimesheetcell(time, 'C')

	def gettimesheetcell(self, day, column):
		rowposition = workbooknav.workbooknavigator(day)
		cell = "%s%s" % (column.upper(), rowposition.row())
		print rowposition.sheetname()
		print cell
		sheet = self.workbook.Sheets(rowposition.sheetname())
		actualCell = sheet.Range(cell)
		return actualCell
