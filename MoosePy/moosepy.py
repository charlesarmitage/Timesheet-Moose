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
import hours_filtering
import reportoutput
from System import DayOfWeek
import hours_input

def read_hours():   
    logFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\Timesheet.log"
    print 'Using: %s' % logFile
    log = open(logFile, 'r')
    lines = log.readlines()
    log.close()
    return hours_input.parse(lines)

def get_estimated_hours(hours):
    estimated = [hours_estimator.estimate_hours(working_hours) for working_hours in hours]
    return estimated

def process_hours(raw_hours):
    normalized_hours = [hours_aggregation.normalize_start_and_end_times(hours) for hours in raw_hours]
    days = hours_aggregation.group_hours_by_day(normalized_hours)
    return days

def print_hours(days):
    weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
    
    for day in days:
        date = day[0].date
        print '\n%s, %s: Options: %i' % (date, weekdays[date.weekday()], len(day))
        estimate = hours_estimator.estimate_hours(day)
        print 'Estimation: %s' % estimate
        for hours in day:
            print '\t %s %s' % (hours.start, hours.end)

def should_write_to_report():
    print "Fill in spreadsheet? (Y/N)"
    fill = sys.stdin.read(1).lower()
    if(fill == 'y'):
        return True
    else:
        print "No report written."
        return False

def write_hours_to_report(hours):
    xls_file = "C:\Users\carmitage\Documents\AdminDocs\Origin Timesheet 2013 - Copy.xlsx"
    xls_report = MooseXLSReports.XlsReport(xls_file)
    writer = reportoutput.ReportWriter(xls_report)
    for estimated_hours in hours:
        print "Written: %s" % estimated_hours
        writer.write(estimated_hours)

if __name__ == '__main__':
    hours = read_hours()

    month = 1
    hours = hours_filtering.filter_by_month(hours, month)
    hours = hours_filtering.remove_weekends(hours)
    hours_grouped_by_day = process_hours(hours)
    print_hours(hours_grouped_by_day)

    if should_write_to_report():
        estimated_hours = get_estimated_hours(hours_grouped_by_day)
        write_hours_to_report(estimated_hours)

    print sys.argv
