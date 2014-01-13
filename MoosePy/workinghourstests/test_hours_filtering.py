import collections
import unittest
from workinghours import hours_filtering
from workinghours import workhours
import datetime
from datetime import datetime
import workbooknav

def new_working_hours_on(date, start, end):
    hours = workhours.WorkingHours()
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
        previousmonth = workhours.build_from_date(datetime(2013, 2, 28))
        previousworksheetmonth = workhours.build_from_date(datetime(2013, 3, 15))
        worksheetmonthstartdate = workhours.build_from_date(datetime(2013, 3, 26))
        nextworksheetmonthdate = workhours.build_from_date(datetime(2013, 4, 1))
        nextcalendarmonth = workhours.build_from_date(datetime(2013, 5, 2))

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

    def test_will_filter_for_january(self):
        start_of_january = workhours.build_from_date(datetime(2014, 1, 1))
        hours = [start_of_january]

        result = hours_filtering.filter_by__current_worksheet_month(datetime(2014, 1, 1), hours)

        assert start_of_january in result

    # TODO: move into seperate test file
    def test_will_use_default_date_from_workbooknav(self):
        workbook = workbooknav.workbooknavigator(datetime(3000, 3, 22))
        
        assert workbook.startofmonthnumber == 0
        assert workbook.sheet_name == "January"
        assert workbook.startofmonthdate == datetime(datetime.now().year, 1, 1)

