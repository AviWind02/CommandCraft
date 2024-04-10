using OpenAppsProject.Misc_Class;
using OpenAppsProject.Speech_Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenAppsProject.Windows
{
    public partial class UserForm : Form
    {
        public bool muteInput = false;
        public UserForm()
        {
            InitializeComponent();
            buttonMute.BackColor = Color.White;
            RichTextBoxStreamWriter richTextBoxStreamWriter = new RichTextBoxStreamWriter(richTextBoxLog);
            Console.SetOut(richTextBoxStreamWriter);
        }

        private void buttonSaveLog_Click(object sender, EventArgs e)
        {
            SaveRichTextBoxContent();

        }

        private void SaveRichTextBoxContent()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string filename = $"VoxCommand - {timestamp}.log";
            string path = Path.Combine(Application.StartupPath, filename);

            try
            {
                File.WriteAllText(path, richTextBoxLog.Text);
                MessageBox.Show($"Log saved to {path}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonMute_Click(object sender, EventArgs e)
        {
            Speech_recognition speechRecognition = new Speech_recognition();

            muteInput = !muteInput;

            // Update button background color based on mute state
            buttonMute.BackColor = muteInput ? Color.Orange : Color.White;

            // Assuming you have an existing instance of Speech_recognition
            // Update its mute state
            speechRecognition.muteSpeech(muteInput);  // Make sure you have such a method in Speech_recognition
        }
    }
}
