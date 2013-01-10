import Moose
from itertools import groupby

def group_hours_by_day(hours):
    days = []
    for date, day in groupby(hours, key=lambda d: d.StartTime.Date):
        days.append(list(day))
    return days

def normalize_start_and_end_times(rawHours):
    normalizer = Moose.WorkingDayCalculator()
    normalizer.AddStartTime(rawHours.StartTime)
    normalizer.AddEndTime(rawHours.EndTime)
    hours = normalizer.CalculateWorkingHours()
    return hours
