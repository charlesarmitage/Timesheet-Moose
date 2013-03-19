using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Nancy;
using Nancy.Responses;

namespace TimesheetWeb
{
    public class ViewOutput
    {
        public string logfileurl;
        public string month;
        public dynamic weeks;
    }

    public class TimesheetModule : NancyModule
    {
        private string TimesheetPythonModulesPath;
        private string Python27Libs;
        private string weekFeedScript;

        public TimesheetModule(IRootPathProvider pathProvider)
        {
            ConfigureTimesheetModules(pathProvider);

            Get["/"] = parameters =>
                {
                    var timesheetLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                    "Timesheet.log");
                    var workingHours = GenerateWorkingHours(weekFeedScript, timesheetLog);

                    var m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    var output = new ViewOutput { weeks = workingHours, month = m, logfileurl = timesheetLog };
                    return View["TimesheetIndex.cshtml", output];
                };

            Post["/logurl"] = parameters =>
                {
                    var timesheetLog = (string)Request.Form.logfileurl.Value;
                    var workingHours = GenerateWorkingHours(weekFeedScript, timesheetLog);

                    var m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    var output = new ViewOutput { weeks = workingHours, month = m, logfileurl = timesheetLog };
                    return View["TimesheetIndex.cshtml", output];
                };

            Get["/download_timesheet"] = parameters =>
                {
                    const string fileName = "Placeholder.txt";
                    var mimeType = MimeTypes.GetMimeType(fileName);
                    var path = Path.Combine(pathProvider.GetRootPath(), fileName);
                    Func<Stream> file = () => new FileStream(path, FileMode.Open);

                    var response = new StreamResponse(file, mimeType);
                    return response.AsAttachment(fileName);
               };
        }

        private void ConfigureTimesheetModules(IRootPathProvider pathProvider)
        {
            var configPath = Path.Combine(pathProvider.GetRootPath(), "timesheet.config");
            var timesheetConfig = new TimesheetConfig();
            using (var timesheetStream = new StreamReader(configPath))
            {
                var configXml = new XmlSerializer(typeof (TimesheetConfig));
                timesheetConfig = (TimesheetConfig) configXml.Deserialize(timesheetStream);
                timesheetStream.Close();
            }

            Python27Libs = timesheetConfig.Python.Path;
            TimesheetPythonModulesPath = Path.Combine(pathProvider.GetRootPath(), timesheetConfig.Module.Relativepath);
            weekFeedScript = timesheetConfig.WeeksFeed.Filename;
        }

        private dynamic GenerateWorkingHours(string hoursFeedPath, string timesheetLogPath)
        {
            var engine = Python.CreateEngine();  
            var scope = engine.CreateScope();
            
            var paths = engine.GetSearchPaths();
            paths.Add(TimesheetPythonModulesPath);
            paths.Add(Python27Libs);
            engine.SetSearchPaths(paths);

            var estimatedHoursFeed = Path.Combine(TimesheetPythonModulesPath, hoursFeedPath);
            var source = engine.CreateScriptSourceFromFile(estimatedHoursFeed);

            scope.SetVariable("logfile", timesheetLogPath);
            source.Execute(scope);
            return scope.GetVariable("weeks");
        }
    }
}