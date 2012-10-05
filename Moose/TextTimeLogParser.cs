using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Moose
{
    public class TextTimeLogParser
    {
        public void Parse(string line)
        {
            string d = ParseDate(line);
            string i = ParseInTime(line);
            string o = ParseOutTime(line);
            StartTime = DateTime.Parse(string.Format("{0} {1}", d, i));
            EndTime = DateTime.Parse(string.Format("{0} {1}", d, o));
        }

        private static string ParseDate(string line)
        {
            Match match = Regex.Match(line, @"../../..");
            if (!match.Success)
                throw new InvalidTimeTextException();
            return match.Value;
        }

        private static string ParseInTime(string line)
        {
            Match m = Regex.Match(line, @"In:......");
            if (!m.Success)
                throw new InvalidTimeTextException();
            return m.Value.Split(new string[] { "In: " }, StringSplitOptions.None)[1];
        }

        private static string ParseOutTime(string line)
        {
            Match m = Regex.Match(line, @"Out:.*");
            if (!m.Success)
                return string.Empty;
            return m.Value.Split(new string[] { "Out:" }, StringSplitOptions.None)[1];
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
    }

    public class InvalidTimeTextException : Exception
    {
    }
}
