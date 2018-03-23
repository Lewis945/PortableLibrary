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
        private TelegramBotClient _client;

        public Bot(TelegramBotClient client)
        {
            _client = client;

            _client.OnMessage += OnMessageReceived;
            _client.OnMessageEdited += OnMessageReceived;
            _client.OnCallbackQuery += OnCallbackQueryReceived;
            _client.OnInlineQuery += OnInlineQueryReceived;
            _client.OnInlineResultChosen += OnChosenInlineResultReceived;
            _client.OnReceiveError += OnReceiveError;
        }

        public async Task RunAsync()
        {
            var me = await _client.GetMeAsync();

            Console.Title = me.Username;

            _client.StartReceiving();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            _client.StopReceiving();
        }

        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            //IReplyMarkup keyboard = new ReplyKeyboardRemove();

            switch (message.Text.Split(' ').First())
            {
                // send inline keyboard
                case "/inline":
                    await _client.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                    await Task.Delay(500); // simulate longer running task

                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("1.1"),
                            InlineKeyboardButton.WithCallbackData("1.2"),
                        },
                        new [] // second row
                        {
                            InlineKeyboardButton.WithCallbackData("2.1"),
                            InlineKeyboardButton.WithCallbackData("2.2"),
                        }
                    });

                    await _client.SendTextMessageAsync(
                        message.Chat.Id,
                        "Choose",
                        replyMarkup: inlineKeyboard);
                    break;

                // send custom keyboard
                case "/keyboard":
                    //var replyKeyboard = new[]
                    //{
                    //    new[] { "1.1", "1.2" },
                    //    new[] { "2.1", "2.2" },
                    //};

                    //await _client.SendTextMessageAsync(
                    //    message.Chat.Id,
                    //    "Choose",
                    //    replyMarkup: new ReplyKeyboardMarkup(replyKeyboard));
                    break;

                // send a photo
                case "/photo":
                    await _client.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                    const string file = @"Files/tux.png";

                    var fileName = file.Split(Path.DirectorySeparatorChar).Last();

                    //using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //{
                    //    await _client.SendPhotoAsync(
                    //        message.Chat.Id,
                    //        fileStream,
                    //        "Nice Picture");
                    //}
                    break;

                // request location or contact
                case "/request":
                    //var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                    //{
                    //    KeyboardButton.WithRequestLocation("Location"),
                    //    KeyboardButton.WithRequestContact("Contact"),
                    //});

                    //await _client.SendTextMessageAsync(
                    //    message.Chat.Id,
                    //    "Who or Where are you?",
                    //    replyMarkup: RequestReplyKeyboard);
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
    }
}
