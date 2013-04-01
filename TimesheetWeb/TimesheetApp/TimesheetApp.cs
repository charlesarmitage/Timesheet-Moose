using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nancy;
using TimesheetWeb;

namespace TimesheetWebApp
{
    public partial class TimesheetApp : Form
    {
        public TimesheetApp()
        {
            InitializeComponent();

            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:41978"));
            TimesheetModule module = new TimesheetModule();
            nancyHost.Start();

            // TODO: Rename to TimeClerk for app name

            this.FormClosed += (sender, args) => nancyHost.Stop();
        }
    }
}
