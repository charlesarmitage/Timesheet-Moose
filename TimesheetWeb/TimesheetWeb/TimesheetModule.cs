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
            var scope = BuildScriptScope();
            var estimatedHoursFeed = Path.Combine(TimesheetPythonModulesPath, hoursFeedPath);
            var source = engine.CreateScriptSourceFromFile(estimatedHoursFeed);

            scope.SetVariable("logfile", timesheetLogPath);
            ExecuteScript(scope, source);
            return scope.GetVariable("weeks");
        }

        private dynamic GenerateSpreadheet(string generationScriptPath, string spreadsheetPath)
        {
            var scope = BuildScriptScope();
            var generationScript = Path.Combine(TimesheetPythonModulesPath, generationScriptPath);
            var source = engine.CreateScriptSourceFromFile(generationScript);

            scope.SetVariable("xls_file", spreadsheetPath);
            var timesheetLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                            "Timesheet.log");
            var workingHours = GenerateWorkingHours(weekFeedScript, timesheetLog);
            scope.SetVariable("weeks", workingHours);
            ExecuteScript(scope, source);
            return scope.GetVariable("output_file");
        }

        private ScriptScope BuildScriptScope()
        {
            var scope = engine.CreateScope();

            var paths = engine.GetSearchPaths();
            paths.Add(TimesheetPythonModulesPath);
            paths.Add(Python27Libs);
            engine.SetSearchPaths(paths);
            
            return scope;
        }

        private void ExecuteScript(ScriptScope scope, ScriptSource source)
        {
            var pco = (PythonCompilerOptions) engine.GetCompilerOptions(scope);

            pco.ModuleName = "__main__";
            pco.Module |= ModuleOptions.Initialize;

            CompiledCode compiled = source.Compile(pco);
            compiled.Execute(scope);
        }
    }
}