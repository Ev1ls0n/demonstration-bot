using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemoBot.Commands
{
    public class ContactCommand : Command
    {
        public override string Name => "/contact";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            try
            {
                ReplyKeyboardMarkup requestReplyKeuboard = new ReplyKeyboardMarkup(
                    new[]
                    {
                        KeyboardButton.WithRequestContact("👤 Contact"),
                        new KeyboardButton("/close")
                    },
                    resizeKeyboard: true,
                    oneTimeKeyboard: true
                );

                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Click on \"Contact\" button to get your contact",
                    replyMarkup: requestReplyKeuboard
                );
            }
            catch (ApiRequestException)
            {
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Phone number can be requested in private chats only.",
                    replyToMessageId: message.MessageId
                );
            }
        }
    }
}
