import sys
import shutil
import workbooknav
import reportoutput
import textreportwriter
import estimatedhoursinweeks

def write_hours_to_report(xls_file, hours):
    #writer = reportoutput.build_ipy_writer(xls_file)
    writer = textreportwriter.build_text_writer(xls_file)
    for estimated_hours in hours:
        writer.write(estimated_hours)

    writer.close()

def write_hours(xls_file, weeks):
	hours = []
	for week in weeks:
		hours.extend(week)

	write_hours_to_report(xls_file, hours)

def generate_spreadsheet(xls_file, weeks):
	output_file = r"C:\git\generated_timesheet.xlsx"
	shutil.copyfile(xls_file, output_file)

	write_hours(output_file, weeks)
	return output_file

if __name__ == '__main__':
	if len(sys.argv) != 3:
		print "Usage from command line: generate_spreadsheet <hours log> <initial spreadsheet>"
		exit()

	logfile = sys.argv[1]
	weeks = estimatedhoursinweeks.generate_estimated_hours(logfile)
	xls_file = sys.argv[2]

	generate_spreadsheet(xls_file, weeks)
