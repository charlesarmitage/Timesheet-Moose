import collections
import unittest
from workinghours import hours_aggregation
import workinghours
import datetime
from System import DateTime

def new_working_hours_on(date, start, end):
    hours = workinghours.WorkingHours()
    hours.date = datetime.datetime.strptime(date, '%d/%m/%y')
    hours.start = datetime.datetime.strptime(start, '%H:%M')
    hours.end = datetime.datetime.strptime(end, '%H:%M')
    return [hours]

def new_workinghours(start, end):
    hours = workinghours.WorkingHours()
    hours.start = datetime.datetime.strptime(start, '%H:%M')
    hours.end = datetime.datetime.strptime(end, '%H:%M')
    return [hours]

def is_single_dimension_collection(list):
    for element in list:
        if isinstance(element, collections.Iterable):
            return False        
    return True
    
class TestHoursAggregation(unittest.TestCase):
    def test_initial_hours_are_single_dimension_list(self):
        hours = new_workinghours('8:15', '16:15')
        hours.extend(new_workinghours('20:15', '16:15'))

        assert is_single_dimension_collection(hours)   

    def test_hours_on_same_day_are_grouped_into_two_dimension_list(self):
        hours = new_workinghours('8:15', '16:15')
        hours.extend(new_workinghours('20:15', '16:15'))
        
        grouped_hours = hours_aggregation.group_hours_by_day(hours)    
        assert not is_single_dimension_collection(grouped_hours)

    def test_should_group_into_two_element_list_when_two_sets_of_hours_on_same_day(self):
        hours = new_workinghours('8:15', '16:15')
        hours.extend(new_workinghours('20:15', '16:15'))
        
        grouped_hours = hours_aggregation.group_hours_by_day(hours)  
        assert len(grouped_hours) == 1
        assert len(grouped_hours[0]) == 2
        
    def test_should_group_into_seperate_lists_when_hours_are_on_different_days(self):
        hours = new_working_hours_on('28/7/12', '8:30', '16:45')
        hours.extend(new_working_hours_on('28/7/12', '20:30', '21:45'))
        hours.extend(new_working_hours_on('29/7/12', '9:15', '17:00'))
        
        grouped_hours = hours_aggregation.group_hours_by_day(hours)
        assert len(grouped_hours) == 2
        assert len(grouped_hours[0]) == 2
        assert len(grouped_hours[1]) == 1
        
if __name__ == '__main__':
    unittest.main()

