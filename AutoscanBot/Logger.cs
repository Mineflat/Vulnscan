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
        public static byte? ttyNum;
        public static void Log(LogLevel level, string message, bool logTTY = true, bool logFS = true)
        {
            if (logTTY)
            {
                if (ttyNum.HasValue)
                {
                    if (ttyNum > 0 && ttyNum < 13)
                    {
                        LogTTY(level, message);
                    }
                }
            }
            if (logFS)
            {
                FSLog(level, message);
            }
        }
        private static void LogTTY(LogLevel level, string message)
        {
            if (!OperatingSystem.IsLinux()) return;
            string preset = string.Empty;
            switch (level)
            {
                case LogLevel.ERROR:
                    preset = $"\\e[1;31m[{DateTime.Now}][{level}]:\\e[0m ";
                    break;
                case LogLevel.CRITICAL:
                    preset = $"\\e[4;31m{DateTime.Now}][{level}]:\\e[0m ";
                    break;
                case LogLevel.SUCCESS:
                    preset = $"\\e[1;32m[{DateTime.Now}][{level}]:\\e[0m ";
                    break;
                case LogLevel.INFO:
                    preset = $"\\e[1;33m[{DateTime.Now}][{level}]:\\e[0m ";
                    break;
                case LogLevel.COMMAND:
                    preset = $"\\e[1;42;33m[{DateTime.Now}][{level}]:\\e[0m ";
                    break;
                case LogLevel.MESSAGE:
                    preset = $"\\e[1;36m[{DateTime.Now}][{level}]:\\e[0m ";
                    break;
            }
            Process.Start("./MODULES/CORE/LogTTY.sh", new string[]
            {
                message,
                $"{ttyNum}"
            });
        }
        private static void FSLog(LogLevel level, string? message)
        {
            try
            {
                if (Storage.LogPath != null)
                {
                    File.AppendAllText($"{Storage.LogPath}latest.log", $"[{DateTime.Now}][{level}]: {message}\n");
                }
            }
            catch (Exception logWriteException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now}][CRITICAL] Error occupied when writing logs in file. Reason:\n{logWriteException.Message}\n{new string('=', 10)}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[{DateTime.Now}][INFO] The logging service has been deactivated. No more logs will be stored.");
                Console.ResetColor();
            }
        }
    }
}
