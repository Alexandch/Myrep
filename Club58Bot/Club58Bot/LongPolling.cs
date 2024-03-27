using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Club58Bot;

public class LongPolling(ITelegramBotClient client)
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient ,Update update, CancellationToken cancellationToken)
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

                await client.SendTextMessageAsync(
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
                await client.SendTextMessageAsync(message.Chat, $"События: {eventNames}", cancellationToken: cancellationToken);
                return;
            }

            await client.SendTextMessageAsync(message.Chat, "Привет-привет!!", cancellationToken: cancellationToken);
        }
        else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            var callbackQuery = update.CallbackQuery;

            if (callbackQuery!.Data == "show_events")
            {
                var service = new MockClubEventService();
                var events = service.GetEvents();
                var eventNames = string.Join(", ", events.Select(e => e.Name));
                await client.SendTextMessageAsync(callbackQuery.Message!.Chat, $"События: {eventNames}", cancellationToken: cancellationToken);
            }
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        return Task.CompletedTask;
    }
}