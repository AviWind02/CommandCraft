using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenAppsProject.Retriever_Class;
using OpenAppsProject.Speech_Class;
using OpenAppsProject.Windows;

namespace OpenAppsProject
{
    internal class Program
    {

        private static SteamGamesRetriever steamGamesRetriever;

        static async Task Main(string[] args)
        {
           var speechTask = new Speech_recognition().run(); // Start the speech recognition task
            steamGamesRetriever = new SteamGamesRetriever();// Trying to get all steam games from all steams lib(Drives) on the computer
            steamGamesRetriever.run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UserForm());
 

            await speechTask; // wait for the speech task to complete after the form is closed
        }

       public static SteamGamesRetriever getSteamGamesRetriever() {

            return steamGamesRetriever;
       }
    
    }
}
