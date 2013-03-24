import datetime
import sys
from workinghours import hours_feed
from workinghours import hours_input
from workinghours import hours_estimator
from workinghours import hours_aggregation

def generate_estimated_hours(logfile):
	# logfile variable loaded from executing host
	hours = hours_input.readfromlog(logfile)
	hours = hours_feed.calculate_estimated_hours(datetime.datetime.today(), hours)
	hours = hours_estimator.get_estimated_hours(hours)

	# TODO: Seperate this view processing from the estimated hours model later.
	# Sort by date
	hours = sorted(hours, cmp=lambda lhs, rhs: lhs.date > rhs.date)

	weeks = hours_aggregation.group_hours_by_week(hours)
	#Print
	for weekindex in range(len(weeks)):
		print "Week %s" % weeks[weekindex][0].date.isocalendar()[1]
        	for day in weeks[weekindex]:
        		print day

	return weeks


if __name__ == "__main__":
	if len(sys.argv) == 1:
		print "Usage from command line: estimatehoursinweek <hours log>"
		exit()

	logfile = sys.argv[1]
	weeks = generate_estimated_hours(logfile)
