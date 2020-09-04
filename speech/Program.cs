using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace speech
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Speech.Speak("some text to speech");
        }
    }

    public static class Speech
    {
        public static void Speak(string text, bool wait = false)
        {
            ExecuteCommand(
                $@"Add-Type -AssemblyName System.speech; 
                $speak = New-Object System.Speech.Synthesis.SpeechSynthesizer; 
                $speak.Speak(""{text}"");");

            void ExecuteCommand(string command)
            {
                string path = Path.GetTempPath() + Guid.NewGuid() + ".ps1";

                // make sure to be using System.Text
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.Write(command);

                    ProcessStartInfo start = new ProcessStartInfo()
                    {
                        FileName = @"C:\Windows\System32\windowspowershell\v1.0\powershell.exe",
                        LoadUserProfile = false,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Arguments = $"-executionpolicy bypass -File {path}",
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    Process process = Process.Start(start);

                    if (wait)
                        process.WaitForExit();
                }
            }
        }
    }
}
