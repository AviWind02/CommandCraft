using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenAppsProject.Retriever_Class
{
    internal class SteamGamesRetriever
    {
        public List<(string GameName, string ExePath)> Games { get; private set; }

        public SteamGamesRetriever()
        {
            Games = new List<(string GameName, string ExePath)>();
        }

   


        private void RetrieveGames()
        {
            Console.WriteLine("Retrieving Steam games...");

            var steamPaths = new List<string>
            {
                @"C:\Program Files (x86)\Steam\steamapps\common"
            };

            Console.WriteLine("Checking for additional Steam library folders on other drives...");
            foreach (var drive in DriveInfo.GetDrives())
            {
                Console.WriteLine($"Checking drive: {drive.Name}");
                if (drive.IsReady)
                {
                    string steamLibPath = Path.Combine(drive.Name, "SteamLibrary", "steamapps", "common");
                    if (Directory.Exists(steamLibPath))
                    {
                        Console.WriteLine($"Found Steam library at: {steamLibPath}");
                        steamPaths.Add(steamLibPath);
                    }
                    else
                    {
                        Console.WriteLine($"No Steam library found at: {steamLibPath}");
                    }
                }
                else
                {
                    Console.WriteLine($"Drive {drive.Name} is not ready.");
                }
            }

            foreach (var path in steamPaths)
            {
                Console.WriteLine($"Scanning directory: {path}");
                if (Directory.Exists(path))
                {
                    var gameDirectories = Directory.GetDirectories(path);
                    foreach (var gameDir in gameDirectories)
                    {
                        Console.WriteLine($"Checking game directory: {gameDir}");

                        // First, check for executables only in the top directory
                        var exeFilesTopDirectory = Directory.GetFiles(gameDir, "*.exe", SearchOption.TopDirectoryOnly);
                        var gameExe = FindGameExecutable(exeFilesTopDirectory);

                        // If no executable found, search in all subdirectories
                        if (gameExe == null)
                        {
                            var exeFilesAllDirectories = Directory.GetFiles(gameDir, "*.exe", SearchOption.AllDirectories);
                            gameExe = FindGameExecutable(exeFilesAllDirectories);
                        }

                        if (gameExe != null)
                        {
                            Console.WriteLine($"Found game executable: {gameExe}");
                            Games.Add((Path.GetFileName(gameDir), gameExe));
                        }
                        else
                        {
                            Console.WriteLine($"No executable found in: {gameDir}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Directory does not exist: {path}");
                }
            }
        }

        private string FindGameExecutable(string[] exeFiles)
        {
            string preferredExeFile = null;

            // Filter out unwanted executables
            List<string> filteredExeFiles = new List<string>();
            foreach (var file in exeFiles)
            {
                string fileName = Path.GetFileName(file);
                if (!fileName.Contains("Unity") &&
                    !fileName.Contains("Crash") &&
                    !fileName.Contains("UnityCrashHandler") &&
                    !fileName.Contains("UnityCrashHandler64") &&
                    !fileName.Contains("Handler"))
                {
                    filteredExeFiles.Add(file);
                }
            }

            // Prioritize .exe files starting with 'start'
            foreach (var file in filteredExeFiles)
            {
                if (Path.GetFileName(file).StartsWith("start", StringComparison.OrdinalIgnoreCase))
                {
                    preferredExeFile = file;
                    break;
                }
            }

            // If no preferred .exe is found, use the largest .exe file
            if (preferredExeFile == null)
            {
                long largestSize = 0;
                foreach (var file in filteredExeFiles)
                {
                    long size = new FileInfo(file).Length;
                    if (size > largestSize)
                    {
                        largestSize = size;
                        preferredExeFile = file;
                    }
                }
            }

            return preferredExeFile;
        }
        public void run()
        {
            RetrieveGames();
        }
        public void showLib()
        {
            Console.WriteLine("Running SteamGamesRetriever...");
            foreach (var game in Games)
            {
                Console.WriteLine($"Game: {game.GameName}, Executable: {game.ExePath}");
            }
        }
    }
}
