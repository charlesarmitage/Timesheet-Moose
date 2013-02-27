import collections
import workbooknav

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

def filter_by__current_worksheet_month(day, raw_hours):
    workbook = workbooknav.workbooknavigator(day)
    month = workbook.startofmonthnumber

    hours = filter_by_month(raw_hours, month)
    hours.extend( filter_by_month(raw_hours, month - 1) ) # TODO: Handle january
    return hours