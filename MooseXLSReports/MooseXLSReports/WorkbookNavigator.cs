using System;
using System.Collections.Generic;
using System.Linq;

namespace MooseXLSReports
{   
    public struct WorksheetStartingDates
    {
        public string SheetName;
        public DateTime StartingDate;
    }

    public class WorksheetDates
    {
        public static List<WorksheetStartingDates> GetStartDatesForWorksheets()
        {
            return new List<WorksheetStartingDates>
                        {
                            new WorksheetStartingDates {SheetName = "January", StartingDate = DateTime.Parse("17/12/2012")},
                            new WorksheetStartingDates {SheetName = "February", StartingDate = DateTime.Parse("21/01/2013")},
                            new WorksheetStartingDates {SheetName = "March", StartingDate = DateTime.Parse("25/02/2013")},
                            new WorksheetStartingDates {SheetName = "April", StartingDate = DateTime.Parse("25/03/2013")},
                            new WorksheetStartingDates {SheetName = "May", StartingDate = DateTime.Parse("22/04/2013")},
                            new WorksheetStartingDates {SheetName = "June", StartingDate = DateTime.Parse("20/05/2013")},
                            new WorksheetStartingDates {SheetName = "July", StartingDate = DateTime.Parse("17/06/2013")},
                            new WorksheetStartingDates {SheetName = "August", StartingDate = DateTime.Parse("15/07/2013")},
                            new WorksheetStartingDates {SheetName = "September", StartingDate = DateTime.Parse("12/08/2013")},
                            new WorksheetStartingDates {SheetName = "October", StartingDate = DateTime.Parse("16/09/2013")},
                            new WorksheetStartingDates {SheetName = "November", StartingDate = DateTime.Parse("14/10/2013")},
                            new WorksheetStartingDates {SheetName = "December", StartingDate = DateTime.Parse("18/11/2013")}
                        };
        }        
    }

    public class WorkbookNavigator
    {
        private const int SevenDays = 7;
        private const int RowsBetweenWeeks = 16;
        private DateTime startOfMonthDate;
        private readonly List<WorksheetStartingDates> startingDates;
        private DateTime requestedDate;

        public WorkbookNavigator(DateTime requestedDate)
        {
            startingDates = WorksheetDates.GetStartDatesForWorksheets();
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
