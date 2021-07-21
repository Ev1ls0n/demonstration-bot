using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DemoBot.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "/start";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(
                chatId: message.Chat,
                text: $"Hello @{message.Chat.Username}! Send me /help to find out what I can."
            );
        }
    }
}
