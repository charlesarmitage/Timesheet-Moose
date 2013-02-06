import datetime

class WorkingHours():

    def __init__(self):
        self.date = datetime.date.min
        self.start = datetime.time.min
        self.end = datetime.time.min

    def __str__(self):
    	return '%s %s, %s' % (self.date, self.start, self.end)


