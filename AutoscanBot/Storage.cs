using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot
{
    internal class Storage
    {
        private static string? ConfigurationPath;
        private static long TelegramBotToken;

        /// <summary>
        /// Проверяет и устанавливает путь до конфигурационного файла
        /// </summary>
        /// <param name="path">Полный путь до файла конфигурации</param>
        /// <returns>True, если путь до конфига существует и был установлен, в остальных случаях - False</returns>
        public static bool TrySet_ConfigurationPath(string? path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (new FileInfo(path).Exists)
                {
                    ConfigurationPath = path;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Устанавливает токен Телеграмм-бота из входящей строки
        /// </summary>
        /// <param name="token">Токен Телеграмм-бота</param>
        /// <returns>True, если токен установлен, в остальных случаях - False</returns>
        public static bool TrySet_BotToken(string? token)
        {
            if (long.TryParse(token, out long parsedToken))
            {
                TelegramBotToken = parsedToken;
                return true;
            }
            return false;
        }
    }
}
