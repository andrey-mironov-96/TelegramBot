using BusinesDAL.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace AppBot.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly IAdmissionPlanService _admissionPlanService;
        public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, IAdmissionPlanService admissionPlanService)
        {
            _botClient = botClient;
            _logger = logger;
            _admissionPlanService = admissionPlanService;
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
                { CallbackQuery: { } callbackQuery } => BotOnCallbackQuery(callbackQuery, cancellationToken),
                { InlineQuery: { } inlineQuery } => BotOnInlineQuery(inlineQuery, cancellationToken),
                { ChosenInlineResult: { } chosenInlineResult } => BotOnChoisenInlineResultReceived(chosenInlineResult, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)

            };
            await handler;

        }

        private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown update type:", update.Type);
            return Task.CompletedTask;
        }

        private async Task BotOnChoisenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);

            await _botClient.SendTextMessageAsync(
                chatId: chosenInlineResult.From.Id,
                text: $"You chose result with Id: {chosenInlineResult.ResultId}",
                cancellationToken: cancellationToken);
        }

        private async Task BotOnInlineQuery(InlineQuery inlineQuery, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);
            InlineQueryResult[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "1",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent("hello"))
        };

            await _botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                cacheTime: 0,
                isPersonal: true,
                cancellationToken: cancellationToken);
        }

        private async Task BotOnCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}",
                cancellationToken: cancellationToken
            );

            await _botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                text: $"Received {callbackQuery.Data}",
                cancellationToken: cancellationToken
                );
        }

        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } messageText)
                return;
            Task<Message> action = messageText.Split(' ')[0] switch
            {
                "/inline_keyboard" => SendInlineKeyboard(_botClient, message, cancellationToken),
                "/keyboard" => SendReplyKeyboard(_botClient, message, cancellationToken),
                "/remove" => RemoveKeyboard(_botClient, message, cancellationToken),
                "/photo" => SendFile(_botClient, message, cancellationToken),
                "/request" => RequestContactAndLocation(_botClient, message, cancellationToken),
                "/inline_mode" => StartInlineQuery(_botClient, message, cancellationToken),
                "/throw" => FailingHandler(_botClient, message, cancellationToken),
                "/ulgu" => UlguHandler(_botClient, message, cancellationToken),
                _ => Usage(_botClient, message, cancellationToken)
            };
            Message sentMessage = await action;
            _logger.LogInformation("The message was sent with id: {SentMesageId}", sentMessage.MessageId);
        }

        private async Task<Message> UlguHandler(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Выберите факультет",
                replyMarkup: _admissionPlanService.DoWork(message.Text),
                cancellationToken: cancellationToken
                );

        }

        private async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            const string usage = "Usage:\n" +
                                 "/inline_keyboard - send inline keyboard\n" +
                                 "/keyboard    - send custom keyboard\n" +
                                 "/remove      - remove custom keyboard\n" +
                                 "/photo       - send a photo\n" +
                                 "/request     - request location or contact\n" +
                                 "/inline_mode - send keyboard with Inline Query";

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }

        private Task<Message> FailingHandler(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            throw new IndexOutOfRangeException();
        }

        private async Task<Message> StartInlineQuery(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(
                InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Inline Mode"));

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Press the button to start Inline Query",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        private async Task<Message> RequestContactAndLocation(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup RequestReplyKeyboard = new(
                new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Who or Where are you?",
                replyMarkup: RequestReplyKeyboard,
                cancellationToken: cancellationToken);
        }

        private async Task<Message> SendFile(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendChatActionAsync(
                chatId: message.Chat.Id,
                chatAction: ChatAction.UploadPhoto,
                cancellationToken: cancellationToken
            );
            const string filePath = "Files/exampleFile.png";
            await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
            return await botClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: new InputFile(fileStream, fileName),
                caption: "Nice Picture",
                cancellationToken: cancellationToken
            );
        }

        private async Task<Message> RemoveKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Remove keyboard",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken
            );
        }

        private async Task<Message> SendReplyKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                    new KeyboardButton[] {"1.1", "1.2"},
                    new KeyboardButton[] {"2.1", "2.2"},
                }
            )
            {
                ResizeKeyboard = true
            };
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }

        private async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendChatActionAsync(
                chatId: message.Chat.Id,
                chatAction: ChatAction.Typing,
                cancellationToken: cancellationToken
            );

            await Task.Delay(500, cancellationToken);
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("3.1", "31"),
                        InlineKeyboardButton.WithCallbackData("3.2", "32")
                    }
                }
            );
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken
            );
        }
    }
}