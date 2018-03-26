using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibraryTelegramBot.Configuration;
using PortableLibraryTelegramBot.Extensions;
using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PortableLibraryTelegramBot.Processing
{
    public class AddCommandSequenceProcessor
    {
        private readonly TelegramBotClient _client;
        private readonly TelegramConfiguration _configuration;
        private readonly DatabaseService _databaseService;

        public AddCommandSequenceProcessor(TelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
        {
            _client = client;
            _configuration = configuration;
            _databaseService = databaseService;
        }

        #region Public Methods

        public async Task ProcessCommandSequence(ChatId chatId, string sequenceKey, string message)
        {
            if (GenerateKey(Command.Add) == sequenceKey)
            {
                var items = message.Split(" ").ToList();
                if (items.Count == 1)
                {
                    var firstItem = items.First();
                    var reply = _configuration.Replies.FirstOrDefault(r =>
                        string.Equals(r.Value, firstItem, StringComparison.InvariantCultureIgnoreCase));

                    if (reply == null)
                    {
                        //send message back
                        return;
                    }

                    if (reply.Reply == Messaging.Enums.Type.Library)
                    {
                        await ProcessAddLibraryCommandAsync(reply.Language, chatId, _databaseService);
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
                var items = message.Split(" ").ToList();
                if (items.Count == 1)
                {
                    var firstItem = items.First();
                    var reply = _configuration.Replies.FirstOrDefault(r =>
                        string.Equals(r.Value, firstItem, StringComparison.InvariantCultureIgnoreCase));

                    if (reply == null)
                    {
                        //send message back
                        return;
                    }

                    if (reply.Reply == Messaging.Enums.Type.Book)
                    {
                        await ProcessAddBooksLibraryCommandAsync(reply.Language, chatId, _databaseService);
                    }
                    else if (reply.Reply == Messaging.Enums.Type.TvShow)
                    {
                        await ProcessAddTvShowsLibraryCommandAsync(reply.Language, chatId, _databaseService);
                    }
                }
            }
            else if (GenerateKey(Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.Book) ==
                     sequenceKey)
            {
                var lastCommand = await _databaseService.GetLastCommand(chatId.Identifier);
                await ProcessAddBooksLibraryNameCommandAsync(message, lastCommand.Language, chatId,
                    _databaseService);
            }
            else if (GenerateKey(Command.Add, Messaging.Enums.Type.Library, Messaging.Enums.Type.TvShow) ==
                     sequenceKey)
            {
                var lastCommand = await _databaseService.GetLastCommand(chatId.Identifier);
                await ProcessAddTvShowsLibraryNameCommandAsync(message, lastCommand.Language, chatId,
                    _databaseService);
            }
        }

        #endregion

        private static string GenerateKey(params object[] args)
        {
            return args.Length > 0 ? string.Join(".", args.Select(a => a.ToString())) : null;
        }

        private string GenerateCurrentStateMessage(string language, params object[] args)
        {
            var mapping = _configuration.Messages.GetMapping(m =>
                m.Message == Messaging.Enums.Message.YourCurrentSelection && m.Language == language);
            return string.Format(mapping.Value,
                string.Join(" -> ", args.Select(a => GetLocalizedMessage(language, a))));
        }

        private string GetLocalizedMessage(string language, object value)
        {
            var command = _configuration.Commands.FirstOrDefault(c =>
                c.Command.ToString().ToLowerInvariant() == value.ToString().ToLowerInvariant() &&
                c.Language == language);
            if (command != null)
                return command.Value.ToLowerInvariant();

            var reply = _configuration.Replies.FirstOrDefault(c =>
                c.Reply.ToString().ToLowerInvariant() == value.ToString().ToLowerInvariant() && c.Language == language);
            return reply != null ? reply.Value.ToLowerInvariant() : value.ToString();
        }

        private async Task ProcessAddCommandAsync(string language, ChatId chatId, DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Command.Add.ToString(), language,
                false);
            await databaseService.SaveAsync();

            var replyKeyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton(_configuration.Replies
                        .GetMapping(r => r.Reply == Messaging.Enums.Type.Library && r.Language == language).Value),
                    new KeyboardButton(_configuration.Replies
                        .GetMapping(r => r.Reply == Messaging.Enums.Type.Book && r.Language == language).Value),
                    new KeyboardButton(_configuration.Replies
                        .GetMapping(r => r.Reply == Messaging.Enums.Type.TvShow && r.Language == language).Value)
                },
                oneTimeKeyboard: true
            );

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages
                    .GetMapping(m => m.Message == Messaging.Enums.Message.Choose && m.Language == language).Value,
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddLibraryCommandAsync(string language, ChatId chatId,
            DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Command.Add.ToString(), language,
                true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.Library.ToString(), language, false);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library));

            var replyKeyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton(_configuration.Replies
                        .GetMapping(r => r.Reply == Messaging.Enums.Type.Book && r.Language == language).Value),
                    new KeyboardButton(_configuration.Replies
                        .GetMapping(r => r.Reply == Messaging.Enums.Type.TvShow && r.Language == language).Value)
                },
                oneTimeKeyboard: true
            );

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages
                    .GetMapping(m => m.Message == Messaging.Enums.Message.Choose && m.Language == language).Value,
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddBooksLibraryCommandAsync(string language, ChatId chatId,
            DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.Book.ToString(), language, false);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library,
                    Messaging.Enums.Type.Book));

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages
                    .GetMapping(m => m.Message == Messaging.Enums.Message.EnterName && m.Language == language).Value);
        }

        private async Task ProcessAddTvShowsLibraryCommandAsync(string language, ChatId chatId,
            DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.TvShow.ToString(), language, false);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library,
                    Messaging.Enums.Type.TvShow));

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages
                    .GetMapping(m => m.Message == Messaging.Enums.Message.EnterName && m.Language == language).Value);
        }

        private async Task ProcessAddBooksLibraryNameCommandAsync(string name, string language, ChatId chatId,
            DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.Book.ToString(), language, true);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library,
                    Messaging.Enums.Type.Book, name));

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages
                    .GetMapping(m => m.Message == Messaging.Enums.Message.Success && m.Language == language).Value);

            var isCleared = await databaseService.ClearSequence(chatId.Identifier);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await databaseService.SaveAsync();
        }

        private async Task ProcessAddTvShowsLibraryNameCommandAsync(string name, string language, ChatId chatId,
            DatabaseService databaseService)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.Library.ToString(), language, true);
            await databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Messaging.Enums.Type.TvShow.ToString(), language, true);
            await databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Messaging.Enums.Type.Library,
                    Messaging.Enums.Type.TvShow, name));

            await _client.SendTextMessageAsync(chatId,
                _configuration.Messages
                    .GetMapping(m => m.Message == Messaging.Enums.Message.Success && m.Language == language).Value);

            var isCleared = await databaseService.ClearSequence(chatId.Identifier);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await databaseService.SaveAsync();
        }
    }
}