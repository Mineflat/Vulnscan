using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot.Telegramm
{
    internal class Bot
    {
        public static void Start()
        {
            #region Setup
            bool isSetupOK = Setup(Configuration.Preset?.Where(i => i.Name == "bot_token").FirstOrDefault()?.Content?.Trim('\0', ' '));
            if (!isSetupOK)
            {
                Logger.Log(Logger.LogLevel.ERROR, "BOT_TOKEN is empty, so can't start service");
                return;
            }
            Logger.Log(Logger.LogLevel.SUCCESS, "Setup is OK, bot started successfully");
            #endregion
            
            #region Launch

            #endregion
        }
        private static bool Setup(string? token)
        {
            if (token?.Length == 0) return false;
            Logger.Log(Logger.LogLevel.INFO, "BOT_TOKEN detected, trying to start it");
            // Telegramm bot setup
            return true;
        }
    }
}
