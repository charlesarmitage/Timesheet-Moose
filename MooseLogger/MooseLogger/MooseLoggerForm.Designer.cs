namespace MooseLog
{
    partial class MooseLoggerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MooseLoggerForm));
            this.timesheetTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemTrayCheckbox = new System.Windows.Forms.CheckBox();
            this.trayContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // timesheetTrayIcon
            // 
            this.timesheetTrayIcon.ContextMenuStrip = this.trayContextMenuStrip;
            this.timesheetTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("timesheetTrayIcon.Icon")));
            this.timesheetTrayIcon.Text = "MooseLog Logger";
            this.timesheetTrayIcon.Visible = true;
            // 
            // trayContextMenuStrip
            // 
            this.trayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayExitMenuItem});
            this.trayContextMenuStrip.Name = "trayContextMenuStrip";
            this.trayContextMenuStrip.Size = new System.Drawing.Size(93, 26);
            // 
            // trayExitMenuItem
            // 
            this.trayExitMenuItem.Name = "trayExitMenuItem";
            this.trayExitMenuItem.Size = new System.Drawing.Size(92, 22);
            this.trayExitMenuItem.Text = "Exit";
            this.trayExitMenuItem.Click += new System.EventHandler(this.OnTrayExitMenuItemClick);
            // 
            // systemTrayCheckbox
            // 
            this.systemTrayCheckbox.AutoSize = true;
            this.systemTrayCheckbox.Checked = true;
            this.systemTrayCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.systemTrayCheckbox.Location = new System.Drawing.Point(13, 13);
            this.systemTrayCheckbox.Name = "systemTrayCheckbox";
            this.systemTrayCheckbox.Size = new System.Drawing.Size(114, 17);
            this.systemTrayCheckbox.TabIndex = 0;
            this.systemTrayCheckbox.Text = "Start in system tray";
            this.systemTrayCheckbox.UseVisualStyleBackColor = true;
            // 
            // TimesheetLoggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 44);
            this.Controls.Add(this.systemTrayCheckbox);
            this.Name = "TimesheetLoggerForm";
            this.Text = "MooseLog Logger";
            this.trayContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon timesheetTrayIcon;
        private System.Windows.Forms.CheckBox systemTrayCheckbox;
        private System.Windows.Forms.ContextMenuStrip trayContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem trayExitMenuItem;
    }
}

