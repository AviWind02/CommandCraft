using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenAppsProject.Speech_Class;
using OpenAppsProject.Windows;

namespace OpenAppsProject
{
    internal class Program
    {


        static async Task Main(string[] args)
        {
            var speechTask = new Speech_recognition().run(); // Start the speech recognition task

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UserForm()); // This will block until the form is closed

            await speechTask; // wait for the speech task to complete after the form is closed
        }

       

    }
}
