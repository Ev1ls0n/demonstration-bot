using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemoBot.Commands
{
    public class LocationCommand : Command
    {
        public override string Name => "/location";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            try
            {
                ReplyKeyboardMarkup requestReplyKeuboard = new ReplyKeyboardMarkup(
                    new[]
                    {
                        KeyboardButton.WithRequestLocation("🗺 Location"),
                        new KeyboardButton("/close")
                    },
                    resizeKeyboard: true,
                    oneTimeKeyboard: true
                );

                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Please enable location tracking on your device so that the app can detect it.",
                    replyMarkup: requestReplyKeuboard
                );
            }
            catch (ApiRequestException)
            {
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Location can be requested in private chats only.",
                    replyToMessageId: message.MessageId
                );
            }
        }
    }
}
