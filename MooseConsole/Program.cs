using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moose;
using System.IO;

namespace MooseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string logFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Timesheet.log";
            //string outputFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\TimesheetOutput.txt";
            //File.WriteAllLines(outputFile, new string[0]);

            TextTimeLogReader reader = new TextTimeLogReader(logFile);

            var lines = reader.ReadAllLines();
            foreach(var line in lines)
            {
                TextTimeLogParser parser = new TextTimeLogParser();
                parser.Parse(line);
                WorkingDayCalculator calc = new WorkingDayCalculator();
                calc.AddStartTime(parser.StartTime);
                calc.AddEndTime(parser.EndTime);
                var hours = calc.CalculateWorkingHours();

                TimesheetTextAppender writer = new TimesheetTextAppender(Console.Out);
                writer.Write(hours);
            }
        }
    }
}
