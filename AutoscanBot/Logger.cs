using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot
{
    internal class Logger
    {
        public enum LogLevel
        {
            MESSAGE, COMMAND, SUCCESS, INFO, ERROR, CRITICAL
        }

        public static void LogTTY(LogLevel level, string? message, int ttyNum)
        {
            if (System.OperatingSystem.IsLinux())
            {
                string preset = string.Empty;
                switch (level)
                {
                    case LogLevel.ERROR:
                        preset = $"\\e[31m[{DateTime.Now}][{level}]:\\e[0m ";
                        break;
                    case LogLevel.CRITICAL:
                        preset = $"\\e[4;31m{DateTime.Now}][{level}]:\\e[0m ";
                        break;
                    case LogLevel.SUCCESS:
                        preset = $"\\e[32m[{DateTime.Now}][{level}]:\\e[0m ";
                        break;
                    case LogLevel.INFO:
                        preset = $"\\e[33m[{DateTime.Now}][{level}]:\\e[0m ";
                        break;
                    case LogLevel.COMMAND:
                        preset = $"\\e[42;33m[{DateTime.Now}][{level}]:\\e[0m ";
                        break;
                    case LogLevel.MESSAGE:
                        preset = $"\\e[36m[{DateTime.Now}][{level}]:\\e[0m ";
                        break;
                }
                //
                Process.Start("/bin/echo", new string[]
                {
                    $"-en",
                    $"\"\\e[1G{preset}\" >> /dev/tty{ttyNum}"
                });
                Process.Start("/bin/echo", new string[]
                {
                    $"-E",
                    $"'{message}'" +
                    $">> /dev/tty{ttyNum}"
                }
                );
                Process.Start("/bin/echo", $"-E '{message}' >> /dev/tty{ttyNum}");

                //Process.Start("/bin/echo", $"-en \"\\e[1G{preset}\" >> /dev/tty{ttyNum}");
                //Process.Start("/bin/echo", $"-E '{message}' >> /dev/tty{ttyNum}");

                //string command = $"echo -en '\\\\e[1G{preset}' >> /dev/pts/{ttyNum}; echo -E '{message}' >> /dev/pts/{ttyNum}\"";

                //Process.Start("/bin/bash -с", $"echo -en '\\e[1G{preset}' >> /dev/pts/{ttyNum}; echo -E '{message}' >> /dev/pts/{ttyNum}");
            }
        }
        public static void FSLog(LogLevel level, string? message)
        {
            Log(level, message);
            try
            {
                if (Storage.LogPath != null)
                {
                    File.AppendAllText(Storage.LogPath, $"[{DateTime.Now}][{level}]: {message}\n");
                }
                else
                {
                    Log(LogLevel.ERROR, "Failed to write logs to a file. Reason: Empty path to log file");
                    Log(LogLevel.INFO, "Switching 'LOGS_ENABLE_FS_WRITER' to 'false'");
                    Configuration.SwitchLogType(false);
                }
            }
            catch (Exception fileWritingException)
            {
                Log(LogLevel.ERROR, "Failed to write logs to a file." +
                    $"Reason:\n{fileWritingException}");
                Log(LogLevel.INFO, "Switching 'LOGS_ENABLE_FS_WRITER' to 'false'");
                Configuration.SwitchLogType(false);
            }
        }
        public static void Log(LogLevel level, string? message)
        {
            if (int.TryParse(Configuration.GetItemValueByName("logs_tty_number"), out int tty))
            {
                LogTTY(level, message, tty);
            }
            else
            {
                switch (level)
                {
                    case LogLevel.MESSAGE:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case LogLevel.COMMAND:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        break;
                    case LogLevel.SUCCESS:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogLevel.INFO:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevel.ERROR:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogLevel.CRITICAL:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Red;
                        break;
                }
                Console.Write($"[{DateTime.Now}][{level}]");
                Console.ResetColor();
                Console.WriteLine($": {message}");
            }
        }
    }
}
