using System;
using System.Collections.Generic;
using System.Linq;

namespace MooseXLSReports
{
    public class WorkbookNavigator
    {
        private const int SevenDays = 7;
        private const int RowsBetweenWeeks = 16;
        private DateTime startOfMonthDate;
        private readonly IEnumerable<WorksheetStartingDates> startingDates;
        private DateTime requestedDate;

        public WorkbookNavigator(DateTime requestedDate, IEnumerable<WorksheetStartingDates> startingDates)
        {
            this.startingDates = startingDates;
            this.SetSheetFromDate(requestedDate);
        }

        public string SheetName { get; private set; }

        public int Row
        {
            get
            {
                return GetRowForStartOfWeek(this.requestedDate) + GetNoOfDaysFromMonday(this.requestedDate);
            }
        }

        public int GetNoOfDaysFromMonday(DateTime date)
        {
            var difference = date - this.startOfMonthDate;
            return difference.Days % SevenDays;
        }

        public int GetRowForStartOfWeek(DateTime date)
        {
            var difference = date - this.startOfMonthDate;
            var noOfWeeks = difference.Days / SevenDays;
            return (noOfWeeks * RowsBetweenWeeks) + SevenDays;
        }

        public void SetSheetFromDate(DateTime date)
        {
            this.requestedDate = date;
            var startingDate = this.startingDates.LastOrDefault(d => d.StartingDate <= date);
            this.startOfMonthDate = startingDate.StartingDate;
            this.SheetName = startingDate.SheetName;
        }
    }
}
