namespace AutoscanBot
{
    internal class Configuration
    {
        private static string? WorkingDirectory;
        private static long TelegramBotToken;
        /// <summary>
        /// Проверяет и значения из конфигурационного файла
        /// </summary>
        /// <param name="path">Полный путь до файла конфигурации</param>
        /// <returns>True, если путь до файла существует и конфиг был установлен, в остальных случаях - False</returns>
        public static bool TryRead(string? path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (new FileInfo(path).Exists)
                {
                    //ToDo: Сделать так чтобы эта хуйня что-то обновляла
                    return true;
                }
            }
            return false;
        }
    }
}
