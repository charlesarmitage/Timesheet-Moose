import Moose
from itertools import groupby
import hours_normalization

def group_hours_by_day(hours):
    days = []
    for date, day in groupby(hours, key=lambda d: d.date):
        days.append(list(day))
    return days

def normalize_start_and_end_times(rawHours):
	return hours_normalization.normalizehours(rawHours)
