using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Club58Bot;

public abstract class Program
{
    private static readonly ITelegramBotClient Bot = new TelegramBotClient("6924859504:AAEyep5De2g_IZ3mhOJ4Buh03XfB2DcPkJg");

    private static void Main()
    {
        Console.WriteLine("Запущен бот " + Bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            //AllowedUpdates = { }, // receive all update types
        };
        var longPolling = new LongPolling(Bot);
        Bot.StartReceiving(
            longPolling.HandleUpdateAsync,
            longPolling.HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }
}