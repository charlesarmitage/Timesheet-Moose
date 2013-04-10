using System;
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

            const string appAddress = "http://localhost:41978";
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri(appAddress));

            nancyHost.Start();

            // TODO: Rename to TimeClerk for app name

            MethodInvoker textUpdate = () => this.addressTextbox.Text = @"Timesheet Standalone. Navigate to: " + appAddress;
            this.Load += (sender, args) => this.addressTextbox.Invoke(textUpdate); ;
            this.FormClosed += (sender, args) => nancyHost.Stop();
        }

        public class TestModule : NancyModule 
        {
            public TestModule()
            {
                Get["/Test"] = p => "It works";
            }
        }
    }
}
