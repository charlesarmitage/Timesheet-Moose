import unittest
import hours_estimator
import Moose
from System import DateTime

def new_workinghours(start, end):
    starttime = DateTime.Parse(start)
    endtime = DateTime.Parse(end)
    return Moose.WorkingHours(starttime, endtime) 

class TestHoursEstimate(unittest.TestCase):

    def test_days_with_single_start_and_end_time(self):
        day = [new_workinghours('8:15', '16:45')]
        estimated_hours = hours_estimator.estimate_hours(day)

        assert estimated_hours.StartTime == DateTime.Parse('8:15')
        assert estimated_hours.EndTime == DateTime.Parse('16:45')
        assert len(estimated_hours.PotentialStartTimes()) == 1
        assert len(estimated_hours.PotentialEndTimes()) == 1
    
    def test_days_with_2_start_and_end_times_select_closest_to_office_hours(self):
        hours_in_day = [new_workinghours('8:00', '17:00'), new_workinghours('20:00', '21:00')]
        estimated_hours = hours_estimator.estimate_hours(hours_in_day)

        assert estimated_hours.StartTime == DateTime.Parse('8:00')
        assert estimated_hours.EndTime == DateTime.Parse('17:00')
        assert len(estimated_hours.PotentialStartTimes()) == 2
        assert len(estimated_hours.PotentialEndTimes()) == 2

    def test_should_use_hours_closest_to_office_hours_for_end_times(self):
        hours_in_day = [new_workinghours('8:00', '10:00'), new_workinghours('10:00', '17:00')]
        estimated_hours = hours_estimator.estimate_hours(hours_in_day)

        assert estimated_hours.StartTime == DateTime.Parse('8:00')
        assert estimated_hours.EndTime == DateTime.Parse('17:00')
        assert len(estimated_hours.PotentialStartTimes()) == 2
        assert len(estimated_hours.PotentialEndTimes()) == 2

    def test_should_use_hours_closest_to_office_hours_for_start_times(self):
        hours_in_day = [new_workinghours('6:00', '7:30'), new_workinghours('7:45', '17:00')]
        estimated_hours = hours_estimator.estimate_hours(hours_in_day)

        assert estimated_hours.StartTime == DateTime.Parse('7:45')
        assert estimated_hours.EndTime == DateTime.Parse('17:00')
        assert len(estimated_hours.PotentialStartTimes()) == 2
        assert len(estimated_hours.PotentialEndTimes()) == 2

    def test_should_return_start_and_end_times_even_when_they_are_not_in_office_hours(self):
        hours_in_day = [new_workinghours('10:00', '14:00'), new_workinghours('21:00', '22:00')]
        estimated_hours = hours_estimator.estimate_hours(hours_in_day)

        assert estimated_hours.StartTime == DateTime.Parse('10:00')
        assert estimated_hours.EndTime == DateTime.Parse('22:00')
        assert len(estimated_hours.PotentialStartTimes()) == 2
        assert len(estimated_hours.PotentialEndTimes()) == 2

if __name__ == '__main__':
    unittest.main()

