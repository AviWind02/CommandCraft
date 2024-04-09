using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Diagnostics;
using System.Collections.Generic;

namespace OpenAppsProject
{
    internal class Program
    {
        private static SpeechSynthesizer synthesizer;

        static void Main(string[] args)
        {
            try
            {
                synthesizer = new SpeechSynthesizer();
                SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();


                string wakeWord = "Jarvis";
                Choices commands = new Choices();
                commands.Add(new string[] { "open notepad", "open calculator", "search" });



                GrammarBuilder gb = new GrammarBuilder();
                gb.Append(wakeWord);
                gb.Append(commands);

                gb.AppendDictation();

                Grammar g = new Grammar(gb);

                recognizer.LoadGrammar(g);
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                recognizer.SpeechDetected += Recognizer_SpeechDetected;
                recognizer.SpeechRecognitionRejected += Recognizer_SpeechRecognitionRejected;
                recognizer.RecognizeCompleted += Recognizer_RecognizeCompleted;

                recognizer.SetInputToDefaultAudioDevice();
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                Console.WriteLine("Listening for commands...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing speech recognition: " + ex.Message);
            }

            Console.ReadLine();
        }
    
        static void Recognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            Console.WriteLine($"Speech detected at AudioPosition: {e.AudioPosition}");
        }
        static void Recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Speech recognition rejected.");
        }

        static void Recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("Error in recognition: " + e.Error.Message);
            }
            if (e.Cancelled)
            {
                Console.WriteLine("Recognition cancelled.");
            }
            Console.WriteLine("Recognition completed.");
        }

        static void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string spokenText = e.Result.Text;
            Console.WriteLine("Recognized command: " + spokenText);

            if (spokenText.StartsWith("Jarvis"))
            {
                string recognizedText = e.Result.Text;
                recognizedText = PostProcessText(recognizedText);

                string command = spokenText.Substring("Jarvis".Length).TrimStart();
                if (command.StartsWith("open notepad"))
                {
                    synthesizer.SpeakAsync("Opening Notepad");
                    Process.Start("notepad.exe");
                }
                else if (command.StartsWith("open calculator"))
                {
                    synthesizer.SpeakAsync("Opening Calculator");
                    Process.Start("calc.exe");
                }
                // Check for specific patterns
                else if (command.StartsWith("search for"))
                {
                    HandleSearchCommand(recognizedText);
                }
                else if (command.StartsWith("search"))
                {
                    string searchQuery = command.Substring("search".Length).TrimStart();
                    synthesizer.SpeakAsync($"Searching for {searchQuery}");
                    Process.Start("chrome.exe", $"http://www.google.com/search?q={Uri.EscapeDataString(searchQuery)}");
                }
                else
                {
                    synthesizer.SpeakAsync("Command not recognized");
                }
            }
            else
            {
                synthesizer.SpeakAsync("Wake word not recognized");
            }
        }

        static void HandleSearchCommand(string command)
        {


            string searchQuery = command.Substring("search for".Length).TrimStart();
            searchQuery = PostProcessText(searchQuery); // Apply corrections here
            //synthesizer.Speak($"Did you say: search for {searchQuery}?");
            //Console.WriteLine("Press 'y' to confirm, 'n' to cancel.");

            if (/*Console.ReadKey().Key == ConsoleKey.Y*/ true)//For now
            {
                synthesizer.SpeakAsync($"Searching for {searchQuery}");
                Process.Start("chrome.exe", $"http://www.google.com/search?q={Uri.EscapeDataString(searchQuery)}");
            }
            else
            {
                synthesizer.SpeakAsync("Search cancelled.");
            }
        }
        static string PostProcessText(string recognizedText)
        {
            // Dictionary of known misrecognitions and corrections
            var corrections = new Dictionary<string, string>
            {
                { "You To", "YouTube" },
                // Add more known corrections here
            };

            foreach (var correction in corrections)
            {
                recognizedText = recognizedText.Replace(correction.Key, correction.Value);
            }

            return recognizedText;
        }

    }
}
