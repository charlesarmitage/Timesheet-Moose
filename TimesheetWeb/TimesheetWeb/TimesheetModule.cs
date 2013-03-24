using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private TimesheetConfig config;

        public TimesheetModule(IRootPathProvider pathProvider)
        {
            ConfigureTimesheetModules(pathProvider);

            Get["/"] = parameters =>
                {
                    var timesheetLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                    "Timesheet.log");
                    var workingHours = GenerateWorkingHours(config.WeeksFeed.Filename, timesheetLog);

                    var m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    var output = new ViewOutput { weeks = workingHours, month = m, logfileurl = timesheetLog };
                    return View["TimesheetIndex.cshtml", output];
                };

            Post["/logurl"] = parameters =>
                {
                    var timesheetLog = (string)Request.Form.logfileurl.Value;
                    var workingHours = GenerateWorkingHours(config.WeeksFeed.Filename, timesheetLog);

                    var m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    var output = new ViewOutput { weeks = workingHours, month = m, logfileurl = timesheetLog };
                    return View["TimesheetIndex.cshtml", output];
                };

            Post["/update_spreadsheet"] = parameters =>
            {
                var uploadedFiles = UploadFile(pathProvider);

                var spreadsheet = uploadedFiles.FirstOrDefault() ?? string.Empty;
                string fileName = GenerateSpreadheet(config.SpreadsheetGenerator.Filename, spreadsheet);
                if(fileName != string.Empty)
                    return BuildFileDownloadResponse(pathProvider, fileName);
                else
                    return "Invalid file";
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

            TimesheetPythonModulesPath = Path.Combine(pathProvider.GetRootPath(), timesheetConfig.Module.Relativepath);
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

        private IEnumerable<string> UploadFile(IRootPathProvider pathProvider)
        {
            var uploadDirectory = Path.Combine(pathProvider.GetRootPath(), "Content", "uploads");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var uploadedFiles = new List<string>();
            foreach (var file in Request.Files)
            {
                var filename = Path.Combine(uploadDirectory, file.Name);
                uploadedFiles.Add(filename);
                using (var fileStream = new FileStream(filename, FileMode.Create))
                {
                    file.Value.CopyTo(fileStream);
                }
            }

            return uploadedFiles;
        }

        private static dynamic BuildFileDownloadResponse(IRootPathProvider pathProvider, string fileName)
        {
            var mimeType = MimeTypes.GetMimeType(fileName);
            var path = Path.Combine(pathProvider.GetRootPath(), fileName);
            Func<Stream> file = () => new FileStream(path, FileMode.Open);

            var response = new StreamResponse(file, mimeType);
            return response.AsAttachment(fileName);
        }

        private dynamic LoadScript(string ironPythonScript)
        {
            var runtime = Python.CreateRuntime();
            var eng = runtime.GetEngineByFileExtension(".py");

            var paths = eng.GetSearchPaths();
            paths.Add(TimesheetPythonModulesPath);
            paths.Add(config.Python.Path);
            eng.SetSearchPaths(paths);

            dynamic script = runtime.UseFile(ironPythonScript);
            return script;
        }
    }
}