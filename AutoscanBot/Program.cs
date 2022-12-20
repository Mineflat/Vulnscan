namespace AutoscanBot
{
    internal class Program
    {
        /// <summary>
        /// Осуществляет проверку аргументов, переданных приложению на старте. Возвращает True, если проверка пройдена успешно. 
        /// </summary>
        /// <param name="args">Аргументы, передаваемые приложению на старте</param>
        /// <returns>True, если аргументы валидны, в остальных случаях - False </returns>
        private static bool CheckArgs(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine(Properties.Resources.BadUsageMessage.Replace("\\n", Environment.NewLine));
                return false;
            }
            if (!Storage.TrySet_ConfigurationPath(args[0]))
            {
                Console.WriteLine($"File '{args[0]}' doesn't exist. Create and fill it before starting");
                return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            if (!CheckArgs(args)) return;
            Console.Title = "Vulnscan";
            Console.Clear();


        }
    }
}