using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using System.Linq;
using OpenAppsProject.Retriever_Class;
using OpenAppsProject.Windows;

namespace OpenAppsProject.Speech_Class
{
    internal class Speech_recognition
    {

        private static System.Speech.Synthesis.SpeechSynthesizer synthesizer;
        private static Dictionary<string, string> appCommands;
        private static Microsoft.CognitiveServices.Speech.SpeechRecognizer msRecognizer;
        private static System.Speech.Recognition.SpeechRecognitionEngine recognizer;
        private static bool commandProcessed = false; //Flag to indicate command processing
        private static bool commandProcessedSearch = false; //Flag to indicate command processing for search

        private bool _muteInput = true;

        public void muteSpeech(bool muteInput)
        {
            _muteInput = muteInput;
        }
        public Speech_recognition()
        {
        }
        public async Task run()
        {
            if (_muteInput)
            {
                Console.WriteLine("Initializing synthesizer...");
                synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
                Console.WriteLine("Initializing application commands...");
                InitializeAppCommands();

                Console.WriteLine("Setting up System.Speech recognition...");
                recognizer = new System.Speech.Recognition.SpeechRecognitionEngine();
                recognizer.SetInputToDefaultAudioDevice();
                Console.WriteLine("Loading Grammar for System.Speech recognition...");

                try
                {
                    var grammar = new System.Speech.Recognition.Grammar(new System.Speech.Recognition.GrammarBuilder(new System.Speech.Recognition.Choices(appCommands.Keys.ToArray())));
                    recognizer.LoadGrammar(grammar);
                    Console.WriteLine("Grammar loaded successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading grammar: {ex.Message}");
                }

                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                recognizer.RecognizeAsync(System.Speech.Recognition.RecognizeMode.Multiple);

                Console.WriteLine("Setting up Cognitive Services speech recognition...");
                var config = SpeechConfig.FromSubscription(new APIKey().getAPIKey(), "eastus");
                msRecognizer = new Microsoft.CognitiveServices.Speech.SpeechRecognizer(config);
                msRecognizer.Recognized += MsRecognizer_Recognized;

                await msRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
                Console.WriteLine("Speech recognition services are running...");

                Console.ReadLine();

                Console.WriteLine("Stopping speech recognitions...");
                recognizer.RecognizeAsyncStop();
                await msRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                Console.WriteLine("Speech recognition services stopped.");
            }
        }


        private static void Recognizer_SpeechRecognized(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            if (_muteInput)
                    return;
                Console.WriteLine($"System.Speech recognized: {e.Result.Text}");
            string command = e.Result.Text.ToLowerInvariant();

            Console.WriteLine($"Processed command: {command}");  // Debugging line

            if (appCommands.ContainsKey(command))
            {
                Console.WriteLine($"Command found in appCommands: {command}");


                Console.WriteLine($"Executing command: {command}");
                if ((command.StartsWith("search") || command.StartsWith("search for") || command.StartsWith("search up")) && !commandProcessedSearch)
                {
                    Console.WriteLine("Search command detected. Asking for search query...");
                    synthesizer.Speak("What would you like to search for?");
                    commandProcessed = true;
                    commandProcessedSearch = true;
                }
                else if (command.StartsWith("show me") || command.StartsWith("list") || command.StartsWith("show"))
                {

                    if (command.Contains("steam games") || command.Contains("steam library"))
                    {
                        Console.WriteLine("Steam command detected. Retrieving Steam games...");
                        Program.getSteamGamesRetriever().showLib();
                        commandProcessed = true;
                    }

                }
                else
                {
                    synthesizer.SpeakAsync($"Opening {command.Split(' ')[1]}");
                    Process.Start(appCommands[command]);
                    commandProcessed = true;
                }
            }
            else
            {
                Console.WriteLine("Command not found in appCommands.");
            }
            Console.WriteLine($"Command processed flag status: {commandProcessed}");

        }

        private static void MsRecognizer_Recognized(object sender, Microsoft.CognitiveServices.Speech.SpeechRecognitionEventArgs e)
        {

            if (commandProcessed && !commandProcessedSearch)
            {
                Console.WriteLine("Command already processed by System.Speech. Ignoring Cognitive Services recognition.");
                commandProcessed = false; // Reset the flag
                return;
            }
            if (commandProcessedSearch)
            {
                if (e.Result.Reason == Microsoft.CognitiveServices.Speech.ResultReason.RecognizedSpeech)
                {
                    string command = e.Result.Text.ToLowerInvariant();
                    Console.WriteLine($"Cognitive Services recognized: {command}");
                    string searchQuery = RemoveCommandPrefix(command, new[] { "search", "search for", "search up" });
                    Console.WriteLine($"Executing search for: {searchQuery}");
                    synthesizer.SpeakAsync($"Searching for {searchQuery}");
                    Process.Start("chrome.exe", $"http://www.google.com/search?q={Uri.EscapeDataString(searchQuery)}");
                    commandProcessedSearch = false;// Done Searhcing
                    Console.WriteLine($"Executed search for: {searchQuery} commandProcessedSearch is set {commandProcessedSearch}");

                }
                else
                {
                    Console.WriteLine($"Speech recognition failed with reason: {e.Result.Reason}");
                }
            }
        }



        private static void InitializeAppCommands()
        {
            appCommands = new Dictionary<string, string>
            {
                { "open notepad", "notepad.exe" },
                { "open calculator", "calc.exe" },
                { "open minecraft", @"C:\Users\gilla\AppData\Local\Programs\launcher\Lunar Client.exe" },
                { "open chrome", @"C:\Program Files\Google\Chrome\Application\chrome.exe" },
                { "open visual studio 22", @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" }, 
                { "open steam", @"C:\Program Files (x86)\Steam\steam.exe" },
                { "open spotify", @"C:\Users\gilla\AppData\Roaming\Spotify\Spotify.exe" },
                { "open file explorer", "explorer" }
            };

            // Special commands that don't directly map to an executable
            appCommands.Add("show me steam games", null);
            appCommands.Add("list steam games", null);
            appCommands.Add("show steam games", null);
            appCommands.Add("show me steam library", null);
            appCommands.Add("list steam library", null);
            appCommands.Add("show steam library", null);
            appCommands.Add("show desktop", null);
            appCommands.Add("search", null);
            appCommands.Add("search for", null);
            appCommands.Add("search up", null);

        }

        private static string RemoveCommandPrefix(string command, string[] prefixes)
        {
            foreach (var prefix in prefixes)
            {
                if (command.StartsWith(prefix))
                {
                    return command.Substring(prefix.Length).TrimStart();
                }
            }
            return command;
        }

    }
}
