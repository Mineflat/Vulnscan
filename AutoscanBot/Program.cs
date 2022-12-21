﻿namespace AutoscanBot
{
    internal class Program
    {
        /// <summary>
        /// Осуществляет проверку аргументов, переданных приложению на старте. Возвращает True, если проверка пройдена успешно. 
        /// </summary>
        /// <param name="args">Аргументы, передаваемые приложению на старте</param>
        /// <returns>True, если аргументы валидны, в остальных случаях - False </returns>
        private static bool CheckArgs(string[] args)
        {
            if (args.Length != 1)
            {
                Logger.Log(Logger.LogLevel.CRITICAL, Properties.Resources.BadUsageMessage);
                return false;
            }
            if (Configuration.TryRead(args[0]) != null)
            {
                Logger.Log(Logger.LogLevel.CRITICAL, $"File '{args[0]}' doesn't exist or invalid. Create and fill it with required variables before starting\n" +
                    $"Config must contain this:\n{Properties.Resources.RequiredVariables}");
                return false;
            }
            return true;
        }
        private static void PreconfigureTerminal()
        {
            Console.Title = "Vulnscan";
            Console.Clear();
        }
        static void Main(string[] args)
        {
            if (!CheckArgs(args)) return;
            PreconfigureTerminal();
        }
    }
}