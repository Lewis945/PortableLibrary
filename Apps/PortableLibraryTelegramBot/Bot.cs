using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibraryTelegramBot.Configuration;
using PortableLibraryTelegramBot.Data.Database;
using PortableLibraryTelegramBot.Extensions;
using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Processing;
using PortableLibraryTelegramBot.Services;
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

        private BookService _bookService;
        private TvShowService _tvShowService;

        #endregion

        #region Properties

        public BookService BookService
        {
            get
            {
                if (_bookService == null)
                    _bookService = new BookService(_client);

                return _bookService;
            }
        }

        public TvShowService TvShowService
        {
            get
            {
                if (_tvShowService == null)
                    _tvShowService = new TvShowService(_client);

                return _tvShowService;
            }
        }

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

        private async Task ProcessTextMessageAsync(string message, ChatId chatId, DatabaseService service)
        {
            try
            {
                var commandSequenceProcessor = new CommandSequenceProcessor(_client, _configuration, service);
                
                //Command
                if (message.StartsWith("/"))
                {   
                    var items = message.Split(' ').ToList();

                    if (items.Count == 0)
                    {
                        //send message back
                        return;
                    }
                    
                    var firstItem = items.First();
                    firstItem = firstItem.Replace("/", string.Empty);
                    var mapping = _configuration.Commands.FirstOrDefault(m =>
                        string.Equals(m.Value, firstItem, StringComparison.InvariantCultureIgnoreCase));

                    if (mapping == null)
                    {
                        //send message back
                        return;
                    }

                    string language = mapping.Language;

                    if (items.Count == 1)
                    {
                        switch (mapping.Command)
                        {
                            case Command.Enter:
                                break;
                            case Command.Exit:
                                break;
                            case Command.Cancel:
                                break;
                            case Command.Add:
                                // start add command sequence
//                                await ProcessAddCommandAsync(language, chatId, service);
                                break;
                            case Command.Remove:
                                break;
                            case Command.Display:
                                break;
                            case Command.Mark:
                                break;
                            case Command.SetName:
                                break;
                            default:
                                await SendDefaultAsync(chatId);
                                break;
                        }
                    }
                    else
                    {
                        // process full command string
                        switch (mapping.Command)
                        {
                            case Command.Enter:
                                break;
                            case Command.Exit:
                                break;
                            case Command.Cancel:
                                break;
                            case Command.Add:
                                break;
                            case Command.Remove:
                                break;
                            case Command.Display:
                                break;
                            case Command.Mark:
                                break;
                            case Command.SetName:
                                break;
                            default:
                                await SendDefaultAsync(chatId);
                                break;
                        }
                    }
                }
                // Message
                else
                {
                    var result = await commandSequenceProcessor.IsCommandSequence(chatId.Identifier);

                    if (result.Item1)
                    {
                        await commandSequenceProcessor.ProcessCommandSequence(chatId, result.Item2, message);
                    }
                    else
                    {
                        await SendDefaultAsync(chatId);
                    }
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

            using (var context = new BotDataContext(options))
            {
                var databaseService = new DatabaseService(context);
                await ProcessTextMessageAsync(message.Text, message.Chat.Id, databaseService);
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