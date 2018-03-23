using PortableLibraryTelegramBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace PortableLibraryTelegramBot
{
    public class Bot
    {
        #region Fields

        private TelegramBotClient _client;
        private List<CommandMapping> _mappings;

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

        public Bot(TelegramBotClient client, List<CommandMapping> mappings)
        {
            _client = client;
            _mappings = mappings;

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

        #region Event Handlers

        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            var commands = message.Text.Split(' ').ToList();

            if (commands.Count == 0)
            {
                throw new Exception("");
            }

            var mapping = _mappings.FirstOrDefault(m => m.Value == commands[0]);

            // add/добавить -> показать клавиатуру с выбором типов (библиотека, книга, сериал) -> введите название -> если библиотека, выберите тип (книга, сериал)

            // add/remove library book/tvshow 'name'
            // добавить/удалить бибилотеку книга/сериал 'название'

            // add/remove book/tvshow 'name' 'libraryname'
            // добавить/удалить книгу/сериал 'название' в 'название библиотеки'

            switch (mapping.Command)
            {
                case Command.Add:

                    if (commands.Count < 4)
                    {
                        throw new Exception("");
                    }

                    var type1 = commands[1];
                    var type2 = commands[2];
                    var name = commands[3];

                    Enum.TryParse<Command>(type1, out var command1);
                    Enum.TryParse<Command>(type2, out var command2);

                    if (command1 == Command.Library)
                    {
                        if (command2 == Command.Book)
                        {
                            _bookService.AddBookLibrary(name, mapping.Language);
                        }
                        if (command2 == Command.TvShow)
                        {
                            _tvShowService.AddTvShowLibrary(name, mapping.Language);
                        }
                    }
                    else if (command1 == Command.Book)
                    {
                        _bookService.AddBook(commands[1], mapping.Language);
                    }
                    else if (command1 == Command.TvShow)
                    {
                        _tvShowService.AddTvShow(name, mapping.Language);
                    }

                    break;
                default:
                    const string usage = @"Usage:
/inline   - send inline keyboard
/keyboard - send custom keyboard
/photo    - send a photo
/request  - request location or contact
";

                    await _client.SendTextMessageAsync(
                        message.Chat.Id,
                        usage,
                        replyMarkup: new ReplyKeyboardRemove());
                    break;
            }
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
