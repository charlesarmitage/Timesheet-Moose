using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace MooseXLSReports
{
    public class XlsReport : IDisposable, ITimesheetReport
    {
        readonly Workbook workbook;
        readonly Microsoft.Office.Interop.Excel.Application excel;

        public XlsReport(string filename)
        {
            var timesheet = new FileInfo(filename);
            excel = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
            var workbooks = excel.Workbooks;
            workbook = workbooks.Open(timesheet.FullName, UpdateLinks: false, ReadOnly: false);

            Marshal.ReleaseComObject(workbooks);
        }

        public void Dispose()
        {
            // In order to tidy up interop. COM objects force garbage collector to collect COM 
            // objects. Then release workbook and excel objects.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            workbook.Save();
            workbook.Close(SaveChanges: true);
            Marshal.FinalReleaseComObject(workbook);

            excel.Quit();
            Marshal.FinalReleaseComObject(excel);
        }

        public void WriteStartTime(DateTime startTime)
        {
            var cell = GetStartTimeCell(startTime);
            cell.Value = string.Format("{0:HH}:{0:mm}", startTime);
        }

        public void WriteEndTime(DateTime endTime)
        {
            var cell = GetEndTimeCell(endTime);
            cell.Value = string.Format("{0:HH}:{0:mm}", endTime);
        }

        public DateTime ReadStartTime(DateTime date)
        {
            var cell = GetStartTimeCell(date);
            var value = cell.Value;
            if (value != null)
            {
                return ConvertToDateTime(date, cell.Value);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public DateTime ReadEndTime(DateTime date)
        {
            var cell = GetEndTimeCell(date);
            var value = cell.Value;
            if (value != null)
            {
                return ConvertToDateTime(date, cell.Value);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private dynamic GetStartTimeCell(DateTime startTime)
        {
            const string column = "C";
            return GetTimesheetCell(startTime, column);
        }

        private dynamic GetEndTimeCell(DateTime endTime)
        {
            const string column = "D";
            return GetTimesheetCell(endTime, column);
        }

        private dynamic GetTimesheetCell(DateTime date, string column)
        {
            var rowPosition = new WorkbookNavigator(date);
            var cell = string.Format("{0}{1}", column, rowPosition.Row);
            var worksheets = excel.Worksheets;
            var sheet = worksheets[rowPosition.SheetName];
            var actualCell = sheet.Range[cell];

            return actualCell;
        }

        private static DateTime ConvertToDateTime(DateTime date, double excelTime)
        {
            double time = excelTime * 24.0 * 60.0;
            var timeOfDay = TimeSpan.FromMinutes(time);
            var readDate = new DateTime(date.Year, date.Month, date.Day) + timeOfDay;
            return readDate;
        }
    }
}