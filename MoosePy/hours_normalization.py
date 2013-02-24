import unittest
import datetime
import time

def normalizehours(hours):
    hours = verify_hours(hours)
    nextq = nextquarter(hours.end)
    previousq = previousquarter(hours.start)

    if is_close_to_nextquarter(nextq, hours.end):
        hours.end = nextq
    else:
        hours.end = previousquarter(hours.end)

    if is_close_to_previousquarter(previousq, hours.start):
        hours.start = previousquarter(hours.start)
    else:
        hours.start = nextquarter(hours.start)

    return hours

def verify_hours(hours):
    if hours.start == None:
        hours.start = datetime.datetime.today().time()
    if hours.end == None:
        hours.end = datetime.datetime.today().time()
    return hours    

def previousquarter(workingtime):
    quarter = workingtime.minute / 15
    quarter = quarter * 15
    return workingtime.replace(minute=quarter)

def nextquarter(workingtime):
    quarter = (workingtime.minute / 15) + 1
    quarter = (quarter * 15) % 60
    rolloverhour = workingtime.hour
    if quarter == 0:
        rolloverhour = workingtime.hour + 1
    return workingtime.replace(hour=rolloverhour, minute=quarter)

def is_close_to_nextquarter(nextquarter, currenttime):
    nextq = datetime.datetime.combine(datetime.datetime.today(), nextquarter)
    currentt = datetime.datetime.combine(datetime.datetime.today(), currenttime)
    diff = nextq - currentt
    return diff <= datetime.timedelta(minutes=3)

def is_close_to_previousquarter(previousquarter, currenttime):
    return currenttime.minute - previousquarter.minute <= 3
