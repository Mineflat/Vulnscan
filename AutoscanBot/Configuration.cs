namespace AutoscanBot
{
    internal class Configuration
    {
        /// <summary>
        /// Проверяет и значения из конфигурационного файла
        /// </summary>
        /// <param name="path">Полный путь до файла конфигурации</param>
        /// <returns>True, если путь до файла существует и конфиг был установлен, в остальных случаях - False</returns>
        public static List<ConfigurationItem>? TryRead(string? path)
        {
            if (File.Exists(path))
            {
                string[] pureConfig = File.ReadAllLines(path);
                List<ConfigurationItem> configurationItems = new List<ConfigurationItem>();
                foreach (string pureConfigItem in pureConfig)
                {
                    ConfigurationItem item = new ConfigurationItem();
                    item.Name = pureConfigItem;
                }

                return configurationItems;
            }
            return null;
        }
    }
}
