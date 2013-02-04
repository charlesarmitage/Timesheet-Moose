import collections
from System import DateTime
from System import DayOfWeek

def filter_by_month(hours, month):
    filtered = [hour for hour in hours if hour.StartTime.Month == month]
    return filtered

def isduringweek(day):
    return day.StartTime.DayOfWeek != DayOfWeek.Saturday and day.StartTime.DayOfWeek != DayOfWeek.Sunday

def remove_weekends(hours):
    filtered = [ day for day in hours if isduringweek(day)]
    return filtered