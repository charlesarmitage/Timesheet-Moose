from workinghours import workhours

def is_office_start_time(potential_start_time):
    return potential_start_time.hour >= 7 and potential_start_time.hour <= 9 

def is_office_end_time(potential_end_time):
    return potential_end_time.hour >= 15 and potential_end_time.hour <= 18 

def get_estimated_start_time(normalized_hours):
    start_times = [hours.start for hours in normalized_hours]
    filtered_times = filter(is_office_start_time, start_times)
    return filtered_times[0] if filtered_times else start_times[0]

def get_estimated_end_time(normalized_hours):
    end_times = [hours.end for hours in normalized_hours]
    filtered_times = filter(is_office_end_time, end_times)
    return filtered_times[-1] if filtered_times else end_times[-1]

def estimate_hours(normalized_hours):
    start_time = get_estimated_start_time(normalized_hours)
    end_time = get_estimated_end_time(normalized_hours)
 
    estimated_hours = workhours.WorkingHours()
    estimated_hours.date = normalized_hours[0].date
    estimated_hours.start = start_time
    estimated_hours.end = end_time
    estimated_hours.potential_start = []
    estimated_hours.potential_end = []

    for potential_hours in normalized_hours:
        estimated_hours.potential_start.append(potential_hours.start)
        estimated_hours.potential_end.append(potential_hours.end)

    return estimated_hours

def get_estimated_hours(hours):
    return [estimate_hours(working_hours) for working_hours in hours]
