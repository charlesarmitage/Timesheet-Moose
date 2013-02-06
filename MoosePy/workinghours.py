import datetime

class WorkingHours():

    def __init__(self):
        self.date = datetime.date.min
        self.start = datetime.datetime.min
        self.end = datetime.datetime.min
