from datetime import datetime

class workbooknavigator:
	def __init__(self, requested_date):
		self.startingdates = [
		("January", datetime(2012, 12, 17)),
		("February", datetime(2013, 1, 21)),
		("March", datetime(2013, 2, 25)),
		("April", datetime(2013, 3, 25)),
		("May", datetime(2013, 4, 22)),
		("June", datetime(2013, 5, 20)),
		("July", datetime(2013, 6, 24)),
		("August", datetime(2013, 7, 22)),
		("September", datetime(2013, 8, 19)),
		("October", datetime(2013, 9, 23)),
		("November", datetime(2013, 10, 21)),
		("December", datetime(2013, 11, 25))
		]
		self.requested_date = requested_date
		self.setsheet_from_date(self.requested_date)
		self.rowsbetweenweeks = 16

	def sheetname(self):
		return self.sheet_name

	def row(self):
		return self.rowofstartofweek(self.requested_date) - self.noofdaysfrom_monday(self.requested_date)

	def noofdaysfrom_monday(self, day):
		diff = day - self.startofmonthdate.date()
		return diff.days % 7

	def rowofstartofweek(self, day):
		diff = day - self.startofmonthdate.date()
		noofweeks = diff.days / 7
		return (noofweeks * self.rowsbetweenweeks) + 7

	def setsheet_from_date(self, day):
		self.requested_date = day.date()
		self.set_default_start_dates()

		for d in reversed(self.startingdates):
			if day.date() <= d[1].date():
				self.sheet_name = d[0]
				self.startofmonthdate = d[1]
				self.startofmonthnumber = self.startingdates.index(d)

	def set_default_start_dates(self):
		self.sheet_name = "January"
		self.startofmonthdate = datetime(datetime.now().year, 1, 1)
		self.startofmonthnumber = 0
