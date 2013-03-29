import sys
import shutil
import workbooknav
import reportoutput
import textreportwriter
import estimatedhoursinweeks

def build_text_writer(xls_file):
	return textreportwriter.build_text_writer(xls_file)

def build_xls_writer(xls_file):
	return reportoutput.build_ipy_writer(xls_file)

def write_hours_to_report(hours):
    for estimated_hours in hours:
        writer.write(estimated_hours)

    # TODO: Implement saving the file as an .xlsx file
    #writer.close()

def write_hours(xls_file, weeks):
	hours = []
	for week in weeks:
		hours.extend(week)

	write_hours_to_report(hours)

def generate_spreadsheet(xls_file, weeks):
	if not xls_file:
		return ""

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

	writer = textreportwriter.build_text_writer(xls_file)
	generate_spreadsheet(xls_file, weeks)