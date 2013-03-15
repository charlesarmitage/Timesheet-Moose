using System;
using System.Globalization;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Nancy;

namespace TimesheetWeb
{
    public class ViewOutput
    {
        public string month;
        public dynamic weeks;
    }

    public class TimesheetModule : NancyModule
    {
        private readonly string timesheetPythonModulesPath;
        private const string Python27Libs = @"C:\Python27\Lib";

        public TimesheetModule(IRootPathProvider pathProvider)
        {
            timesheetPythonModulesPath = Path.Combine(pathProvider.GetRootPath(), @"..\..\MoosePy\");

            Get["/"] = parameters =>
                           {
                               var workingHours = GenerateWorkingHours();
                               var m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                               var output = new ViewOutput{weeks = workingHours, month = m};
                               return View["TimesheetIndex.cshtml", output];
                           };
        }

        private dynamic GenerateWorkingHours()
        {
            var engine = Python.CreateEngine();  
            var scope = engine.CreateScope();
            
            var paths = engine.GetSearchPaths();
            paths.Add(timesheetPythonModulesPath);
            paths.Add(Python27Libs);
            engine.SetSearchPaths(paths);

            var estimatedHoursFeed = Path.Combine(timesheetPythonModulesPath, @"estimatedhoursinweeks.py");
            var source = engine.CreateScriptSourceFromFile(estimatedHoursFeed);

            var logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Timesheet.log";
            scope.SetVariable("logfile", logFilePath);

            source.Execute(scope);

            return scope.GetVariable("weeks");
        }
    }
}