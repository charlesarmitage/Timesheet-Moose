using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moose
{
    public class MonthSheet
    {
        public MonthSheet()
        {
            IsFilledIn = false;
        }

        public bool IsFilledIn { get; private set; }

        public Week GetWeek(string p)
        {
            return new Week();
        }
    }
}
