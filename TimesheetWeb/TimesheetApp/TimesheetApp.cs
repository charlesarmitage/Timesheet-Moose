using System;
using System.Windows.Forms;
using TimesheetWeb;

namespace TimesheetWebApp
{
    public partial class TimesheetApp : Form
    {
        public TimesheetApp()
        {
            InitializeComponent();

            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:41978"));
            var module = new TimesheetModule(); // Force loading of TimesheetModule dll so that Nancy can find it.
            nancyHost.Start();

            // TODO: Rename to TimeClerk for app name

            this.FormClosed += (sender, args) => nancyHost.Stop();
        }
    }
}
