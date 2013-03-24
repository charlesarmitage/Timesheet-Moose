using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using IronPython.Compiler;
using IronPython.Hosting;
using IronPython.Runtime;
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
        private TimesheetConfig config;
        private ScriptEngine engine = Python.CreateEngine();

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
                    string fileName = GenerateSpreadheet(config.SpreadsheetGenerator.Filename, @"C:\git\Timesheet-Moose\MooseXLSReports\TestTimesheet.xlsx");
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
            config = timesheetConfig;
        }

        private dynamic GenerateWorkingHours(string hoursFeedPath, string timesheetLogPath)
        {
            var estimatedHoursFeed = Path.Combine(TimesheetPythonModulesPath, hoursFeedPath);
            var script = LoadScript(estimatedHoursFeed);
            return script.generate_estimated_hours(timesheetLogPath);
        }

        private dynamic GenerateSpreadheet(string generationScriptPath, string spreadsheetPath)
        {
            var timesheetLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Timesheet.log");
            var estimatedHoursFeed = Path.Combine(TimesheetPythonModulesPath, this.config.WeeksFeed.Filename);

            var script = LoadScript(estimatedHoursFeed);
            var workingHours = script.generate_estimated_hours(timesheetLog);

            var generationScript = Path.Combine(TimesheetPythonModulesPath, generationScriptPath);
            var spreadsheet = LoadScript(generationScript);
            return spreadsheet.generate_spreadsheet(spreadsheetPath, workingHours);
        }

        private dynamic LoadScript(string estimatedHoursFeed)
        {
            var runtime = Python.CreateRuntime();
            var eng = runtime.GetEngineByFileExtension(".py");

            var paths = eng.GetSearchPaths();
            paths.Add(TimesheetPythonModulesPath);
            paths.Add(Python27Libs);
            eng.SetSearchPaths(paths);

            dynamic script = runtime.UseFile(estimatedHoursFeed);
            return script;
        }
    }
}