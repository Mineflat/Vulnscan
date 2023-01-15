/*
    Плохое решение, не делайте так
 */
using System.Linq;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;

namespace AutoscanBot.Telegramm
{
    internal class Bot
    {
        private static BotClient? TelegrammBotClient;
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
                if (TryLaunch(token).Result) return true;
            }
            return false;
        }
        private static async Task<bool> TryLaunch(string? token)
        {
            if (token == null || token?.Length == 0) return false;
            SetIgnoreExceptions(token);
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
            catch
            {
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
                                    Logger.Log(Logger.LogLevel.INFO, $"{update.Message?.Contact?.FirstName} {update.Message?.Contact?.LastName} ({update.Message?.Contact?.UserId}): ");
                                    Logger.Log(Logger.LogLevel.MESSAGE, update.Message?.Text);
                                }
                                else
                                {
                                    Logger.Log(Logger.LogLevel.INFO,
                                        $"{update.Message?.Contact?.FirstName} {update.Message?.Contact?.LastName} ({update.Message?.Contact?.UserId}): Received {update.Type}");
                                }
                                string? replyMessage = ProcessMeggase(update.Message?.Text);
                                if (replyMessage != null)
                                {
                                    await TelegrammBotClient.SendMessageAsync(chatId, replyMessage); // Send a message
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
        private static string? ProcessMeggase(string? message)
        {
            message = message?.Trim(); // если в message лежит null, то ошибки не должно произойти из-за `?`
            string reply = string.Empty;
            switch (message)
            {
                default:
                    reply = "I don't understand you"; // Сюда бы рандомных ответов добавить... 
                    break;
            }
            return reply;
        }

        private static void SetIgnoreExceptions(string? token)
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
    }
}
