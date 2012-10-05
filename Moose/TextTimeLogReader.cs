using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Moose
{
    public class TextTimeLogReader : TimeLogReader
    {
        private string logPath;

        public TextTimeLogReader(string logPath)
        {
            this.logPath = logPath;
        }

        public IEnumerable<string> ReadAllLines()
        {
            return File.ReadAllLines(logPath);
        }
    }
}
