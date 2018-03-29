﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Processing.Inline;
using PortableLibrary.TelegramBot.Processing.Sequence;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PortableLibraryTelegramBot
{
    public class Bot
    {
        #region Fields

        private readonly TelegramBotClient _client;
        private readonly TelegramConfiguration _configuration;

        #endregion

        #region .ctor

        public Bot(TelegramBotClient client, TelegramConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;

            _client.OnMessage += OnMessageReceived;
            _client.OnMessageEdited += OnMessageReceived;
            _client.OnCallbackQuery += OnCallbackQueryReceived;
            _client.OnInlineQuery += OnInlineQueryReceived;
            _client.OnInlineResultChosen += OnChosenInlineResultReceived;
            _client.OnReceiveError += OnReceiveError;
        }

        #endregion

        #region Public Methods

        public async Task RunAsync()
        {
            var me = await _client.GetMeAsync();

            Console.Title = me.Username;

            _client.StartReceiving();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            _client.StopReceiving();
        }

        #endregion

        #region Private Methods

        private async Task ProcessTextMessageAsync(string message, ChatId chatId, DatabaseService service,
            LibraryService libraryService, BookService bookService, TvShowService tvShowService)
        {
            try
            {
                //Command
                if (message.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var items = message.Split(' ').ToList();

                    if (items.Count == 0)
                    {
                        //send message back
                        return;
                    }

                    string firstItem = items.First();
                    string command = firstItem.Replace("/", string.Empty);

                    if (items.Count == 1)
                    {
                        var commandSequenceProcessor = new CommandSequenceProcessor(_client, _configuration, service);
                        var commandFound = await commandSequenceProcessor.StartCommandSequence(chatId, command);
                        if (!commandFound)
                            await SendDefaultAsync(chatId);
                    }
                    else
                    {
                        // process inline command string
                        var inlineCommandProcessor = new InlineCommandProcessor(_client, _configuration, service, libraryService, bookService, tvShowService);
                        var commandFound = await inlineCommandProcessor.ProcessInlineCommand(chatId, command, string.Join(" ", items.Skip(1)));
                        if (!commandFound)
                            await SendDefaultAsync(chatId);
                    }
                }
                // Message
                else
                {
                    var commandSequenceProcessor = new CommandSequenceProcessor(_client, _configuration, service);
                    var isSequenceFound = await commandSequenceProcessor.ProcessCommandSequence(chatId, message);
                    if (!isSequenceFound)
                        await SendDefaultAsync(chatId);
                }
            }
            catch (Exception ex)
            {
                await service.ClearSequence(chatId.Identifier);
                throw;
            }
        }

        #endregion

        #region Event Handlers

        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            var user = message.From;
            var chat = message.Chat;

            if (message.Type != MessageType.TextMessage) return;

            var options = new DbContextOptionsBuilder<BotDataContext>()
                .UseInMemoryDatabase(databaseName: "PortableLibrary")
                .Options;

            var libraryOptions = new DbContextOptionsBuilder<PortableLibraryDataContext>()
               .UseInMemoryDatabase(databaseName: "PortableLibrary")
               .Options;

            using (var context = new BotDataContext(options))
            {
                var databaseService = new DatabaseService(context);
                using (var libraryContext = new PortableLibraryDataContext(libraryOptions))
                {
                    var libraryService = new LibraryService(libraryContext);
                    var bookService = new BookService(libraryContext);
                    var tvShowService = new TvShowService(libraryContext);

                    await ProcessTextMessageAsync(message.Text, message.Chat.Id, databaseService,
                        libraryService, bookService, tvShowService);
                }
            }
        }

        private async Task SendDefaultAsync(ChatId id)
        {
            const string usage = @"Usage:
/inline   - send inline keyboard
/keyboard - send custom keyboard
/photo    - send a photo
/request  - request location or contact
";

            await _client.SendTextMessageAsync(
                id,
                usage,
                replyMarkup: new ReplyKeyboardRemove());
        }

        private async void OnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await _client.AnswerCallbackQueryAsync(
                callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }

        private async void OnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            //InlineQueryResultBase[] results = {
            //    new InlineQueryResultLocation(
            //        id: "1",
            //        latitude: 40.7058316f,
            //        longitude: -74.2581888f,
            //        title: "New York")   // displayed result
            //        {
            //            InputMessageContent = new InputLocationMessageContent(
            //                latitude: 40.7058316f,
            //                longitude: -74.2581888f)    // message if result is selected
            //        },

            //    new InlineQueryResultLocation(
            //        id: "2",
            //        latitude: 13.1449577f,
            //        longitude: 52.507629f,
            //        title: "Berlin") // displayed result
            //        {

            //            InputMessageContent = new InputLocationMessageContent(
            //                latitude: 13.1449577f,
            //                longitude: 52.507629f)   // message if result is selected
            //        }
            //};

            //await _client.AnswerInlineQueryAsync(
            //    inlineQueryEventArgs.InlineQuery.Id,
            //    results,
            //    isPersonal: true,
            //    cacheTime: 0);
        }

        private void OnChosenInlineResultReceived(object sender,
            ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private void OnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }

        #endregion
    }
}