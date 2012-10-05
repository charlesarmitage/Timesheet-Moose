using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moose
{
    public class QuarterHours
    {
        private DateTime time;
        public QuarterHours(DateTime t)
        {
            this.time = t;
        }

        public TimeSpan NextQuarter()
        {
            int minutes = NextQuarterInMinutes();
            int hours = time.Hour;
            if (minutes == 60)
            {
                minutes = 0;
                hours += 1;
            }
            return new TimeSpan(hours, minutes, seconds: 0);
        }

        public int NextQuarterInMinutes()
        {
            int quarterOfHour = (time.TimeOfDay.Minutes / 15) + 1;
            return quarterOfHour * 15;
        }

        public int PreviousQuarterInMinutes()
        {
            int quarterOfHour = time.TimeOfDay.Minutes / 15;
            return quarterOfHour * 15;
        }

    }
}
