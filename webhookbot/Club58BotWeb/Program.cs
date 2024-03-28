using Club58BotWeb;
using Club58BotWeb.Controllers;
using Club58BotWeb.Services;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Setup Bot configuration
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration botConfig = sp.GetConfiguration<BotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

// Dummy business-logic service
builder.Services.AddScoped<UpdateHandlers>();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();
// Construct webhook route from the Route configuration parameter
// It is expected that BotController has single method accepting Update
if (botConfiguration != null) app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
app.MapControllers();
app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable RCS1110 // Declare type inside namespace.
namespace Club58BotWeb
{
    public class BotConfiguration
#pragma warning restore RCS1110 // Declare type inside namespace.
#pragma warning restore CA1050 // Declare types in namespaces
    {
        public static readonly string Configuration = "BotConfiguration";

        public string BotToken { get; init; } = default!;
        public string HostAddress { get; init; } = default!;
        public string Route { get; init; } = default!;
        public string SecretToken { get; init; } = default!;
    }
}