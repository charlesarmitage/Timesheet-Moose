import Moose

def is_office_start_time(potential_start_time):
    return potential_start_time.Hour >= 7 and potential_start_time.Hour <= 9 

def is_office_end_time(potential_end_time):
    return potential_end_time.Hour >= 15 and potential_end_time.Hour <= 18 

def get_estimated_start_time(normalized_hours):
    start_times = [hours.StartTime for hours in normalized_hours]
    filtered_times = filter(is_office_start_time, start_times)
    return filtered_times[0] if filtered_times else start_times[0]

def get_estimated_end_time(normalized_hours):
    end_times = [hours.EndTime for hours in normalized_hours]
    filtered_times = filter(is_office_end_time, end_times)
    return filtered_times[-1] if filtered_times else end_times[-1]

def estimate_hours(normalized_hours):
    start_time = get_estimated_start_time(normalized_hours)
    end_time = get_estimated_end_time(normalized_hours)
    estimated_hours = Moose.WorkingHours(start_time, end_time)

    for potential_hours in normalized_hours:
        estimated_hours.AddPotentialStartTime(potential_hours.StartTime)
        estimated_hours.AddPotentialEndTime(potential_hours.EndTime)

    return estimated_hours
