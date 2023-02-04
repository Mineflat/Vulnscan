using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot.Shell
{
    internal class Shell
    {
        private static string Prompt = AutoscanBot.Properties.Resources.SHELL_Prompt;
        private static Thread? ShellThread;
        public static bool Start()
        {
            if (Configuration.GetItemValueByName("ENABLE_SHELL_START") == "true")
            {
                return RunShell();
            }
            return false;
        }
        private static bool RunShell()
        {
            Logger.Log(Logger.LogLevel.INFO, "Starting shell session");
            ShellThread = new Thread(x =>
            {
                StartReceiving();
            });
            ShellThread.Name = "Autoscan Shell processor";
            ShellThread.IsBackground = true;
            ShellThread.Start();
            Logger.Log(Logger.LogLevel.INFO, $"Shell session started. Current thread state: {ShellThread.IsAlive}; Name: '{ShellThread.Name}'");
            return ShellThread.IsAlive;
        }
        private static void StartReceiving()
        {
            while (true)
            {
                // Drowing prompt
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Prompt);
                Console.ResetColor();
                // and start receiving
                string? command = Console.ReadLine();
                if (string.IsNullOrEmpty(command)) continue;
                command = command.Trim();
                if (command?.Length > 0)
                {

                }
            }
        }
    }
}
