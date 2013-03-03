# Script for MVC WebApp
import System
from System import Environment
import datetime
import sys
sys.path.append(r"C:\Users\carmitage\Dropbox\hg\Timesheet-Moose\MoosePy")
sys.path.append(r"C:\Python27\Lib")
from workinghours import hours_feed
from workinghours import hours_input
from workinghours import hours_estimator

logfile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + r"\Timesheet.log"
raw_hours = hours_input.readfromlog(logfile)
hours = hours_feed.calculate_estimated_hours(datetime.datetime.today(), raw_hours)
hours = hours_estimator.get_estimated_hours(hours)
