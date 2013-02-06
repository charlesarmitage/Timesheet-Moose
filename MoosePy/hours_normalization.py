import unittest
import datetime
import time

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
    return nextquarter.minute - currenttime.minute <= 3

def is_close_to_previousquarter(previousquarter, currenttime):
    return currenttime.minute - previousquarter.minute <= 3

def normalizehours(hours):
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
