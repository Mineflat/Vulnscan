using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot.Telegramm
{
    internal class PresetStorage
    {
        public static bool Load()
        {
            string? rootPath = Configuration.GetItemValueByName("WORKING_DIRECTORY");
            if (rootPath == null) throw new Exception("CRITICAL ERROR: Config has empty parameter 'WORKING_DIRECTORY'. Write it and relaunch app");
            if (!new DirectoryInfo(rootPath).Exists) throw new Exception($"CRITICAL ERROR: Directory {rootPath} doesn't exist. Is 'WORKING_DIRECTORY' in config valud value?");
            if (OperatingSystem.IsLinux())
            {
                DirectoryInfo directoryInfo = new DirectoryInfo($"{rootPath}/CORE/Scripts/");
                if (!directoryInfo.Exists)
                {
                    Directory.CreateDirectory($"{rootPath}/CORE/Scripts/");
                }
                Configuration.Log?.Invoke(Logger.LogLevel.INFO, "Cleaning modules...");
                foreach (var item in directoryInfo.EnumerateFiles())
                {
                    item.Delete();
                }
                File.WriteAllText($"{rootPath}/CORE/Scripts/", echoTTY);
            }
            else
            if (OperatingSystem.IsWindows())
            {
                DirectoryInfo directoryInfo = new DirectoryInfo($"{rootPath}\\CORE\\Scripts\\");
                if (!directoryInfo.Exists)
                {
                    Directory.CreateDirectory($"{rootPath}\\CORE\\Scripts\\");
                }
                Configuration.Log?.Invoke(Logger.LogLevel.INFO, "Cleaning modules...");
                foreach (var item in directoryInfo.EnumerateFiles())
                {
                    item.Delete();
                }
                File.WriteAllText($"{rootPath}\\CORE\\Scripts\\", echoTTY);
            }
            else throw new Exception("Sorry, this OS is unsupported for now :&");
            return true;
        }


        #region Shebangs
        public const string bash_Shebang = "#!/bin/bash";
        #endregion
        #region Scripts
        // echoTTY.sh - принимает на вход 2 параметра:
        // $1 - то, что будет перед сообщением лога
        // $2 - то, что будет после сообщения лога
        // $3 - номер tty, куда отправится лог
        public const string echoTTY = bash_Shebang +
            @"
echo -en ""$1"" >> /dev/tty$3
echo -E $2 >> /dev/tty$3";
        #endregion
    }
}
