import sys
import shutil
import workbooknav
import reportoutput
import estimatedhoursinweeks

def write_hours_to_report(xls_file, hours):
    writer = reportoutput.build_ipy_writer(xls_file)
    for estimated_hours in hours:
        writer.write(estimated_hours)

    writer.close()

def write_hours(xls_file, weeks):
	hours = []
	for week in weeks:
		hours.extend(week)

	write_hours_to_report(xls_file, hours)

if __name__ == '__main__':
	if 'weeks' not in globals():
		logfile = sys.argv[1]
		weeks = estimatedhoursinweeks.generate_estimated_hours(logfile)
	
	xls_file = r"C:\git\Timesheet-Moose\MooseXLSReports\Testtimesheet.xlsx"
	shutil.copyfile(xls_file, xls_file + ".tmp.xlsx")

	write_hours(xls_file + ".tmp.xlsx", weeks)
