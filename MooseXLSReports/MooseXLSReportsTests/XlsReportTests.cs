using System;
using NUnit.Framework;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Interop = Microsoft.Office.Interop;
using MooseXLSReports;

namespace MooseXLSReportsTests
{
    [TestFixture]
    public class XlsReportTests
    {
        private const string TestTimesheet = "TestTimesheet.xlsx";

        [Test]
        public void ShouldReadDateWhenReadingFromCellInTestSpreadsheet()
        {
            Assert.That(File.Exists(TestTimesheet));
            var date = GetValueFromCell(TestTimesheet, "January", "C5");
            Assert.That(date, Is.EqualTo(new DateTime(2012, 12, 17)));
        }

        [Test]
        public void ShouldWriteTimeToCorrectCellForMondayStartTime()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var mondayStartTime = new DateTime(2012, 12, 17, 9, 30, 0);
                report.WriteStartTime(mondayStartTime);
            }

            var date = GetValueFromCell(TestTimesheet, "January", "C7");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(9, 30, 0)));
        }

        [Test]
        public void ShouldWriteTimeToCorrectCellForTuesdayStartTime()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var tuesdayStartTime = new DateTime(2012, 12, 18, 08, 15, 00);
                report.WriteStartTime(tuesdayStartTime);
            }

            var date = GetValueFromCell(TestTimesheet, "January", "C8");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(8, 15, 0)));
        }

        [Test]
        public void ShouldWriteTimeToCorrectCellForSundayStartTime()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var tuesdayStartTime = new DateTime(2012, 12, 23, 08, 15, 00);
                report.WriteStartTime(tuesdayStartTime);
            }

            var date = GetValueFromCell(TestTimesheet, "January", "C13");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(8, 15, 0)));
        }

        [Test]
        public void ShouldWriteTimeToCorrectCellForNextMondayStartTime()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var nextMondayStartTime = new DateTime(2012, 12, 24, 08, 15, 00);
                report.WriteStartTime(nextMondayStartTime);
            }

            var date = GetValueFromCell(TestTimesheet, "January", "C23");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(8, 15, 0)));
        }

        [Test]
        public void ShouldWriteTimeToCorrectCellForNextFridayStartTime()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var nextMondayStartTime = new DateTime(2012, 12, 28, 08, 15, 00);
                report.WriteStartTime(nextMondayStartTime);
            }

            var date = GetValueFromCell(TestTimesheet, "January", "C27");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(8, 15, 0)));
        }

        [Test]
        public void ShouldWritetimeToCorrectCellForTuesdayInCoupleOfWeeks()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var nextMondayStartTime = new DateTime(2013, 01, 01, 08, 15, 00);
                report.WriteStartTime(nextMondayStartTime);
            }

            var date = GetValueFromCell(TestTimesheet, "January", "C40");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(8, 15, 0)));
        }

        [Test]
        public void ShouldWriteTimeToCorrectCellForMondayNextMonth()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var nextMondayStartTime = new DateTime(2013, 01, 21, 08, 15, 00);
                report.WriteStartTime(nextMondayStartTime);
            }

            var date = GetValueFromCell(TestTimesheet, "February", "C7");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(8, 15, 0)));
        }

        [Test]
        public void ShouldWriteEndTimeToCorrectCellForThursday()
        {
            using (var report = new XlsReport(TestTimesheet))
            {
                var thursdayEndTime = new DateTime(2012, 12, 20, 08, 15, 00);
                report.WriteEndTime(thursdayEndTime);
            }

            var date = GetValueFromCell(TestTimesheet, "January", "D10");
            Assert.That(ConvertToDateTime(date), Is.EqualTo(new TimeSpan(8, 15, 0)));
        }

        [Test]
        public void ShouldReturnTimeFromStartTimeCellWhenPassedARequestedStartTime()
        {
            var startTime = new DateTime(2012, 12, 20, 08, 15, 00);
            using (var report = new XlsReport(TestTimesheet))
            {
                report.WriteStartTime(startTime);
                var readTime = report.ReadStartTime(new DateTime(2012, 12, 20));
                Assert.That(readTime, Is.EqualTo(startTime));
            }
        }

        [Test]
        public void ShouldReturnTimeFromEndTimeCellWhenPassedARequestedEndTime()
        {
            var endTime = new DateTime(2012, 12, 20, 08, 15, 00);
            using (var report = new XlsReport(TestTimesheet))
            {
                report.WriteEndTime(endTime);
                var readTime = report.ReadEndTime(new DateTime(2012, 12, 20));
                Assert.That(readTime, Is.EqualTo(endTime));
            }
        }

        private static dynamic GetValueFromCell(string filename, string sheet, string cell)
        {
            var timesheet = new FileInfo(filename);
            var excel = new Interop.Excel.Application() { Visible = false };
            var workbook = excel.Workbooks.Open(timesheet.FullName, UpdateLinks: false, ReadOnly: false);

            Worksheet worksheet = excel.Worksheets[sheet];
            var value = worksheet.Range[cell, cell];
            var date = value.Cells[1].Value;

            workbook.Close(SaveChanges: false);
            return date;
        }

        private static TimeSpan ConvertToDateTime(double excelTime)
        {
            double time = excelTime * 24.0 * 60.0;
            return TimeSpan.FromMinutes(time);
        }
    }
}
