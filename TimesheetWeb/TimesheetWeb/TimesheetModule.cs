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
        private readonly IRootPathProvider pathProvider;

        public TimesheetModule(IRootPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;

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
            paths.Add(@"C:\Users\carmitage\Dropbox\hg\Timesheet-Moose\MoosePy");
            paths.Add(@"C:\Python27\Lib");
            engine.SetSearchPaths(paths);

            var path = Path.Combine(pathProvider.GetRootPath(), @"..\..\MoosePy\estimatedhoursinweeks.py");
            var source = engine.CreateScriptSourceFromFile(path);

            var logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Timesheet.log";
            scope.SetVariable("logfile", logFilePath);

            source.Execute(scope);

            return scope.GetVariable("weeks");
        }
    }
}