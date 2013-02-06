import collections
import unittest
import Moose
import hours_filtering
import workinghours
import datetime
from System import DateTime
from System import DayOfWeek

def new_working_hours_on(date, start, end):
    hours = workinghours.WorkingHours()
    hours.date = datetime.datetime.strptime(date, '%d/%m/%y')
    hours.start = datetime.datetime.strptime(start, '%M:%H')
    hours.end = datetime.datetime.strptime(end, '%M:%H')
    return hours

class TestHoursFiltering(unittest.TestCase):

    def test_can_remove_saturdays(self):
    	friday = new_working_hours_on("1/2/2013","08:15","16:15")
    	saturday = new_working_hours_on("2/2/2013","08:15","16:15")    	
    	hours = [friday, saturday]

    	hours = hours_filtering.remove_weekends(hours)
    	assert len(hours) == 1
    	assert hours[0] == friday

	def test_can_remove_saturdays(self):
		friday = new_working_hours_on("1/2/2013","08:15","16:15")
    	sunday = new_working_hours_on("3/2/2013","08:15","16:15")    	
    	hours = [friday, sunday]

    	hours = hours_filtering.remove_weekends(hours)
    	assert len(hours) == 1
    	assert hours[0] == friday

	def test_does_not_filter_weekdays(self):
		pass
		#monday = new_working_hours_on("28/1/2013", "08:15", "16:15")
		#tuesday = new_working_hours_on("29/01/2013", "08:15", "16:15")
		#wednesday = new_working_hours_on("30/01/2013", "08:15", "16:15")
		#thursday = new_working_hours_on("31/1/2013", "08:15", "16:15")
		#friday = new_working_hours_on("1/2/2013","08:15","16:15")

    	#hours = [ monday, tuesday, wednesday, thursday, friday]

    	#hours = moosepy.remove_weekends(hours)
    	#assert len(hours) == 5
