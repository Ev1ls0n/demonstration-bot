using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using DemoBot.Commands;

namespace DemoBot.Services
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdateService> _logger;
        private readonly List<Command> _commandsList;

        public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger)
        {
            _botClient = botClient;
            _logger = logger;

            // Инициализация списка команд
            _commandsList = new List<Command>
            {
                new StartCommand(),
                new HelpCommand(),
                new DeveloperCommand(),
                new InlineCommand(),
                new KeyboardCommand(),
                new CloseCommand(),
                new LocationCommand(),
                new ContactCommand()
            };
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        // Метод для обработки текстового сообщения, котрое отправил пользователь
        private async Task BotOnMessageReceived(Message message)
        {
            _logger.LogInformation($"(i) Receive message type: {message.Type}");
            _logger.LogInformation($"(i) Receive message from: {message.Chat.Id}");

            if (message.Type != MessageType.Text)
                return;

            foreach (var command in _commandsList)
            {
                if(command.Contains(message.Text, _botClient.GetMeAsync().Result.Username))
                {
                    await command.Execute(message, _botClient as TelegramBotClient);
                    break;
                }
            }
        }

        // Метод для обработки данных обратного вызова. Например событий кнопок (keyboard)
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            // Вывод специального всплывающего окна с сообщением, которое будет содержать данные, отправляемые обратным вызовом
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"ℹ️ Received: {callbackQuery.Data}"
            );

            /*
            // Вывод в чат сообщения с данными, которые отправляет обратный вызов
            await Bot.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"ℹ️ Received: {callbackQuery.Data}"
            );

            // Удаление из чата сообщения
            await Bot.DeleteMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId
            );
            */

            // Закрытие опроса, который отправил бот
            //if (callbackQuery.Data == "close_poll")
            //{
            //    await _botClient.StopPollAsync(
            //        chatId: callbackQuery.Message.Chat,
            //        messageId: callbackQuery.Message.MessageId
            //    );
            //}

            // Удаление сообщения, которое отправил бот
            if (callbackQuery.Data == "delete_msg")
            {
                // Если сообщение находится в личной переписке, то бот не может его удалить
                // У личной переписки свойство Title равно null
                if (callbackQuery.Message.Chat.Title == null)
                {
                    await _botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        text: "⚠️ Bot can't delete messages in private chat."
                    );
                }
                else
                {
                    await _botClient.DeleteMessageAsync(
                        chatId: callbackQuery.Message.Chat,
                        messageId: callbackQuery.Message.MessageId
                    );
                }
            }

        }

        // Метод, который вызывается в случае, если тип обновления от пользователя не подходит не под один из обрабатываемых типов
        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogWarning($"(!) Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        // Метод для сообщения об ошибке в Telegram API
        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"(!) Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
