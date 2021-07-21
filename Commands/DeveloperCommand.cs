using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DemoBot.Commands
{
    public class DeveloperCommand : Command
    {
        public override string Name => "/developer";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            string msg = "" +
                "• <b>Bot developer:</b> [name]\n" +
                "• <b>Telegram:</b> @[username]\n" +
                "• <b>Discord:</b> [username]#[id]";

            await client.SendTextMessageAsync(
                chatId: message.Chat,
                text: msg,
                parseMode: ParseMode.Html
            );
        }
    }
}
