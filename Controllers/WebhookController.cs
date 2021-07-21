using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using DemoBot.Services;

namespace DemoBot.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices]HandleUpdateService handleUpdateService, [FromBody]Update update)
        {
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
