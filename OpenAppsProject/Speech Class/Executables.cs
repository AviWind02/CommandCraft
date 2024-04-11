using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAppsProject.Speech_Class
{
    internal class Executables_Class
    {
        public string[] executables = { "open", "close", "kill" };
        public static void ExecuteApplicationAction(string action, string appName, string executablePath)
        {
            Console.WriteLine($"Executing action '{action}' on '{appName}'");

            // Find the process by its name (without .exe extension)
            var processes = Process.GetProcessesByName(appName.Replace(".exe", ""));
            Console.WriteLine($"Found {processes.Length} process(es) with the name '{appName}'.");

            switch (action)
            {
                case "open":
                    if (processes.Length == 0) // Check if the app is not already running
                    {
                        Console.WriteLine($"Opening application '{appName}'.");
                        Process.Start(executablePath); // Open the application
                    }
                    else
                    {
                        Console.WriteLine($"Application '{appName}' is already running.");
                    }
                    break;

                case "close":
                    foreach (var process in processes)
                    {
                        Console.WriteLine($"Closing application '{appName}' (process ID: {process.Id}).");
                        process.CloseMainWindow(); // Close the application gracefully
                    }
                    break;

                case "kill":
                    foreach (var process in processes)
                    {
                        Console.WriteLine($"Killing application '{appName}' (process ID: {process.Id}).");
                        process.Kill(); // Forcefully close the application
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized action.");
                    break;
            }
        }
    }
}
