using System;
using System.Diagnostics;

namespace AutoscanBot
{
    internal class Logger
    {
        public enum LogLevel
        {
            MESSAGE, COMMAND, SUCCESS, INFO, ERROR, CRITICAL
        }
        public static int? ttyNum;
        //public static void LogTTY(LogLevel level, string? message)
        //{
        //    string preset = string.Empty;
        //    switch (level)
        //    {
        //        case LogLevel.ERROR:
        //            preset = $"\\e[31m[{DateTime.Now}][{level}]:\\e[0m ";
        //            break;
        //        case LogLevel.CRITICAL:
        //            preset = $"\\e[4;31m{DateTime.Now}][{level}]:\\e[0m ";
        //            break;
        //        case LogLevel.SUCCESS:
        //            preset = $"\\e[32m[{DateTime.Now}][{level}]:\\e[0m ";
        //            break;
        //        case LogLevel.INFO:
        //            preset = $"\\e[33m[{DateTime.Now}][{level}]:\\e[0m ";
        //            break;
        //        case LogLevel.COMMAND:
        //            preset = $"\\e[42;33m[{DateTime.Now}][{level}]:\\e[0m ";
        //            break;
        //        case LogLevel.MESSAGE:
        //            preset = $"\\e[36m[{DateTime.Now}][{level}]:\\e[0m ";
        //            break;
        //    }
        //    Process.Start("./MODULES/CORE/", $"-E '{message}' >> /dev/tty{ttyNum}");
        //}
        public static void FSLog(LogLevel level, string? message)
        {
            try
            {
                if (Storage.LogPath != null)
                {
                    File.AppendAllText(Storage.LogPath, $"[{DateTime.Now}][{level}]: {message}\n");
                }
            }
            catch (Exception logWriteException) 
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now}][CRITICAL] Error occupied when writing logs in file. Reason: {logWriteException.Message}");
                Configuration.Log = null;
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.WriteLine($"[{DateTime.Now}][INFO] The logging service has been deactivated. No more logs will be stored.");
                Console.ResetColor();
            }
        }
        //public static void Log(LogLevel level, string? message)
        //{
        //    if (int.TryParse(Configuration.GetItemValueByName("logs_tty_number"), out int tty))
        //    {
        //        LogTTY(level, message);
        //    }
        //    else
        //    {
        //        switch (level)
        //        {
        //            case LogLevel.MESSAGE:
        //                Console.ForegroundColor = ConsoleColor.Cyan;
        //                break;
        //            case LogLevel.COMMAND:
        //                Console.ForegroundColor = ConsoleColor.Yellow;
        //                Console.BackgroundColor = ConsoleColor.DarkGreen;
        //                break;
        //            case LogLevel.SUCCESS:
        //                Console.ForegroundColor = ConsoleColor.Green;
        //                break;
        //            case LogLevel.INFO:
        //                Console.ForegroundColor = ConsoleColor.Yellow;
        //                break;
        //            case LogLevel.ERROR:
        //                Console.ForegroundColor = ConsoleColor.Red;
        //                break;
        //            case LogLevel.CRITICAL:
        //                Console.ForegroundColor = ConsoleColor.White;
        //                Console.BackgroundColor = ConsoleColor.Red;
        //                break;
        //        }
        //        Console.Write($"[{DateTime.Now}][{level}]");
        //        Console.ResetColor();
        //        Console.WriteLine($": {message}");
        //    }
        //}
    }
}
