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
            DateTime startTime = unnormalizedStartTimes.FirstOrDefault();
            startTime = StartHoursNormalizer.NormalizeStartTime(startTime);
            DateTime endTime = unnormalizedEndTimes.FirstOrDefault();
            endTime = EndHoursNormalizer.NormalizeEndTime(endTime);

            var hours = new WorkingHours(startTime, endTime);
            AddPotentialWorkingHours(hours);

            // Execute scripts
            return hours;
        }

        private void AddPotentialWorkingHours(WorkingHours hours)
        {
            foreach (DateTime start in unnormalizedStartTimes)
            {
                hours.AddPotentialStartTime(StartHoursNormalizer.NormalizeStartTime(start));
            }

            foreach (DateTime end in unnormalizedEndTimes)
            {
                hours.AddPotentialEndTime(EndHoursNormalizer.NormalizeEndTime(end));
            }
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

        public void SetStrategy(string strategyScript)
        {
        }
    }
}
