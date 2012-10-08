using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moose;

namespace MooseUnitTests
{
    [TestFixture]
    public class WorkingHoursScriptsTests
    {
        private static DateTime CreateTimeOfDay(int hours, int minutes)
        {
            return DateTime.Now.Date + new TimeSpan(hours, minutes, 0);
        }

        [Test]
        public void CalculatorWithNoStrategy_Should_ReturnSameWorkingHours_When_WorkingHoursArePassedToIt()
        {
            WorkingDayCalculator calc = new WorkingDayCalculator();
            DateTime _9_00 = CreateTimeOfDay(9, 00);
            DateTime _17_00 = CreateTimeOfDay(17, 00);

            calc.AddStartTime(_9_00);
            calc.AddEndTime(_17_00);
            var day = calc.CalculateWorkingHours();

            Assert.That(day.StartTime, Is.EqualTo(_9_00));
            Assert.That(day.EndTime, Is.EqualTo(_17_00));
        }

        [Ignore]
        [Test]
        public void AddOneHoursStrategy_Should_ReturnWorkingHoursIncrementedByOneHour_When_WorkingHoursAreCalculated()
        {
            WorkingDayCalculator calc = new WorkingDayCalculator();
            string strategyScript = "";
            calc.SetStrategy(strategyScript);
            DateTime _9_00 = CreateTimeOfDay(9, 00);
            DateTime _17_00 = CreateTimeOfDay(17, 00);

            calc.AddStartTime(_9_00);
            calc.AddEndTime(_17_00);
            var day = calc.CalculateWorkingHours();

            Assert.That(day.StartTime, Is.EqualTo(CreateTimeOfDay(10, 00)));
            Assert.That(day.EndTime, Is.EqualTo(CreateTimeOfDay(18, 00)));
        }
    }
}
