namespace AutoscanBot
{
    internal class Storage
    {
        public static string? LogPath { get; set; }
        public static void ConfigureLog()
        {
            string? workdir = Configuration.GetItemValueByName("WORKING_DIRECTORY");
            if (!Directory.Exists(workdir)) throw new Exception("Configuration error: param 'WORKING_DIRECTORY' is missing or directory missing");
            bool logFSEnabled = Configuration.GetItemValueByName("LOGS_ENABLE_FS_WRITER")?.Trim().ToLower() == "true";
            if (logFSEnabled)
            {
                string logPath = string.Empty;
                if (OperatingSystem.IsWindows())
                {
                    logPath = $"{workdir}\\LOGS\\";
                }
                else
                if (OperatingSystem.IsLinux())
                {
                    logPath = $"{workdir}/LOGS/";
                    if(byte.TryParse(Configuration.GetItemValueByName("LOGS_TTY_NUMBER"), out byte tty))
                    {
                        if(tty > 0 && tty < 13)
                        {
                            Logger.ttyNum = tty;
                        }
                        else
                        {
                            Console.WriteLine($"[{DateTime.Now}][ERROR]: Can't configure log. Reason: tty must be in range [1;12]. Current: {tty}");
                        }
                    }
                }
                else throw new Exception("This OS is unsupported for now. Sorry :)");
                LogPath = logPath;
                // Проверяем наличие директории и создаем в ней лог
                if (!new DirectoryInfo(logPath).Exists)
                {
                    try
                    {
                        Directory.CreateDirectory(logPath);
                        Logger.Log(Logger.LogLevel.INFO, "Log successfilly set");
                    }
                    catch (Exception directoryException)
                    {
                        throw new Exception($"Failed to create log directory '{logPath}'.\nReason:\n{directoryException.Message}");
                    }
                }
            }
        }
    }
}
