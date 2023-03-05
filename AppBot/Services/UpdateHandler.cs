using app.common.DTO;
using BusinesDAL.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace AppBot.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly IBusinessBotService _businessBotService;

        public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, IBusinessBotService botService)
        {
            _botClient = botClient;
            _logger = logger;
            _businessBotService = botService;
        }
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            // Cooldown in case of network connection error
            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
                { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)

            };
            await handler;
        }

        private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown update type:", update.Type);
            return Task.CompletedTask;
        }
        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } messageText)
                return;
            MessageDTO messageDTO = await _businessBotService.UserChoose(messageText, message.Chat.Id);
            Message action = await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: messageDTO.Message,
                replyMarkup: messageDTO.KeyboardMarkup,
                cancellationToken: cancellationToken
            );
            _logger.LogInformation("The message was sent with id: {SentMesageId}", action.MessageId);
        }
    }


}