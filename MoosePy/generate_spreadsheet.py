import sys
import workbooknav
import reportoutput
import estimatedhoursinweeks

def write_hours_to_report(hours):
    xls_file = r"C:\git\Timesheet-Moose\MooseXLSReports\Testtimesheet.xlsx"
    writer = reportoutput.build_ipy_writer(xls_file)
    for estimated_hours in hours:
        writer.write(estimated_hours)

def write_hours(weeks):
	hours = []
	for week in weeks:
		hours.extend(week)

	print hours
	write_hours_to_report(hours)

if __name__ == '__main__':
	if 'weeks' not in globals():
		logfile = sys.argv[1]
		weeks = estimatedhoursinweeks.generate_estimated_hours(logfile)
	
	write_hours(weeks)
