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

if __name__ == '__main__':
	if 'weeks' not in globals():
		logfile = sys.argv[1]
		weeks = estimatedhoursinweeks.generate_estimated_hours(logfile)

	if 'xls_file'  not in globals():
		xls_file = sys.argv[2]

	#Copy file
	output_file = r".\generated_timesheet.xlsx"
	shutil.copyfile(xls_file, output_file)

	write_hours(output_file, weeks)
