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
                Configuration.Log.Invoke(Logger.LogLevel.CRITICAL, Properties.Resources.BadUsageMessage);
                return false;
            }
            Configuration.Preset = Configuration.TryRead(args[0]);
            if (Configuration.Preset == null)
            {
                Configuration.Log.Invoke(Logger.LogLevel.CRITICAL, $"File '{args[0]}' doesn't exist or invalid. Create and fill it with required variables before starting\n" +
                    $"Config missing this things:\n{Properties.Resources.RequiredVariables}");
                return false;
            }
            Configuration.Log.Invoke(Logger.LogLevel.SUCCESS, "Configuration done, starting system");
            return true;
        }
        //private static bool InstallWorkingDirectory()
        //{
        //    string? path = Configuration.GetItemValueByName("WORKING_DIRECTORY");
        //    if (path == null) return false;
        //    if (Directory.Exists(path)) Directory.CreateDirectory(path);
        //    return true;
        //}
        static void Main(string[] args)
        {
            if (!CheckArgs(args)) return;
            if (!Storage.SetLogPath()) return;
            Configuration.ConfigureLog();
            Configuration.Log(Logger.LogLevel.INFO, $"{new string('=', 10)} SYSTEM STARTED AT {DateTime.Now} {new string('=', 10)}");
            Telegramm.Bot.Start();
            Console.ReadLine();
        }
    }
}