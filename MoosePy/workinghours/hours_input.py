import datetime
import re
from workinghours import workhours

def parsedate(line):
    match = re.search(r"../../..", line)
    day = None
    if match != None:
        d = datetime.datetime.strptime(match.group(0), "%d/%m/%y")
        day = d.date()
    return day

def extracttime(match, prefix):
    t = match.lstrip(prefix)
    starttime = datetime.datetime.strptime(t, "%H:%M")
    return starttime.time()    

def parsestarttime(line):
    match = re.search(r"In:......", line)
    if match != None:
        return extracttime(match.group(0), 'In: ')
    return None

def parseendtime(line):
    match = re.search(r"Out:.*", line)
    if match != None:
        return extracttime(match.group(0), 'Out:')
    return None

def parse_workingperiod(line):
    workingperiod = workhours.WorkingHours()
    workingperiod.date = parsedate(line)
    workingperiod.start = parsestarttime(line)
    workingperiod.end = parseendtime(line)
    return workingperiod

def is_valid_period(period):
    return period.date != None and period.start != None

def parse(lines):
    workingperiods = [parse_workingperiod(line) for line in lines]
    workingperiods = [p for p in workingperiods if is_valid_period(p)]
    return workingperiods
