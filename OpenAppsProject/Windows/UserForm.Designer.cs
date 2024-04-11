namespace OpenAppsProject.Windows
{
    partial class UserForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonSaveLog = new System.Windows.Forms.Button();
            this.buttonMute = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 314);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(6, 19);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(764, 289);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // buttonSaveLog
            // 
            this.buttonSaveLog.Location = new System.Drawing.Point(632, 415);
            this.buttonSaveLog.Name = "buttonSaveLog";
            this.buttonSaveLog.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveLog.TabIndex = 5;
            this.buttonSaveLog.Text = "Save to file";
            this.buttonSaveLog.UseVisualStyleBackColor = true;
            this.buttonSaveLog.Click += new System.EventHandler(this.buttonSaveLog_Click);
            // 
            // buttonMute
            // 
            this.buttonMute.Location = new System.Drawing.Point(713, 415);
            this.buttonMute.Name = "buttonMute";
            this.buttonMute.Size = new System.Drawing.Size(75, 23);
            this.buttonMute.TabIndex = 0;
            this.buttonMute.Text = "Mute";
            this.buttonMute.UseVisualStyleBackColor = true;
            this.buttonMute.Click += new System.EventHandler(this.buttonMute_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.RosyBrown;
            this.buttonExit.Location = new System.Drawing.Point(93, 415);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.Text = "Kill App";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.BackColor = System.Drawing.Color.RosyBrown;
            this.buttonClearLog.Location = new System.Drawing.Point(12, 415);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 4;
            this.buttonClearLog.Text = "Clear";
            this.buttonClearLog.UseVisualStyleBackColor = false;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonMute);
            this.Controls.Add(this.buttonSaveLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonClearLog);
            this.Name = "UserForm";
            this.Text = "UserForm";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSaveLog;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonMute;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonClearLog;
    }
}