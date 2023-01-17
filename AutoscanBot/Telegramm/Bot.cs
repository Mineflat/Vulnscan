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
        public static void Start()
        {
            if (Setup(Configuration.GetItemValueByName("bot_token")))
                Logger.Log(Logger.LogLevel.SUCCESS, "Setup is OK, bot started successfully");
            else
                Logger.Log(Logger.LogLevel.ERROR, Properties.Resources.BadOrMissingTelegrammBotToken);
        }
        private static bool Setup(string? token)
        {
            if (token != null || token?.Length >= 0)
            {
                Logger.Log(Logger.LogLevel.INFO, "BOT_TOKEN detected, trying to start it");
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
                    Logger.Log(Logger.LogLevel.INFO, $"Started bot @{username}");
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
                Logger.Log(Logger.LogLevel.ERROR, BotLaunchException.Message);
                return false;
            }
            return true;
        }
        private static async void StartReceiving()
        {
            if (TelegrammBotClient == null)
            {
                Logger.Log(Logger.LogLevel.ERROR, Properties.Resources.BadOrMissingTelegrammBotToken);
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
                            if (update.Message != null)
                            {
                                long chatId = update.Message.Chat.Id; // Target chat Id
                                if (update.Type == UpdateType.Message && update.Message?.Text?.Length > 0)
                                {
                                    Logger.Log(Logger.LogLevel.INFO, 
                                        $"{update.Message?.From?.FirstName} {update.Message?.From?.LastName} ({update.Message?.From?.Username}): ");
                                    Logger.Log(Logger.LogLevel.MESSAGE, update.Message?.Text);
                                }
                                else
                                {
                                    Logger.Log(Logger.LogLevel.INFO,
                                        $"{update.Message?.From?.FirstName} {update.Message?.From?.LastName} ({update.Message?.From?.Username}): Received message of type `{update.Type}`");
                                }
                                // ToDo: Сделать для каждой командый свой тип ParseMode.
                                /*
                                 * 1. Возвращаем CommandReply
                                 * 2. Берем оттуда поле ParseMode
                                 * 3. Вставляем поле в соответствующее место
                                 */
                                string? replyMessage = ProcessMessage(update.Message?.Text);
                                if (replyMessage != null)
                                {
                                    await TelegrammBotClient.SendMessageAsync(chatId, replyMessage, parseMode: ParseMode.MarkdownV2 ); // вот сюда вставляем ParseMode
                                }
                            }
                        }
                        var offset = updates.Last().UpdateId + 1;
                        updates = TelegrammBotClient.GetUpdates(offset);
                    }
                    else
                    {
                        updates = TelegrammBotClient.GetUpdates();
                    }
                    Thread.Sleep(10);
                }
            }
        }
        private static string? ProcessMessage(string? message)
        {
            message = message?.Trim()?.ToLower();

            foreach (Command command in Command.AvailibleCommands)
            {
                if(command.InvokeName.ToLower() == message)
                {
                    if(command.LinkedTo != null)
                    {
                        CommandExecutionResult result = command.LinkedTo.Invoke();
                        if(result != null && result.Success) 
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
                    Logger.Log(Logger.LogLevel.INFO, $"Telegramm bot ignore exceptions set to TRUE");
                }
                else
                {
                    TelegrammBotClient = new BotClient(token, false);
                    Logger.Log(Logger.LogLevel.INFO, $"Telegramm bot ignore exceptions set to FALSE");
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
