/*
    Плохое решение, не делайте так
 */
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableMethods.FormattingOptions;
using Telegram.BotAPI.GettingUpdates;

namespace AutoscanBot.Telegramm
{
    internal class Bot
    {
        private static BotClient? TelegrammBotClient;
        private static readonly Random getrandom = new Random(); // Это нужно для стабильного рандома. Пока что это скорее костыль
        private static string BotName { get; set; } = Configuration.GetItemValueByName("BOT_INVOKE_NAME");
        public static void Start()
        {
            if (Setup(Configuration.GetItemValueByName("bot_token")))
                Configuration.Log.Invoke(Logger.LogLevel.SUCCESS, "Setup is OK, bot started successfully");
            else
                Configuration.Log.Invoke(Logger.LogLevel.ERROR, Properties.Resources.BadOrMissingTelegrammBotToken);
        }
        private static bool Setup(string? token)
        {
            if (token != null || token?.Length >= 0)
            {
                Configuration.Log.Invoke(Logger.LogLevel.INFO, "BOT_TOKEN detected, trying to start it");
                if (TryLaunch(token)) return true;
            }
            return false;
        }
        private static bool TryLaunch(string? token)
        {
            if (token == null || token?.Length == 0) return false;
            CreateBotInstance(token);
            try
            {
                if (TelegrammBotClient != null)
                {
                    string? username = TelegrammBotClient?.GetMeAsync().Result.Username; // Получаем его юзернейм
                    Configuration.Log.Invoke(Logger.LogLevel.INFO, $"Started bot @{username}");
                    Thread UpdateThread = new Thread(() =>
                    {
                        StartReceiving();
                    });
                    UpdateThread.IsBackground = true;
                    UpdateThread.Start();
                }
            }
            catch (Exception BotLaunchException)
            {
                Configuration.Log.Invoke(Logger.LogLevel.ERROR, BotLaunchException.Message);
                return false;
            }
            return true;
        }
        private static async void StartReceiving()
        {
            if (TelegrammBotClient == null)
            {
                Configuration.Log.Invoke(Logger.LogLevel.ERROR, Properties.Resources.BadOrMissingTelegrammBotToken);
            }
            else
            {
                var updates = TelegrammBotClient.GetUpdates();
                while (true)
                {
                    if (updates.Any())
                    {
                        foreach (var update in updates)
                        {
                            // Process update
                            if (update.Message?.Text == null) continue;
                            if (!VerifiBorAppearal(update.Message.Text)) continue; // проверяем, что обратились именно к боту
                            string? messageText = GetCleanCommand(update.Message.Text);
                            if (messageText == null) continue;

                            long chatId = update.Message.Chat.Id;

                            Configuration.Log.Invoke(Logger.LogLevel.MESSAGE,
                                $"[{update.Type}] {update.Message?.From?.FirstName} {update.Message?.From?.LastName} ({update.Message?.From?.Username}): {update.Message?.Text}");

                            // ToDo: Сделать для каждой командый свой тип ParseMode.
                            /*
                             * 1. Возвращаем CommandReply
                             * 2. Берем оттуда поле ParseMode
                             * 3. Вставляем поле в соответствующее место
                             */
                            string? replyMessage = ProcessMessage(messageText);
                            if (replyMessage != null)
                            {
                                // await TelegrammBotClient.SendMessageAsync(chatId, replyMessage, parseMode: ParseMode.MarkdownV2); // вот сюда вставляем ParseMode
                                await TelegrammBotClient.SendMessageAsync(chatId, replyMessage); // вот сюда вставляем ParseMode
                            }
                        }
                        var offset = updates.Last().UpdateId + 1;
                        updates = TelegrammBotClient.GetUpdates(offset);
                    }
                    else
                    {
                        updates = TelegrammBotClient.GetUpdates();
                    }
                    Thread.Sleep(100);
                }
            }
        }
        private static string? GetCleanCommand(string message)
        {
            if (message.Length == 0) return null;
            return message.Replace(Configuration.GetItemValueByName("BOT_INVOKE_NAME"), "").Trim();
        }
        private static bool VerifiBorAppearal(string message)
        {
            string botInvokeName = Configuration.GetItemValueByName("BOT_INVOKE_NAME");
            message = message.Trim();
            string[] buffer = message.Split(' ');
            if (buffer.Length >= 0)
            {
                if (buffer[0].ToLower().Contains(botInvokeName.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
        private static string? ProcessMessage(string? message)
        {
            message = message?.Trim()?.ToLower();

            foreach (Command command in Command.AvailibleCommands)
            {
                if (command.InvokeName.ToLower() == message)
                {
                    if (command.LinkedTo != null)
                    {
                        CommandExecutionResult result = command.LinkedTo.Invoke();
                        if (result != null && result.Success)
                        {
                            return result.Message;
                        }
                    }
                    else
                    {
                        return "Эта команда, конечно, существует, но мой создатель пока ее не реализовал. Пните этого человека чтобы получить больше информаци";
                    }
                }
            }
            return GetRandomConfuseReply();
        }
        private static void CreateBotInstance(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;
            string? ignoreBotExceptions = Configuration.GetItemValueByName("BOT_IGNORE_EXCEPTIONS")?.ToLower();
            if (ignoreBotExceptions != null)
            {
                if (ignoreBotExceptions == "true")
                {
                    TelegrammBotClient = new BotClient(token, true);
                    Configuration.Log.Invoke(Logger.LogLevel.INFO, $"Telegramm bot ignore exceptions set to TRUE");
                }
                else
                {
                    TelegrammBotClient = new BotClient(token, false);
                    Configuration.Log.Invoke(Logger.LogLevel.INFO, $"Telegramm bot ignore exceptions set to FALSE");
                }
            }
        }
        private static string GetRandomConfuseReply()
        {
            lock (getrandom) // Это финт ушами для синхронизации. Не обращайте внимания...
            {
                return Configuration.ConfuseReplyPresets[getrandom.Next(0, Configuration.ConfuseReplyPresets.Count - 1)];
            }
        }
    }
}
