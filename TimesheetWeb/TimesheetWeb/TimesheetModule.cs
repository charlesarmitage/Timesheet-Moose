using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using IronPython.Hosting;
using Nancy;
using System.Web;

namespace TimesheetWeb
{
    public class ViewOutput
    {
        public string month;
        public dynamic hours;
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
                               var output = new ViewOutput{hours = workingHours, month = m};
                               return View["TimesheetIndex.cshtml", output];
                           };
        }

        private dynamic GenerateWorkingHours()
        {
            var ipy = Python.CreateRuntime();
            string path = Path.Combine(pathProvider.GetRootPath(), @"..\..\MoosePy\estimatedhoursfeed.py");
            dynamic estimatedHours = ipy.UseFile(path);
            return estimatedHours.hours;
        }
    }
}