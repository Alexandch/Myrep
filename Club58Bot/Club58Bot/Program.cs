using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Club58Bot;

public static class Program
{
    private static readonly ITelegramBotClient Bot = new TelegramBotClient("6924859504:AAEyep5De2g_IZ3mhOJ4Buh03XfB2DcPkJg");

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            if (message!.Text!.ToLower() == "/start")
            {
                var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[] // first row
                    {
                        InlineKeyboardButton.WithCallbackData("Показать ивенты", "show_events"),
                    }
                });

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Добро пожаловать на борт, добрый путник!",
                    replyMarkup: keyboard, cancellationToken: cancellationToken);
                return;
            }

            if (message.Text.ToLower() == "/events")
            {
                var service = new MockClubEventService();
                var events = service.GetEvents();
                var eventNames = string.Join(", ", events.Select(e => e.Name));
                await botClient.SendTextMessageAsync(message.Chat, $"События: {eventNames}", cancellationToken: cancellationToken);
                return;
            }

            await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!", cancellationToken: cancellationToken);
        }
        else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            var callbackQuery = update.CallbackQuery;

            if (callbackQuery!.Data == "show_events")
            {
                var service = new MockClubEventService();
                var events = service.GetEvents();
                var eventNames = string.Join(", ", events.Select(e => e.Name));
                await botClient.SendTextMessageAsync(callbackQuery.Message!.Chat, $"События: {eventNames}", cancellationToken: cancellationToken);
            }
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        return Task.CompletedTask;
    }

    static void Main()
    {
        Console.WriteLine("Запущен бот " + Bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            //AllowedUpdates = { }, // receive all update types
        };
        Bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }
}
