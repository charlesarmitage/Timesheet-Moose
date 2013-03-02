import sys
import System
from System import Environment
import datetime
import hours_estimator
import hours_aggregation
import hours_filtering
import hours_normalization
import hours_feed
import reportoutput
import hours_input
import workbooknav

def read_hours():   
    logFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\Timesheet.log"
    print 'Using: %s' % logFile
    log = open(logFile, 'r')
    lines = log.readlines()
    log.close()
    return hours_input.parse(lines)

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
    writer = reportoutput.build_ipy_writer(xls_file)
    for estimated_hours in hours:
        print "Written: %s" % estimated_hours
        writer.write(estimated_hours)

if __name__ == '__main__':
    raw_hours = read_hours()
    hours_grouped_by_day = hours_feed.calculate_estimated_hours(datetime.datetime.today(), raw_hours)
    print_hours(hours_grouped_by_day)

    if should_write_to_report():
        estimated_hours = hours_estimator.get_estimated_hours(hours_grouped_by_day)
        write_hours_to_report(estimated_hours)

    print sys.argv
