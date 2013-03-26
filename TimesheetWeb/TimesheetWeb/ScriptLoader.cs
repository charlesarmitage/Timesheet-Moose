using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IronPython.Hosting;

namespace TimesheetWeb
{
    public class ScriptLoader
    {
        public ScriptLoader()
        {
            ExtraModulePath = string.Empty;
            PythonLibraries = string.Empty;
        }

        public string ExtraModulePath { get; set; }
        public string PythonLibraries { get; set; }

        public dynamic LoadScript(string ironPythonScript)
        {
            var runtime = Python.CreateRuntime();
            var eng = runtime.GetEngineByFileExtension(".py");

            var paths = eng.GetSearchPaths();
            paths.Add(ExtraModulePath);
            paths.Add(PythonLibraries);
            eng.SetSearchPaths(paths);

            dynamic script = runtime.UseFile(ironPythonScript);
            return script;
        }
    }
}