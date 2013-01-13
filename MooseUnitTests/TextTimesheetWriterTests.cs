using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moose;
using System.IO;

namespace MooseUnitTests
{
    [TestFixture]
    class TextTimesheetWriterTests
    {
        WorkingDayCalculator monday;
        WorkingDayCalculator tuesday;
        TimesheetTextAppender writer;
        StreamWriter outfile;

        [SetUp]
        public void Setup()
        {
            File.WriteAllText("WriterTestOutput.txt", "");
            outfile = new StreamWriter("WriterTestOutput.txt");
            writer = new TimesheetTextAppender(outfile);
            monday = GetMonday();
            tuesday = new WorkingDayCalculator();
            tuesday.AddStartTime(new DateTime(2012, 08, 07, 09, 00, 00));
            tuesday.AddEndTime(new DateTime(2012, 08, 07, 17, 00, 00));
        }

        [TearDown]
        public void Teardown()
        {
            outfile.Dispose();
        }

        [Test]
        public void Should_WriteToATextFile_When_ADayIsWritten()
        {
            var hours = monday.CalculateWorkingHours();
            writer.Write(hours);
            writer.Close();

            var lines = GetText();
            Assert.That(lines.Count(), Is.EqualTo(1));
            Assert.That(lines[0], Is.EqualTo("06/08/12 (Monday) In: 09:00, Out: 17:00"));
        }

        [Test]
        public void Should_AppendToATextFile_When_MultipleDaysAreWritten()
        {
            var hours = monday.CalculateWorkingHours();
            var tuesdayHours = tuesday.CalculateWorkingHours();
            writer.Write(hours);
            writer.Write(tuesdayHours);
            writer.Close();

            var lines = GetText();
            Assert.That(lines.Count(), Is.EqualTo(2));
            Assert.That(lines[0], Is.EqualTo("06/08/12 (Monday) In: 09:00, Out: 17:00"));
            Assert.That(lines[1], Is.EqualTo("07/08/12 (Tuesday) In: 09:00, Out: 17:00"));
        }

        private string[] GetText()
        {
            return File.ReadAllLines("WriterTestOutput.txt");
        }

        private static WorkingDayCalculator GetMonday()
        {
            var firstDay = new WorkingDayCalculator();
            firstDay.AddStartTime(new DateTime(2012, 08, 06, 09, 00, 00));
            firstDay.AddEndTime(new DateTime(2012, 08, 06, 17, 00, 00));
            return firstDay;
        }
    }
}
