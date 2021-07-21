using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace DemoBot.Commands
{
    public class InlineCommand : Command
    {
        public override string Name => "/inline";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            await client.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                // Первая строка
                new []
                {
                    InlineKeyboardButton.WithCallbackData("❤️", "You choose ❤️"),
                    InlineKeyboardButton.WithCallbackData("🍏", "You choose 🍏")
                },
                // Вторая строка
                new []
                {
                    InlineKeyboardButton.WithCallbackData("🐈", "You choose 🐈"),
                    InlineKeyboardButton.WithCallbackData("🎲", "You choose 🎲")
                },
            });

            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Click :)",
                replyMarkup: inlineKeyboard
            );
        }
    }
}
