namespace AutoscanBot
{
    internal class Configuration
    {

        public static List<ConfigurationItem>? Preset = new List<ConfigurationItem>();
        public static List<string> ConfuseReplyPresets = new List<string>()
        {
            "Я не совсем понял о чем ты говоришь", "Я тебя не понимаю",
            "Ты уверен что я такое умею?", "Что-то мне подсказывает что ты не до конца понимаешь, зачем я тут",
            "А можно сформулировать конкретнее?", "Я бот, а не человек. Я не понимаю о чем ты говоришь - используй команды",
            "Между человеком и ботом слишком большая разница. Используй команды если хочешь чтобы я тебя понял", "А можно командой?",
            "Формат команд мне нравится больше. Давай на \"командном\" языке", "И все же, командами мне нравится больше. Я их хотя бы понимаю..."
        };
       
        public static List<ConfigurationItem>? TryRead(string? path)
        {
            if (File.Exists(path))
            {
                string[] pureConfig = File.ReadAllLines(path); // Считываем весь конфиг построчно
                List<ConfigurationItem> configurationItems = new List<ConfigurationItem>(); // Определяем список из параметров конфигурации и их значений
                string[]? paramParts;
                char[] replaceChars = new char[] { ' ', '\0' };

                foreach (string pureConfigItem in pureConfig)
                {
                    if (pureConfigItem.Length == 0) continue; // чтобы не реагировал, если попалась пустая строка
                    if (pureConfigItem.Trim()[0] == '#') continue;

                    paramParts = pureConfigItem.Split('=', StringSplitOptions.TrimEntries);
                    // В записи a=b=c=...=X будет рассматриваться только первые 2 параметра, остальное будет отброшено
                    if (paramParts.Length >= 2)
                    {
                        paramParts[1] = paramParts[1].Replace('\"', (char)0).Trim();
                        if (paramParts[1].Contains('#'))
                        {
                            paramParts[1] = paramParts[1].Substring(0, paramParts[1].IndexOf('#') - 1);
                        }

                        ConfigurationItem item = new ConfigurationItem(paramParts[0].Trim(replaceChars).ToLower(), paramParts[1].Trim(replaceChars));
                        configurationItems.Add(item);
                        }
                    else
                    {
                        if (paramParts[0].Trim()[0] == '#') continue; // пропускаем комментарии, остальное парсим по возможности
                        Logger.Log(Logger.LogLevel.ERROR, $"Config: Bad string \'{pureConfigItem}\'. Expected usage is \'PARAMETER=VALUE\'");
                    }
                }
                if (configurationItems.Count > 0) return configurationItems;
            }
            return null;
        }
        //public static void SwitchLogType(bool enableFS)
        //{
        //    if (Preset == null) throw new Exception("CRITICAL ERROR: Invalid confuguration file which can't be parsed. Replace it and try to launch again");
        //    int replaceIndex = Preset.FindIndex(x => x.Name?.ToLower() == "logs_enable_fs_writer");
        //    if (replaceIndex == -1)
        //        throw new Exception("CRITICAL ERROR: Parameter 'LOGS_ENABLE_FS_WRITER' in confuguration file is invalid, missing or can't be parsed. Replace it and restart");
        //    if (enableFS)
        //    {
        //        Log = Logger.FSLog;
        //        Preset[replaceIndex].Content = "true";
        //        Log?.Invoke(Logger.LogLevel.SUCCESS, $"Log type set to FS logs ({Preset[replaceIndex].Name} => 'true'). Path: '{Storage.LogPath}'");
        //    }
        //    else
        //    {
        //        Preset[replaceIndex].Content = "false";
        //        Logger.Log(Logger.LogLevel.SUCCESS, $"Log type set to stdIO - terminal plain text used ({Preset[replaceIndex].Name} => 'false')");
        //    }
        //}
        public static string? GetItemValueByName(string configItemName)
        {
            if (string.IsNullOrEmpty(configItemName)) return null;
            string? result = Preset?.Where(i => i.Name?.ToLower() == configItemName.ToLower()).FirstOrDefault()?.Content?.Trim('\0', ' ');
            return result;
        }
    }
}
