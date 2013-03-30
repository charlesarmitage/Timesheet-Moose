using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nancy;

namespace TimesheetWebApp
{
    public partial class TimesheetApp : Form
    {
        public TimesheetApp()
        {
            InitializeComponent();

            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:41978"));

            nancyHost.Start();



            this.FormClosed += (sender, args) => nancyHost.Stop();
        }
    }

    public class TestModule : NancyModule
    {
        public TestModule(IRootPathProvider pathProvider)
        {
            Get["/"] = parameters => "Hello";
        }
    }
}
