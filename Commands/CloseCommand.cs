using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemoBot.Commands
{
    public class CloseCommand : Command
    {
        public override string Name => "/close";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Removing keyboard...",
                replyMarkup: new ReplyKeyboardRemove()
            );
        }
    }
}
