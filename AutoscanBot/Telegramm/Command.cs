using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot.Telegramm
{
    internal class Command
    {
        public string InvokeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public short Class { get; set; } = 0; // TERMINAL = 0, BOT = 1, COMMON = 2

        public delegate CommandExecutionResult LinkedMethod();
        public LinkedMethod? LinkedTo { get; set; }

        public static List<Command> AvailibleCommands = new List<Command>()
        {
            new Command
            {
                InvokeName="Help",
                Description = "Покажет сообщение со всеми доступными командами",
                LinkedTo = Invoke_Help,
                Class = 2
            }
        };


        #region Реализация команд
        private static CommandExecutionResult Invoke_Help()
        {
            string returnMessage = string.Empty;
            // Дальше пойдет код, за которой мне стыдно, но это лишь временный костыль, ведь уже почти час ночи...
            // Да, это дубляж кода :/
            List<Command> bot = AvailibleCommands.FindAll(x => x.Class == 1);
            if (bot.Count > 0)
            {
                returnMessage += "Команды, доступные боту:\n";
                for (int i = 0; i < bot.Count; i++)
                {
                    returnMessage += $"{DigitToSmile(i + 1)} {bot[i].InvokeName}:\n`{bot[i].Description}`\n";
                }
            }

            List<Command> common = AvailibleCommands.FindAll(x => x.Class == 2);
            if (common.Count > 0)
            {
                returnMessage += "Команды, доступные везде:\n";
                for (int i = 0; i < common.Count; i++)
                {
                    returnMessage += $"{DigitToSmile(i + 1)} {common[i].InvokeName}:\n`{common[i].Description}`\n";
                }
            }

            List<Command> terminal = AvailibleCommands.FindAll(x => x.Class == 0);
            if (terminal.Count > 0)
            {
                returnMessage += "Команды, доступные в терминале:\n";
                for (int i = 0; i < terminal.Count; i++)
                {
                    returnMessage += $"{DigitToSmile(i + 1)} {terminal[i].InvokeName}:\n`{terminal[i].Description}`\n";
                }
            }
            return new CommandExecutionResult(true, returnMessage);
        }

        private static string? DigitToSmile(int digit)
        {
            if (digit < 0) return digit.ToString();
            string[] smiles =
            {
                "0⃣", "1⃣", "2⃣", "3⃣", "4⃣",
                "5⃣", "6⃣", "7⃣", "8⃣", "9⃣"
            };
            string converted = digit.ToString();
            int maxDigit = converted.Select(c => int.Parse(c.ToString())).Max(); // Получаем наибольшую цифру в числе
            for (int i = 0; i <= maxDigit; i++)
            {
                converted = System.Text.RegularExpressions.Regex.Replace(converted, $"{i}", smiles[i], System.Text.RegularExpressions.RegexOptions.None);
            }
            return converted;
        }
        #endregion

    }
}
