using BusinesDAL;
using BusinesDAL.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace UlguBotConsole.Services
{
    public class BotHostedService : IHostedService
    {
        private readonly ICommandService _cmdService;
        private readonly ILogger<BotHostedService> _logger;
        public BotHostedService(ILogger<BotHostedService> logger, ICommandService cmdService)
        {
            _logger = logger;
            _cmdService = cmdService;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var botClient = new TelegramBotClient("5970266602:AAE8KptXWJwi_pUzCfSNSKxbpGWLoi_Z5lk");

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();
            _logger.LogInformation($"Start listening for @{me.Username}");
            while(true){}
            cts.Cancel();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Message is not { } message)
            {
                return;
            }
            if (message.Text is not { } messageText)
            {
                return;
            }
            var chatId = message.Chat.Id;
            System.Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            ExecuteResult executeResult = await _cmdService.ExecuteCommandAsync(messageText);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: executeResult.Message,
                replyMarkup: executeResult.replyMarkup,
                cancellationToken: token
            );
        }
        Task HandlePollingAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            System.Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}