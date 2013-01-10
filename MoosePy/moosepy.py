import clr
clr.AddReferenceToFile('moose')
import Moose
import System
from System import Environment
from itertools import groupby
import datetime
import hours_estimator
import hours_aggregation

def read_hours():   
    logFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\Timesheet.log"
    print 'Using: %s' % logFile
    reader = Moose.TextTimeLogReader(logFile)
    rawLines = reader.ReadAllLines()
    return [Moose.TextTimeLogParser(line) for line in rawLines]

def process_hours(raw_hours):
    normalized_hours = [hours_aggregation.normalize_start_and_end_times(hours) for hours in raw_hours]
    days = hours_aggregation.group_hours_by_day(normalized_hours)
    return days

def print_hours(days):
    weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
    
    for day in days:
        date = datetime.datetime(day[0].StartTime.Date)
        print '\n%s, %s: Options: %i' % (date.date(), weekdays[date.weekday()], len(day))
        print 'Estimation: %s' % (hours_estimator.estimate_hours(day))
        for hours in day:
            print '\t %s %s' % (datetime.datetime(hours.StartTime).time(), datetime.datetime(hours.EndTime).time())

if __name__ == '__main__':
    hours = read_hours()
    hours_grouped_by_day = process_hours(hours)
    print_hours(hours_grouped_by_day)
