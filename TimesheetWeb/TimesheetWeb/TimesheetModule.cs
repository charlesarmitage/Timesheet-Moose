using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using IronPython.Hosting;
using Nancy;
using Nancy.Responses;

namespace TimesheetWeb
{
    public class ViewOutput
    {
        public string LogFileUrl;
        public string Month;
        public dynamic Weeks;
    }

    public class TimesheetModule : NancyModule
    {
        private string estimatedHoursScript;
        private string spreadsheetGeneratorScript;
        private string timesheetLog;
        private string timesheetPythonModulesPath;
        private TimesheetConfig config;
        private string timesheetWriterType;

        public TimesheetModule(IRootPathProvider pathProvider)
        {
            ConfigureTimesheetModules(pathProvider);

            Get["/"] = parameters =>
                {
	                var workingHours = GenerateWorkingHours(timesheetLog);


                    var m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    var output = new ViewOutput { Weeks = workingHours, Month = m, LogFileUrl = timesheetLog };
                    return View["TimesheetIndex.cshtml", output];
                };

            Post["/logurl"] = parameters =>
                {
                    var workingHours = GenerateWorkingHours(timesheetLog);


                    var m = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    var output = new ViewOutput { Weeks = workingHours, Month = m, LogFileUrl = timesheetLog };
                    return View["TimesheetIndex.cshtml", output];
                };

            Post["/update_spreadsheet"] = parameters =>
            {
                var uploadedFiles = UploadFile(pathProvider);

                var spreadsheet = uploadedFiles.FirstOrDefault() ?? string.Empty;
                string fileName = GenerateSpreadheet(spreadsheet);

                if(fileName != string.Empty)
                    return BuildFileDownloadResponse(pathProvider, fileName);
                else
                    return "Invalid file";
            };
        }

        private void ConfigureTimesheetModules(IRootPathProvider pathProvider)
        {
            var configPath = Path.Combine(pathProvider.GetRootPath(), "timesheet.config");
            config = new TimesheetConfig();
            using (var timesheetStream = new StreamReader(configPath))
            {
                var configXml = new XmlSerializer(typeof (TimesheetConfig));
                config = (TimesheetConfig) configXml.Deserialize(timesheetStream);
                timesheetStream.Close();
            }

            timesheetLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Timesheet.log");

            timesheetPythonModulesPath = Path.Combine(pathProvider.GetRootPath(), config.Module.Relativepath);
            estimatedHoursScript = Path.Combine(timesheetPythonModulesPath, config.WeeksFeed.Filename);
            spreadsheetGeneratorScript = Path.Combine(timesheetPythonModulesPath, config.SpreadsheetGenerator.Filename);
            timesheetWriterType = config.WriterType.Type ?? string.Empty;
        }

        private dynamic GenerateWorkingHours(string timesheetLogPath)
        {
            var script = LoadScript(estimatedHoursScript);
            return script.generate_estimated_hours(timesheetLogPath);
        }

        private dynamic GenerateSpreadheet(string spreadsheetPath)
        {
            var workingHoursScript = LoadScript(estimatedHoursScript);
            var workingHours = workingHoursScript.generate_estimated_hours(timesheetLog);

            var spreadsheetScript = LoadScript(spreadsheetGeneratorScript);
            if (timesheetWriterType == "xls")
            {
                spreadsheetScript.writer = spreadsheetScript.build_xls_writer(spreadsheetPath);
            }
            else
            {
                spreadsheetScript.writer = spreadsheetScript.build_text_writer(spreadsheetPath);
            }

            return spreadsheetScript.generate_spreadsheet(spreadsheetPath, workingHours);
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
            var fileInfo = new FileInfo(path);
            return response.AsAttachment(fileInfo.Name);
        }

        private dynamic LoadScript(string ironPythonScript)
        {
            var runtime = Python.CreateRuntime();
            var eng = runtime.GetEngineByFileExtension(".py");

            var paths = eng.GetSearchPaths();
            paths.Add(timesheetPythonModulesPath);
            paths.Add(config.Python.Path);
            eng.SetSearchPaths(paths);

            dynamic script = runtime.UseFile(ironPythonScript);
            return script;
        }
    }
}