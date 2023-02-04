using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI.AvailableMethods.FormattingOptions;

namespace AutoscanBot.Core
{
    internal class Command
    {
        public string InvokeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool EnableTerminalUsing = false;
        public bool EnableBotUsing = false;
        //        public dynamic TelegrammParseMode = ParseMode.MarkdownV2; // Это плохой код, не надо так делать. Я же - скоро исправлю
        public byte Type { get; set; } = 0; // ⚙=0 🤖=1 ⚙🤖=2 
        public short Class { get; set; } = 0; // TERMINAL = 0, BOT = 1, COMMON = 2

        public delegate CommandExecutionResult LinkedMethod();
        public LinkedMethod? LinkedTo { get; set; }

        public static List<Command> AvailibleCommands = new List<Command>()
        {
            new Command
            {
                InvokeName = "Show config",
                Description = "Покажет актуальную конфигурацию",
                LinkedTo = ShowConfig,
                EnableTerminalUsing = true,
                EnableBotUsing = true
            },
            new Command
            {
                InvokeName="Help",
                Description = "Покажет сообщение со всеми доступными командами",
                LinkedTo = Invoke_Help,
                EnableTerminalUsing=true,
                EnableBotUsing=true
            }
        };


        #region Реализация команд

        private static CommandExecutionResult ShowConfig()
        {
            string returnMessage = string.Empty;
            if (Configuration.Preset != null)
            {
                foreach (var item in Configuration.Preset)
                {
                    if (item.Name == "bot_token") continue;
                    returnMessage += $"`{item.Name}` => `{item.Content}`\n";
                }
                return new CommandExecutionResult(true, returnMessage);
            }
            return new CommandExecutionResult(true, "Конфигурационный файл пуст");

        }
        private static CommandExecutionResult Invoke_Help()
        {
            string returnMessage = "Вот доступные команды (⚙ - доступно боту, 🤖 - доступно администратору).\n";

            for (int i = 0; i < AvailibleCommands.Count; i++)
            {
                returnMessage += $"{DigitToSmile(i + 1)} ";

                if (AvailibleCommands[i].EnableTerminalUsing)
                    returnMessage += "⚙";
                if (AvailibleCommands[i].EnableBotUsing)
                    returnMessage += "🤖";

                returnMessage += $" `{AvailibleCommands[i].InvokeName}`:\n{AvailibleCommands[i].Description}\n";
            }
            returnMessage += $"\nНапомню, формат команды следующий:\n`{Configuration.GetItemValueByName("BOT_INVOKE_NAME")} <команда>`";

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
