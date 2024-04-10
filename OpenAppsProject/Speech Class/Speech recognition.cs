using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using System.Linq;
using OpenAppsProject.Retriever_Class;

namespace OpenAppsProject.Speech_Class
{
    internal class Speech_recognition
    {

        private static System.Speech.Synthesis.SpeechSynthesizer synthesizer;
        private static Dictionary<string, string> appCommands;
        private static Microsoft.CognitiveServices.Speech.SpeechRecognizer msRecognizer;
        private static System.Speech.Recognition.SpeechRecognitionEngine recognizer;
        private static bool commandProcessed = false; //Flag to indicate command processing

        public async Task run()
        {
            Console.WriteLine("Initializing synthesizer...");
            synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();

            Console.WriteLine("Initializing application commands...");
            InitializeAppCommands();

            Console.WriteLine("Setting up System.Speech recognition...");
            recognizer = new System.Speech.Recognition.SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();

            var grammar = new System.Speech.Recognition.Grammar(new System.Speech.Recognition.GrammarBuilder(new System.Speech.Recognition.Choices(appCommands.Keys.ToArray())));
            recognizer.LoadGrammar(grammar);

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.RecognizeAsync(System.Speech.Recognition.RecognizeMode.Multiple);

            Console.WriteLine("Setting up Cognitive Services speech recognition...");
            var config = SpeechConfig.FromSubscription("01eeed71b753442098af2ed03a8f2cef", "eastus");
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


        private static void Recognizer_SpeechRecognized(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            Console.WriteLine($"System.Speech recognized: {e.Result.Text}");
            string command = e.Result.Text.ToLowerInvariant();

            Console.WriteLine($"Processed command: {command}");  // Debugging line


            
            if (appCommands.ContainsKey(command))
            {


                Console.WriteLine($"Executing command: {command}");
                if (command.StartsWith("show me") || command.StartsWith("list") || command.StartsWith("show"))
                {

                    if (command.Contains("steam games") || command.Contains(" steam library"))
                    {
                        Console.WriteLine("Steam command detected. Retrieving Steam games...");
                        new SteamGamesRetriever().run();
                        commandProcessed = true;
                    }
                    return;  //Dont need to Execute opening - This only displays it

                }

                synthesizer.SpeakAsync($"Opening {command.Split(' ')[1]}");
                Process.Start(appCommands[command]);
                commandProcessed = true;

            }
            else
            {
                Console.WriteLine("Command not found in appCommands.");
            }
        }

        private static void MsRecognizer_Recognized(object sender, Microsoft.CognitiveServices.Speech.SpeechRecognitionEventArgs e)
        {
            return; // I really dont want to waste my free shit

            if (commandProcessed)
            {
                Console.WriteLine("Command already processed by System.Speech. Ignoring Cognitive Services recognition.");
                commandProcessed = false; // Reset the flag
                return;
            }

            if (e.Result.Reason == Microsoft.CognitiveServices.Speech.ResultReason.RecognizedSpeech)
            {
                string command = e.Result.Text.ToLowerInvariant();
                Console.WriteLine($"Cognitive Services recognized: {command}");

                // Check if the command is a search command
                if (command.StartsWith("search") || command.StartsWith("search for") || command.StartsWith("search up"))
                {
                    string searchQuery = RemoveCommandPrefix(command, new[] { "search", "search for", "search up" });
                    Console.WriteLine($"Executing search for: {searchQuery}");
                    synthesizer.SpeakAsync($"Searching for {searchQuery}");
                    Process.Start("chrome.exe", $"http://www.google.com/search?q={Uri.EscapeDataString(searchQuery)}");
                }
                else
                {
                    Console.WriteLine("Command is not a search command.");
                }
            }
            else
            {
                Console.WriteLine($"Speech recognition failed with reason: {e.Result.Reason}");
            }
        }



        private static void InitializeAppCommands()
        {
            appCommands = new Dictionary<string, string>
            {
                { "open notepad", "notepad.exe" },
                { "open calculator", "calc.exe" },
                { "open minecraft", @"C:\Users\gilla\AppData\Local\Programs\launcher\Lunar Client.exe" }
                   
            };
            //Special commands that don't directly map to an executable
            appCommands.Add("show me steam games", null);
            appCommands.Add("list steam games", null);
            appCommands.Add("show steam games", null);
            appCommands.Add("show me steam library", null);
            appCommands.Add("list steam library", null);
            appCommands.Add("show steam library", null);
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
