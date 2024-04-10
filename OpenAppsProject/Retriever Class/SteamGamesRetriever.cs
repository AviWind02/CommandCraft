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
            RetrieveGames();
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
                        var exeFiles = Directory.GetFiles(gameDir, "*.exe", SearchOption.TopDirectoryOnly)
                                                .Where(file => !file.Contains("Crash") && !file.Contains("Handler"));

                        if (exeFiles.Any())
                        {
                            var exePath = exeFiles.First();
                            Console.WriteLine($"Found game executable: {exePath}");
                            Games.Add((Path.GetFileName(gameDir), exePath));
                        }
                        else
                        {
                            Console.WriteLine($"No valid executable found in: {gameDir}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Directory does not exist: {path}");
                }
            }
        }

        public void run()
        {
            Console.WriteLine("Running SteamGamesRetriever...");
            foreach (var game in Games)
            {
                Console.WriteLine($"Game: {game.GameName}, Executable: {game.ExePath}");
            }
        }
    }
}
