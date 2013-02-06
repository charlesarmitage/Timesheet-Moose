import collections
from System import DateTime
from System import DayOfWeek

def filter_by_month(hours, month):
    filtered = [hour for hour in hours if hour.date.month == month]
    return filtered

def isduringweek(day):
	saturday = 6
	sunday = 7
	return day.date.isoweekday() != saturday and day.date.isoweekday() != sunday

def remove_weekends(hours):
    filtered = [ day for day in hours if isduringweek(day)]
    return filtered