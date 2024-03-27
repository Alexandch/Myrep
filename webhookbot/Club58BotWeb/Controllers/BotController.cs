using Club58BotWeb.Filters;
using Club58BotWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Club58BotWeb.Controllers;

public class BotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}