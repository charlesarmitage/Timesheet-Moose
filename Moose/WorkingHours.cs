﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moose
{
    public class WorkingHours
    {
        private List<DateTime> potentialStartTimes;
        private List<DateTime> potentialEndTimes;

        public WorkingHours(DateTime start, DateTime end)
        {
            potentialStartTimes = new List<DateTime>();
            potentialEndTimes = new List<DateTime>();
            this.StartTime = start;
            this.EndTime = end;
        }
        
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public IEnumerable<DateTime> PotentialStartTimes()
        {
            return potentialStartTimes;
        }

        public IEnumerable<DateTime> PotentialEndTimes()
        {
            return potentialEndTimes;
        }

        public void AddPotentialStartTime(DateTime dateTime)
        {
            potentialStartTimes.Add(dateTime);
        }

        public void AddPotentialEndTime(DateTime dateTime)
        {
            potentialEndTimes.Add(dateTime);
        }

        public override string ToString()
        {
            string output = string.Format("{0:dd/MM/yy}: {1}, {2}", StartTime.Date, StartTime.TimeOfDay, EndTime.TimeOfDay);
            return output;
        }
    }
}
