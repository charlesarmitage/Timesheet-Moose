using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Moose
{
    public class TimesheetTextAppender
    {
        private TextWriter outputStream;

        public TimesheetTextAppender(TextWriter outputStream)
        {
            this.outputStream = outputStream;
        }

        public void Write(WorkingHours day)
        {
            var output = string.Format("{0:dd/MM/yy} ({0:dddd}) In: {0:HH:mm}, Out: {1:HH:mm}\r\n", day.StartTime, day.EndTime);
            outputStream.Write(output);
        }

        public void Close()
        {
            outputStream.Close();
        }
    }
}
