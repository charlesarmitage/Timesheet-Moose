using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moose;

namespace MooseUnitTests
{
    [TestFixture]
    class TextTimeLogReaderTests
    {
        [Test]
        public void TextTimeLogReaderIsATypeOfTimeLogReader()
        {
            TimeLogReader reader = new TextTimeLogReader(@"SimpleLog.log");
            Assert.That(reader is TimeLogReader);
        }

        [Test]
        public void ReaderReadsAllLinesFromTextFile()
        {
            TextTimeLogReader reader = new TextTimeLogReader(@"SimpleLog.log");
            var lines = reader.ReadAllLines();
            Assert.That(lines.Count(), Is.EqualTo(5));
            Assert.That(lines.ElementAt(0), Is.EqualTo("12/06/12 In: 09:00 Out: 17:00"));
            Assert.That(lines.ElementAt(1), Is.EqualTo("13/06/12 In: 08:00 Out: 16:00"));
            Assert.That(lines.ElementAt(2), Is.EqualTo("Invalid line"));
            Assert.That(lines.ElementAt(3), Is.EqualTo("14/06/12 In: 07:00 Out: "));
            Assert.That(lines.ElementAt(4), Is.EqualTo("   "));
        }

        [Test]
        public void ParserCanParseDateFromIndividualLogLine()
        {
            TextTimeLogParser parser = new TextTimeLogParser("12/06/12 In: 09:00 Out: 17:00");
            DateTime date = parser.StartTime;
            Assert.That(date.Date, Is.EqualTo(new DateTime(2012, 06, 12).Date));
        }

        [Test]
        public void ParserCanParseStartTimeFromIndividualLogLine()
        {
            TextTimeLogParser parser = new TextTimeLogParser("12/06/12 In: 09:00 Out: 17:00");
            DateTime time = parser.StartTime;
            Assert.That(time.TimeOfDay, Is.EqualTo(new DateTime(2012, 06, 12, 09, 00, 00).TimeOfDay));
        }

        [Test]
        public void ParserCanParseEndTimeFromIndividualLogLine()
        {
            TextTimeLogParser parser = new TextTimeLogParser("12/06/12 In: 09:00 Out: 17:00");
            DateTime time = parser.EndTime;
            Assert.That(time.TimeOfDay, Is.EqualTo(new DateTime(2012, 06, 12, 17, 00, 00).TimeOfDay));
        }

        [Test]
        public void ParserCanParseStartAndEndTimeToMinutesFromIndividualLogLine()
        {
            TextTimeLogParser parser = new TextTimeLogParser("12/06/12 In: 09:23 Out: 17:23");
            DateTime time = parser.EndTime;
            Assert.That(time.TimeOfDay, Is.EqualTo(new DateTime(2012, 06, 12, 17, 23, 00).TimeOfDay));
        }

        [Test]
        public void ParserThrowsAnExceptionIfTextIsInvalid()
        {
            Assert.Throws<InvalidTimeTextException>(()=> new TextTimeLogParser("Invalid text"));
            Assert.Throws<InvalidTimeTextException>(() => new TextTimeLogParser("Invalid text"));
            Assert.Throws<InvalidTimeTextException>(() => new TextTimeLogParser("27/08/12 Invalid text"));
        }

        [Test]
        public void ParserAnIncompleteDayHasAnInvalidEndTime()
        {
            TextTimeLogParser parser = null;
            Assert.DoesNotThrow(() => parser = new TextTimeLogParser("27/08/12 In: 09:00 Out:"));
            Assert.That(parser.EndTime.TimeOfDay, Is.EqualTo(new TimeSpan(0,0,0)));
        }

        [Test]
        public void ParserAnDayWithNoOutTimeHasAnInvalidEndTime()
        {
            TextTimeLogParser parser = null;
            Assert.DoesNotThrow(() => parser = new TextTimeLogParser("27/08/12 In: 09:00 "));
            Assert.That(parser.EndTime.TimeOfDay, Is.EqualTo(new TimeSpan(0, 0, 0)));
        }
    }
}
