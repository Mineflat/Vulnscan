using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace AutoscanBot
{
    internal class Storage
    {
        public static string? LogPath { get; set; }
        public static void ConfigureLog()
        {
            string? workdir = Configuration.GetItemValueByName("WORKING_DIRECTORY");
            if (!Directory.Exists(workdir)) throw new Exception("Configuration error: param 'WORKING_DIRECTORY' is missing or directory missing");
            /*
              public static void ConfigureLog()
        {
            string? LogFS_Preset = GetItemValueByName("LOGS_ENABLE_FS_WRITER");
            if (LogFS_Preset != null)
            {
                if (LogFS_Preset.ToLower().Contains("true"))
                {
                    Log = Logger.FSLog;
                }
            }
            if (int.TryParse(GetItemValueByName("LOGS_ENABLE_FS_WRITER"), out int tty))
            {
                Logger.ttyNum = tty;
                Log = Logger.LogTTY;
            }
        }
             */

            bool logFSEnabled = Configuration.GetItemValueByName("LOGS_ENABLE_FS_WRITER")?.Trim().ToLower() == "true";
            if (logFSEnabled)
            {
                string logPath = string.Empty;
                if (OperatingSystem.IsWindows())
                {
                    logPath = $".\\{workdir}\\LOGS";
                }
                else
                if (OperatingSystem.IsLinux())
                {
                    logPath = $"./{workdir}/LOGS";
                    if(int.TryParse(Configuration.GetItemValueByName("LOGS_TTY_NUMBER"), out int tty))
                    {
                        Logger.ttyNum = tty;
                        Configuration.Log = Logger.FSLog;
                    }
                }
                else throw new Exception("This OS is unsupported for now. Sorry :)");

                if (!new DirectoryInfo(logPath).Exists)
                {
                    try
                    {
                        Directory.CreateDirectory(logPath);
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
