using System.Runtime.InteropServices;

namespace AutoscanBot
{
    internal class Storage
    {
        public static string? LogPath { get; set; }
        public static bool SetLogPath()
        {
            string? logsPath = Configuration.GetItemValueByName("WORKING_DIRECTORY");
            if (logsPath == null) return false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                logsPath += "\\LOGS\\";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                logsPath += "/LOGS/";
            }
            else
            {
                Logger.Log(Logger.LogLevel.CRITICAL, "Unsupported platform detected. This app can be run only on Windows and Linux hosts");
                return false;
            }
            string? logFSEnabled = Configuration.GetItemValueByName("LOGS_ENABLE_FS_WRITER");
            if (logFSEnabled != null)
            {
                if (logFSEnabled.Trim().ToLower() == "true")
                {
                    if (!new DirectoryInfo(logsPath).Exists)
                    {
                        try
                        {
                            Logger.Log(Logger.LogLevel.INFO, $"Log directory '{logsPath}' missing. Creating it");
                            Directory.CreateDirectory($"{logsPath}");
                            Logger.Log(Logger.LogLevel.SUCCESS, $"Created log directory in '{logsPath}'");
                        }
                        catch (Exception directoryException)
                        {
                            Logger.Log(Logger.LogLevel.ERROR, $"Failed to create log directory '{logsPath}'.\nReason:\n{directoryException.Message}" +
                                "\nSwitching 'LOGS_ENABLE_FS_WRITER' to 'false'");
                            Configuration.SwitchLogType(false);
                        }
                    }
                }
            }
            LogPath = $"{logsPath}latest.log";
            return true;
        }
    }
}
