namespace TimesheetWebApp
{
    partial class TimesheetApp
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
            this.addressTextbox = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addressTextbox
            // 
            this.addressTextbox.AutoSize = true;
            this.addressTextbox.Location = new System.Drawing.Point(13, 13);
            this.addressTextbox.Name = "addressTextbox";
            this.addressTextbox.Size = new System.Drawing.Size(114, 13);
            this.addressTextbox.TabIndex = 0;
            this.addressTextbox.Text = "Timesheet standalone.";
            // 
            // TimesheetApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 94);
            this.Controls.Add(this.addressTextbox);
            this.Name = "TimesheetApp";
            this.Text = "Timesheet Standalone";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label addressTextbox;
    }
}

