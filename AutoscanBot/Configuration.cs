namespace AutoscanBot
{
    internal class Configuration
    {
        /// <summary>
        /// Читает значения из конфигурационного файла
        /// </summary>
        /// <param name="path">Полный путь до файла конфигурации</param>
        /// <returns>Возвращает неструктурированный список параметров и значений из конфига. Может вернуть Null, если список пуст</returns>
        public static List<ConfigurationItem>? TryRead(string? path)
        {
            if (File.Exists(path))
            {
                string[] pureConfig = File.ReadAllLines(path); // Считываем весь конфиг построчно
                List<ConfigurationItem> configurationItems = new List<ConfigurationItem>(); // Определяем список из параметров конфигурации и их значений
                string[]? paramParts;
                foreach (string pureConfigItem in pureConfig)
                {
                    paramParts = pureConfigItem.Split('=', StringSplitOptions.TrimEntries);
                    // В записи a=b=c=...=X будет рассматриваться только 1 и 2 параметр (a, b)
                    if (paramParts[0].Length > 0) // чтобы не реагировал, если попалась пустая строка
                    {
                        if (paramParts.Length >= 2)
                        {
                            ConfigurationItem item = new ConfigurationItem(paramParts[0], paramParts[1]);
                            configurationItems.Add(item);
                            Logger.Log(Logger.LogLevel.INFO, $"Config: \'{paramParts[0]}\' => \'{paramParts[1]}\'");
                        }
                        else
                        {
                            Logger.Log(Logger.LogLevel.ERROR, $"Config: Bad string \'{pureConfigItem}\'. Expected usage is \'PARAMETER=VALUE\'");
                        }
                    }
                }
                if (configurationItems.Count > 0) return configurationItems;
            }
            return null;
        }
    }
}
