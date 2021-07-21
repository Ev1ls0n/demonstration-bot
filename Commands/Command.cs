using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DemoBot.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract Task Execute(Message message, TelegramBotClient client);

        public bool Contains(string command, string botName)
        {
            if (!command.Contains(botName))
                return command.Contains(this.Name);
            else
                return command.Contains(this.Name) && command.Contains(botName);
        }
    }
}
