using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moose
{
    public class StartHoursNormalizer
    {
        public static DateTime NormalizeStartTime(DateTime startTime)
        {
            QuarterHours quarters = new QuarterHours(startTime);
            if (IsWithinPreviousQuarterHourTolerance(startTime, quarters))
            {
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, quarters.PreviousQuarterInMinutes(), 0);
            }
            else
            {
                return new DateTime(startTime.Year, startTime.Month, startTime.Day) + quarters.NextQuarter();
            }
        }

        private static bool IsWithinPreviousQuarterHourTolerance(DateTime time, QuarterHours quarters)
        {
            return time.TimeOfDay.Minutes - quarters.PreviousQuarterInMinutes() <= 3;
        }
    }

    public class EndHoursNormalizer
    {
        public static DateTime NormalizeEndTime(DateTime endTime)
        {
            QuarterHours quarters = new QuarterHours(endTime);
            if (IsWithinNextQuarterHourTolerance(endTime, quarters))
            {
                 return new DateTime(endTime.Year, endTime.Month, endTime.Day) + quarters.NextQuarter();
            }
            else
            {
                return new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, quarters.PreviousQuarterInMinutes(), 0);
            }
        }

        private static bool IsWithinNextQuarterHourTolerance(DateTime time, QuarterHours quarters)
        {
            return quarters.NextQuarterInMinutes() - time.TimeOfDay.Minutes <= 3;
        }
    }
}
