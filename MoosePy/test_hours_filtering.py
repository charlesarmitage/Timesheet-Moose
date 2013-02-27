import collections
import unittest
import hours_filtering
import workinghours
import datetime
from datetime import datetime
from System import DateTime
from System import DayOfWeek

def new_working_hours_on(date, start, end):
    hours = workinghours.WorkingHours()
    hours.date = datetime.strptime(date, '%d/%m/%Y')
    hours.start = datetime.strptime(start, '%M:%H')
    hours.end = datetime.strptime(end, '%M:%H')
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
        monday = new_working_hours_on("28/01/2013", "08:15", "16:15")
        tuesday = new_working_hours_on("29/01/2013", "08:15", "16:15")
        wednesday = new_working_hours_on("30/01/2013", "08:15", "16:15")
        thursday = new_working_hours_on("31/1/2013", "08:15", "16:15")
        friday = new_working_hours_on("1/2/2013","08:15","16:15")

    	hours = [ monday, tuesday, wednesday, thursday, friday]

    	hours = hours_filtering.remove_weekends(hours)
    	assert len(hours) == 5

    def test_will_filter_dates_based_on_worksheet_months(self):
        previousmonth = workinghours.build_from_date(datetime(2013, 2, 28))
        previousworksheetmonth = workinghours.build_from_date(datetime(2013, 3, 15))
        worksheetmonthstartdate = workinghours.build_from_date(datetime(2013, 3, 26))
        nextworksheetmonthdate = workinghours.build_from_date(datetime(2013, 4, 1))
        nextcalendarmonth = workinghours.build_from_date(datetime(2013, 5, 2))

        hours = [previousmonth,
                previousworksheetmonth,
                worksheetmonthstartdate,
                nextworksheetmonthdate,
                nextcalendarmonth]

        result = hours_filtering.filter_by__current_worksheet_month(datetime(2013, 3, 27), hours)

        assert worksheetmonthstartdate in result
        assert previousworksheetmonth in result
        assert previousmonth not in result
        assert nextcalendarmonth not in result

