using System;

namespace MooseXLSReports
{
    public interface ITimesheetReport
    {
        void WriteStartTime(DateTime startTime);
        void WriteEndTime(DateTime endTime);
        DateTime ReadStartTime(DateTime date);
        DateTime ReadEndTime(DateTime date);
    }
}