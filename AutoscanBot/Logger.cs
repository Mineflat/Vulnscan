using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot
{
    internal class Logger
    {
        public enum LogLevel
        {
            MESSAGE, SUCCESS, INFO, ERROR, CRITICAL
        }
        public static void Log(LogLevel level, string? message)
        {
            if (message?.Length == 0) return;
            switch (level)
            {
                case LogLevel.MESSAGE:
                    Console.ForegroundColor = ConsoleColor.Cyan;
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
            Console.Write($"[{DateTime.Now.ToUniversalTime()}][{level}]");
            Console.ResetColor();
            Console.WriteLine($": {message}");
        }
    }
}
