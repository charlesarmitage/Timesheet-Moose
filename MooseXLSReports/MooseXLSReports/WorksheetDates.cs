using System;
using System.Collections.Generic;

namespace MooseXLSReports
{
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
}