import hours_estimator
import hours_aggregation
import hours_filtering
import hours_normalization

def calculate_estimated_hours(startday, raw_hours):
    hours = hours_filtering.filter_by__current_worksheet_month(startday, raw_hours)
    hours = hours_filtering.remove_weekends(hours)
    normalized_hours = hours_normalization.normalize_hours_list(hours)
    hours_grouped_by_day = hours_aggregation.group_hours_by_day(normalized_hours)
    return hours_grouped_by_day
