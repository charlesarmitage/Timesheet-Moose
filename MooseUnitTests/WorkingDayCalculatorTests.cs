using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moose;

namespace MooseUnitTests
{
    [TestFixture]
    public class WorkingDayCalculatorTests
    {
        WorkingDayCalculator calc;
        DateTime _9_00;
        DateTime _17_00;

        [SetUp]
        public void Setup()
        {
            calc = new WorkingDayCalculator();
            _9_00 = CreateTimeOfDay(9, 00);
            _17_00 = CreateTimeOfDay(17, 00);
        }

        private static DateTime CreateTimeOfDay(int hours, int minutes)
        {
            DateTime time = DateTime.Now.Date + new TimeSpan(hours, minutes, 0);
            return time;
        }

        [Test]
        public void ANewMonthIsNotFilledIn()
        {
            MonthSheet august = new MonthSheet(); 
            Assert.That(august.IsFilledIn, Is.False);
        }

        [Test]
        public void ANewWeekIsNotFilledIn()
        {
            Week week = new Week(); 
            Assert.That(week.IsFilledIn, Is.False);
        }

        [Test]
        public void ANewWorkingDayHasAStartTime()
        {
            WorkingHours day = new WorkingHours(start: _9_00, end: _17_00);
            Assert.That(day.StartTime, Is.EqualTo(_9_00));
        }

        [Test]
        public void ANewWorkingDayHasAnEndTime()
        {
            WorkingHours day = new WorkingHours(start: _9_00, end: _17_00);
            Assert.That(day.EndTime, Is.EqualTo(_17_00));
        }

        [Test]
        public void ANewWorkingDayHasNoPotentialStartTimes()
        {
            WorkingHours day = new WorkingHours(start: _9_00, end: _17_00);
            Assert.That(day.PotentialStartTimes(), Is.Empty);
        }
        
        [Test]
        public void ANewWorkingDayHasNoPotentialEndTimes()
        {
            WorkingHours day = new WorkingHours(start: _9_00, end: _17_00);
            Assert.That(day.PotentialEndTimes(), Is.Empty);
        }

        [Test]
        public void AWorkingDayThatStartsAt9amHasAStartTimeOf9am()
        {
            calc.AddStartTime(_9_00);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_9_00));
        }

        [Test]
        public void AWorkingThatStartsAt_9_15_HasAStartTimeOf_9_15()
        {
            DateTime _9_15 = CreateTimeOfDay(9, 15);
            calc.AddStartTime(_9_15);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_9_15)); 
        }

        [Test]
        public void AWorkingThatStartsAt_9_30_HasAStartTimeOf_9_30()
        {
            DateTime _9_30 = CreateTimeOfDay(9, 30);
            calc.AddStartTime(_9_30);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_9_30));
        }

        [Test]
        public void AWorkingDayThatEndsAt5pmHasAnEndTimeof1700()
        {
            calc.AddEndTime(_17_00);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.EndTime, Is.EqualTo(_17_00));
        }

        [Test]
        public void AWorkingThatEndsAt_17_45_HasAnEndTimeOf_17_45()
        {
            DateTime _17_45 = CreateTimeOfDay(17, 45);
            calc.AddStartTime(_17_45);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_17_45));
        }

        [Test]
        public void AWorkingDayThatStartsAt_9_14_HasAStartTimeOf_9_15()
        {
            DateTime _9_14 = CreateTimeOfDay(9, 14);
            DateTime _9_15 = CreateTimeOfDay(9, 15);
            calc.AddStartTime(_9_14);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_9_15));
        }

        [Test]
        public void AWorkingDayThatStartsAt_9_59_HasAStarttimeOf_10_00()
        {
            DateTime _9_59 = CreateTimeOfDay(9, 59);
            DateTime _10_00 = CreateTimeOfDay(10, 00);
            calc.AddStartTime(_9_59);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_10_00));
        }

        [Test]
        public void AWorkingDayThatEndsAt_5_02_HasAnEndTimeOf_5_00()
        {
            DateTime _17_02 = CreateTimeOfDay(17, 02);
            calc.AddEndTime(_17_02);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.EndTime, Is.EqualTo(_17_00));
        }

        [Test]
        public void AWorkingDayThatEndsAt_5_47_HasAnEndTimeOf_5_45()
        {
            DateTime _17_47 = CreateTimeOfDay(17, 47);
            DateTime _17_45 = CreateTimeOfDay(17, 45);
            calc.AddEndTime(_17_47);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.EndTime, Is.EqualTo(_17_45));
        }

        [Test]
        public void AWorkingDayThatEndsAt_5_58_HasAnendTimeOf_18_00()
        {
            DateTime _17_58 = CreateTimeOfDay(17, 58);
            DateTime _18_00 = CreateTimeOfDay(18, 00);
            calc.AddEndTime(_17_58);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.EndTime, Is.EqualTo(_18_00));
        }

        [Test]
        public void AWorkingDayThatStartsAt_9_03_HasAStartTimeOf_0900()
        {
            DateTime _9_03 = CreateTimeOfDay(9, 03);
            calc.AddStartTime(_9_03);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_9_00));
        }

        [Test]
        public void AWorkingDayThatEndsAt_17_12_HasAnEndTimeOf_1715()
        {
            DateTime _17_12 = CreateTimeOfDay(17, 12);
            DateTime _17_15 = CreateTimeOfDay(17, 15);
            calc.AddEndTime(_17_12);
            var day = calc.CalculateWorkingHours();
            Assert.That(day.EndTime, Is.EqualTo(_17_15));
        }

        [Test]
        public void WorkingCalculator_Should_AddExtraStartAndEndTimesToWorkingDay_When_ExtraStartAndEndTimesAreAdded()
        {
            DateTime _9_00 = CreateTimeOfDay(9, 00);
            DateTime _17_00 = CreateTimeOfDay(17, 00);
            calc.AddStartTime(_9_00);
            calc.AddEndTime(_17_00);

            var _20_00 = CreateTimeOfDay(20, 00);
            var _21_00 = CreateTimeOfDay(21, 00);
            calc.AddStartTime(_20_00);
            calc.AddEndTime(_21_00);

            var day = calc.CalculateWorkingHours();
            Assert.That(day.StartTime, Is.EqualTo(_9_00));
            Assert.That(day.EndTime, Is.EqualTo(_17_00));
            Assert.That(day.PotentialStartTimes().Count() > 0);
            Assert.That(day.PotentialEndTimes().Count() > 0);
        }
    }
}
