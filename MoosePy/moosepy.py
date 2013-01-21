import sys
import calendar
import clr
clr.AddReferenceToFile('moose')
clr.AddReferenceToFile('MooseXLSReports')
import Moose
import MooseXLSReports
import System
from System import Environment
from itertools import groupby
import datetime
import hours_estimator
import hours_aggregation
import reportoutput

def read_hours():   
    logFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\Timesheet.log"
    print 'Using: %s' % logFile
    reader = Moose.TextTimeLogReader(logFile)
    rawLines = reader.ReadAllLines()
    return [Moose.TextTimeLogParser(line) for line in rawLines]

def filter_by_month(hours, month):
    filtered = [hour for hour in hours if hour.StartTime.Month == month]
    return filtered

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

def write_hours_to_report(hours):
    xls_file = "Origin Timesheet 2013.xlsx"
    xls_report = MooseXLSReports.XlsReport(xls_file)
    writer = reportoutput.ReportWriter(xls_report)
    for working_hours in hours:
        estimated_hours = hours_estimator.estimate_hours(working_hours)
        print "Written: %s" % estimated_hours
        writer.write(estimated_hours)

if __name__ == '__main__':
    hours = read_hours()
    month = 1
    hours = filter_by_month(hours, month)
    hours_grouped_by_day = process_hours(hours)
    print_hours(hours_grouped_by_day)

    print "Fill in spreadsheet? (Y/N)"
    fill = sys.stdin.read(1).lower()
    if(fill == 'y'):
        write_hours_to_report(hours_grouped_by_day)
    else:
        print "No report written. Finished."
    print sys.argv
