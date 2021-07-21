using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemoBot.Commands
{
    public class KeyboardCommand : Command
    {
        public override string Name => "/keyboard";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                    new KeyboardButton[] { "🎲", "🎯" },
                    new KeyboardButton[] { "⚽️", "🎳" },
                    new KeyboardButton[] { "/close" }
                },
                resizeKeyboard: true,
                oneTimeKeyboard: true
            );

            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose emoji for play 🎮",
                replyMarkup: replyKeyboardMarkup
            );
        }
    }
}
