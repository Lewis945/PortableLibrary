using Microsoft.EntityFrameworkCore;
using PortableLibraryTelegramBot.Configuration;
using PortableLibraryTelegramBot.Data.Database;
using PortableLibraryTelegramBot.Extensions;
using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Messaging.Mappings;
using PortableLibraryTelegramBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        private TelegramBotClient _client;
        private TelegramConfiguration _configuration;

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

        private string GenerateKey(params object[] args)
        {
            if (args.Length > 0)
                return string.Join(".", args.Select(a => a.ToString()));
            return null;
        }

        private string GenerateCurrentStateMessage(string language, params object[] args)
        {
            var mapping = _configuration.Messages.GetMapping(m => m.Message == Messaging.Enums.Message.YourCurrentSelection && m.Language == language);
            return string.Format(mapping.Value, string.Join(" -> ", args.Select(a => GetLocalizedMessage(language, a))));
        }

        private string GetLocalizedMessage(string language, object value)
        {
            var command = _configuration.Commands.FirstOrDefault(c => c.Command.ToString().ToLowerInvariant() == value.ToString().ToLowerInvariant() && c.Language == language);
            if (command != null)
                return command.Value.ToLowerInvariant();

            var reply = _configuration.Replies.FirstOrDefault(c => c.Reply.ToString().ToLowerInvariant() == value.ToString().ToLowerInvariant() && c.Language == language);
            if (reply != null)
                return reply.Value.ToLowerInvariant();

            return value.ToString();
        }

        private async Task ProcessClearCommandAsync(ChatId chatId)
        {
            var updates = await _client.GetUpdatesAsync();

            //foreach (var update in updates.Where(u => u.Message != null && u.Message.Chat.Id == chatId.Identifier))
            //    await _client.DeleteMessageAsync(chatId, update.Message.MessageId);
        }

        private async Task ProcessAddCommandAsync(string language, ChatId chatId, DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Command.Add.ToString(), language, false);
            await databaseService.SaveAsync();

            var replyKeyboard = new ReplyKeyboardMarkup(
                                new KeyboardButton[3] {
                                    new KeyboardButton(_configuration.Replies.GetMapping(r => r.Reply == Messaging.Enums.Type.Library && r.Language == language).Value),
                                    new KeyboardButton(_configuration.Replies.GetMapping(r => r.Reply == Messaging.Enums.Type.Book && r.Language == language).Value),
                                    new KeyboardButton(_configuration.Replies.GetMapping(r => r.Reply == Messaging.Enums.Type.TvShow && r.Language == language).Value)
                                },
                                oneTimeKeyboard: true
                            );

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages.GetMapping(m => m.Message == Messaging.Enums.Message.Choose && m.Language == language).Value,
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddLibraryCommandAsync(string language, ChatId chatId, DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Command.Add.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.Library.ToString(), language, false);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId, GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library));

            var replyKeyboard = new ReplyKeyboardMarkup(
                                new KeyboardButton[2] {
                                    new KeyboardButton(_configuration.Replies.GetMapping(r => r.Reply == Messaging.Enums.Type.Book && r.Language == language).Value),
                                    new KeyboardButton(_configuration.Replies.GetMapping(r => r.Reply == Messaging.Enums.Type.TvShow && r.Language == language).Value)
                                },
                                oneTimeKeyboard: true
                            );

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages.GetMapping(m => m.Message == Messaging.Enums.Message.Choose && m.Language == language).Value,
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddBooksLibraryCommandAsync(string language, ChatId chatId, DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.Book.ToString(), language, false);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId, GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.Book));

            await _client.SendTextMessageAsync(chatId,
               _configuration.Messages.GetMapping(m => m.Message == Messaging.Enums.Message.EnterName && m.Language == language).Value);
        }

        private async Task ProcessAddTvShowsLibraryCommandAsync(string language, ChatId chatId, DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.TvShow.ToString(), language, false);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId, GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.TvShow));

            await _client.SendTextMessageAsync(chatId,
               _configuration.Messages.GetMapping(m => m.Message == Messaging.Enums.Message.EnterName && m.Language == language).Value);
        }

        private async Task ProcessAddBooksLibraryNameCommandAsync(string name, string language, ChatId chatId, DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.Book.ToString(), language, true);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId, GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.Book, name));

            await _client.SendTextMessageAsync(chatId,
               _configuration.Messages.GetMapping(m => m.Message == Messaging.Enums.Message.Success && m.Language == language).Value);

            var isCleared = await databaseService.ClearSequence(chatId.Identifier);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await databaseService.SaveAsync();
        }

        private async Task ProcessAddTvShowsLibraryNameCommandAsync(string name, string language, ChatId chatId, DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Messaging.Enums.Type.TvShow.ToString(), language, true);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId, GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.TvShow, name));

            await _client.SendTextMessageAsync(chatId,
               _configuration.Messages.GetMapping(m => m.Message == Messaging.Enums.Message.Success && m.Language == language).Value);

            var isCleared = await databaseService.ClearSequence(chatId.Identifier);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await databaseService.SaveAsync();
        }

        private async Task ProcessTextMessageAsync(string message, ChatId chatId, DatabaseService service)
        {
            try
            {
                var items = message.Split(' ').ToList();

                if (items.Count == 0)
                {
                    //send message back
                    return;
                }

                if (message.StartsWith("/"))
                {
                    if (items.Count == 1)
                    {
                        var firstItem = items.First();
                        firstItem = firstItem.Replace("/", string.Empty);
                        var mapping = _configuration.Commands.FirstOrDefault(m => m.Value.ToLowerInvariant() == firstItem.ToLowerInvariant());

                        if (mapping == null)
                        {
                            //send message back
                            return;
                        }

                        string language = mapping.Language;

                        switch (mapping.Command)
                        {
                            case Command.Add:
                                await ProcessAddCommandAsync(language, chatId, service);
                                break;
                            case Command.Remove:
                                break;
                            case Command.Display:
                                break;
                            case Command.Mark:
                                break;
                            case Command.SetName:
                                break;
                            case Command.Clear:
                                await ProcessClearCommandAsync(chatId);
                                break;
                            default:
                                await SendDefaultAsync(chatId);
                                break;
                        }
                    }
                    else
                    {
                        // process full command string
                    }
                }
                // Message
                else
                {
                    var sequenceKey = await service.GetCommandSequenceKey(chatId.Identifier);

                    if (!string.IsNullOrWhiteSpace(sequenceKey))
                    {
                        if (GenerateKey(Command.Add) == sequenceKey)
                        {
                            if (items.Count == 1)
                            {
                                var firstItem = items.First();
                                var reply = _configuration.Replies.FirstOrDefault(r => r.Value.ToLowerInvariant() == firstItem.ToLowerInvariant());

                                if (reply == null)
                                {
                                    //send message back
                                    return;
                                }

                                if (reply.Reply == Messaging.Enums.Type.Library)
                                {
                                    await ProcessAddLibraryCommandAsync(reply.Language, chatId, service);
                                }
                                else if (reply.Reply == Messaging.Enums.Type.Book)
                                {
                                    //
                                }
                                else if (reply.Reply == Messaging.Enums.Type.TvShow)
                                {
                                    //
                                }
                            }
                        }
                        else if (GenerateKey(Command.Add, Messaging.Enums.Type.Library) == sequenceKey)
                        {
                            if (items.Count == 1)
                            {
                                var firstItem = items.First();
                                var reply = _configuration.Replies.FirstOrDefault(r => r.Value.ToLowerInvariant() == firstItem.ToLowerInvariant());

                                if (reply == null)
                                {
                                    //send message back
                                    return;
                                }

                                if (reply.Reply == Messaging.Enums.Type.Book)
                                {
                                    await ProcessAddBooksLibraryCommandAsync(reply.Language, chatId, service);
                                }
                                else if (reply.Reply == Messaging.Enums.Type.TvShow)
                                {
                                    await ProcessAddTvShowsLibraryCommandAsync(reply.Language, chatId, service);
                                }
                            }
                        }
                        else if (GenerateKey(Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.Book) == sequenceKey)
                        {
                            var lastCommand = await service.GetLastCommand(chatId.Identifier);
                            await ProcessAddBooksLibraryNameCommandAsync(message, lastCommand.Language, chatId, service);
                        }
                        else if (GenerateKey(Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.TvShow) == sequenceKey)
                        {
                            var lastCommand = await service.GetLastCommand(chatId.Identifier);
                            await ProcessAddTvShowsLibraryNameCommandAsync(message, lastCommand.Language, chatId, service);
                        }
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

            if (message == null || message.Type != MessageType.TextMessage) return;

            var options = new DbContextOptionsBuilder<BotDataContext>()
               .UseInMemoryDatabase(databaseName: "PortableLibrary")
               .Options;

            using (var context = new BotDataContext(options))
            {
                var databaseService = new DatabaseService(context);
                await ProcessTextMessageAsync(message.Text, message.Chat.Id, databaseService);
            }

            // add/добавить -> показать клавиатуру с выбором типов (библиотека, книга, сериал) -> введите название -> если библиотека, выберите тип (книга, сериал)

            // add/remove library book/tvshow 'name'
            // добавить/удалить бибилотеку книга/сериал 'название'

            // add/remove book/tvshow 'name' 'libraryname'
            // добавить/удалить книгу/сериал 'название' в 'название библиотеки'
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

        private void OnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
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
