using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DemoBot.Commands
{
    public class HelpCommand : Command
    {
        public override string Name => "/help";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            string msg = "" +
                "/help - send a list of available commands\n" +
                "/developer - send information about developer\n" +
                "/inline - send inline keyboard\n" +
                "/keyboard - send custom keyboard\n" +
                "/location - send your location\n" +
                "/contact - send your contact";

            await client.SendTextMessageAsync(
                chatId: message.Chat,
                text: msg
            );
        }
    }
}
