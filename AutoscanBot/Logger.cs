﻿using System;
using System.Collections.Generic;
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
            string? isLogEnabled = Configuration.GetItemValueByName("logs_enable_stdio");
            if (isLogEnabled == null || isLogEnabled?.Trim()?.ToLower() != "true") return;

            if (message?.Length == 0) return;
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
