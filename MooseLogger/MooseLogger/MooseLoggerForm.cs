using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace MooseLog
{
    public partial class MooseLoggerForm : Form
    {
        [DllImport("kernel32.dll")]
        static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        static string ConfigIniFile = @".\MooseLogger.ini";
        private string LogFile = "";

        public MooseLoggerForm()
        {
            InitializeComponent();
            this.Load += OnFormLoad;
            this.FormClosing += OnFormClosing;
            this.Resize += OnLoggerResize;
            timesheetTrayIcon.MouseDoubleClick += OnLoggerMouseDoubleClick;
            systemTrayCheckbox.CheckedChanged += OnCheckedChanged;

            systemTrayCheckbox.Checked = GetPrivateProfileInt("Logger", "MinimizeToSysTray", 0, ConfigIniFile) > 0;
            LogFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MooseLog.log";
        }

        void OnCheckedChanged(object sender, EventArgs e)
        {
            string ticked = systemTrayCheckbox.Checked ? "1" : "0";
            WritePrivateProfileString("Logger", "MinimizeToSysTray", ticked, ConfigIniFile);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            string inTime = string.Format("\r\n{0:dd/MM/yy} ({0:dddd}) In: {0:HH:mm}", DateTime.Now);
            File.AppendAllText(LogFile, inTime);

            if (systemTrayCheckbox.Checked)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            string outTime = string.Format(" Out: {0:HH:mm}", DateTime.Now);
            File.AppendAllText(LogFile, outTime);
        }

        private void OnLoggerMouseDoubleClick(object sender, MouseEventArgs e)
        {        
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            timesheetTrayIcon.Visible = false;
        }

        private void OnLoggerResize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
                timesheetTrayIcon.Visible = true;
            }
        }

        private void OnTrayExitMenuItemClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
