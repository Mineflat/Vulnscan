using AutoscanBot.Core;
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
                Logger.Log(Logger.LogLevel.INFO, "Starting shell session");
                return RunShell();
            }
            Logger.Log(Logger.LogLevel.INFO, "Shell off due the config valie");
            return false;
        }
        private static bool RunShell()
        {
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
                    Console.WriteLine(ProcessMessage(command));
                }
            }
        }
        private static string? ProcessMessage(string message)
        {
            foreach (Command command in Command.AvailibleCommands)
            {
                if (command.InvokeName.ToLower() == message)
                {
                    if (command.LinkedTo != null)
                    {
                        CommandExecutionResult result = command.LinkedTo.Invoke();
                        if (result != null && result.Success)
                        {
                            return result.Message;
                        }
                    }
                    else
                    {
                        return "Эта команда, конечно, существует, но мой создатель пока ее не реализовал. Пните этого человека чтобы получить больше информаци";
                    }
                }
            }
            return string.Empty;
        }
    }
}
