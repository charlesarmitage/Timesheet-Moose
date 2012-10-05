using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moose
{
    public class WorkingDayCalculator
    {
        private List<DateTime> unnormalizedStartTimes;
        private List<DateTime> unnormalizedEndTimes;

        public WorkingDayCalculator()
        {
            unnormalizedStartTimes = new List<DateTime>();
            unnormalizedEndTimes = new List<DateTime>();
        }


        public void AddStartTime(DateTime startTime)
        {
            unnormalizedStartTimes.Add(startTime);
        }

        public void AddEndTime(DateTime endTime)
        {
            unnormalizedEndTimes.Add(endTime);
        }

        public WorkingHours CalculateWorkingHours()
        {
            // Use null coalising operator
            var startTime = new DateTime();
            if (unnormalizedStartTimes.Count > 0)
            {
                startTime = StartHoursNormalizer.NormalizeStartTime(unnormalizedStartTimes[0]);
            }

            var endTime = new DateTime();
            if (unnormalizedEndTimes.Count > 0)
            {
                endTime = EndHoursNormalizer.NormalizeEndTime(unnormalizedEndTimes[0]);
            }

            var hours = new WorkingHours(startTime, endTime);

            foreach(DateTime start in unnormalizedStartTimes)
            {
                hours.AddPotentialStartTime(StartHoursNormalizer.NormalizeStartTime(start));
            }

            foreach (DateTime end in unnormalizedEndTimes)
            {
                hours.AddPotentialEndTime(EndHoursNormalizer.NormalizeEndTime(endTime));
            }

            // Execute scripts
            return hours;
        }

        /*private void LoadAndRunScript()
        {
            // Load script from file as the text from the file
            script = DefaultImports + script;

            try
            {
                ScriptEngine engine = Python.CreateEngine();
                ScriptSource source = engine.CreateScriptSourceFromString(script);
                return source.Compile();
            }
            catch (SyntaxErrorException ex)
            {
             //   Logger.Error("Error loading script.", ex);
                return;// null;
            }

            if (this.code != null)
            {
                this.code.DefaultScope.RemoveVariable("fields");
                this.code.DefaultScope.SetVariable("record", record);
                return this.RunScript() as CDR;
            }
            else
            {
                return null;
            }

            if (this.code != null)
            {
                try
                {
                    return this.code.Execute();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error executing script.", ex);
                }
            }
        }*/
    }
}
