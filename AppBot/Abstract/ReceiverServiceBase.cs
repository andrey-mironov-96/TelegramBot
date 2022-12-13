using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace AppBot.Abstract
{
    public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService
        where TUpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly TUpdateHandler _updateHandler;
        private readonly ILogger<ReceiverServiceBase<TUpdateHandler>> _logger;
        internal ReceiverServiceBase(
            ITelegramBotClient botClient,
            TUpdateHandler updateHandler,
            ILogger<ReceiverServiceBase<TUpdateHandler>> logger
        )
        {
            _botClient = botClient;
            _updateHandler = updateHandler;
            _logger = logger;
        }
        public async Task ReceiveAsync(CancellationToken stoppingToken)
        {
            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true
            };
            var me = await _botClient.GetMeAsync(stoppingToken);
            _logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My Awesome Bot");

            await _botClient.ReceiveAsync(
                updateHandler: _updateHandler,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken
            );
        }
    }
}