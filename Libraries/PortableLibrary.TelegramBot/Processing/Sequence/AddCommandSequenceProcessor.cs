using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = PortableLibrary.TelegramBot.Messaging.Enums.Message;
using Type = PortableLibrary.TelegramBot.Messaging.Enums.Type;

namespace PortableLibrary.TelegramBot.Processing.Sequence
{
    public class AddCommandSequenceProcessor : BaseCommandSequenceProcessor
    {
        private readonly TelegramBotClient _client;
        private readonly DatabaseService _databaseService;

        public AddCommandSequenceProcessor(TelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
            : base(configuration)
        {
            _client = client;
            _databaseService = databaseService;
        }

        #region Public Methods

        public async Task ProcessStartCommandSequence(ChatId chatId, string language)
        {
            await ProcessAddCommandAsync(chatId, language);
        }

        public async Task ProcessCommandSequence(ChatId chatId, string sequenceKey, string message)
        {
            if (GenerateKey(Command.Add) == sequenceKey)
            {
                var items = message.Split(" ").ToList();
                if (items.Count == 1)
                {
                    var firstItem = items.First();

                    var result = await Prevalidate(chatId, firstItem);

                    if (!result.Result)
                    {
                        //send message back
                        return;
                    }

                    if (result.Type == Type.Library)
                    {
                        await ProcessAddLibraryCommandAsync(result.Language, chatId);
                    }
                    else if (result.Type == Type.Book)
                    {
                        //
                    }
                    else if (result.Type == Type.TvShow)
                    {
                        //
                    }
                }
            }
            else if (GenerateKey(Command.Add, Type.Library) == sequenceKey)
            {
                var items = message.Split(" ").ToList();
                if (items.Count == 1)
                {
                    var firstItem = items.First();

                    var result = await Prevalidate(chatId, firstItem);

                    if (!result.Result)
                    {
                        //send message back
                        return;
                    }

                    if (result.Type == Type.Book)
                    {
                        await ProcessAddBooksLibraryCommandAsync(result.Language, chatId);
                    }
                    else if (result.Type == Type.TvShow)
                    {
                        await ProcessAddTvShowsLibraryCommandAsync(result.Language, chatId);
                    }
                }
            }
            else if (GenerateKey(Command.Add, Type.Library, Type.Book) ==
                     sequenceKey)
            {
                var lastCommand = await _databaseService.GetLastCommand(chatId.Identifier);
                await ProcessAddBooksLibraryNameCommandAsync(message, lastCommand.Language, chatId);
            }
            else if (GenerateKey(Command.Add, Type.Library, Type.TvShow) ==
                     sequenceKey)
            {
                var lastCommand = await _databaseService.GetLastCommand(chatId.Identifier);
                await ProcessAddTvShowsLibraryNameCommandAsync(message, lastCommand.Language, chatId);
            }
        }

        #endregion

        private async Task<(bool Result, Type Type, string Language)> Prevalidate(ChatId chatId,
            string alias)
        {
            var command = Configuration.Commands.FirstOrDefault(c => c.Command == Command.Add);
            if (command == null)
            {
                throw new Exception("");
            }

            var aliasModel = command.GetAlias(alias);

            if (aliasModel.IsEmpty)
            {
                //send message back
                return (false, default, null);
            }

            var lastCommand = await _databaseService.GetLastCommand(chatId.Identifier);

            if (lastCommand == null || !string.Equals(lastCommand.Language, aliasModel.Language,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                //send message back
                return (false, default, null);
            }

            var option = command.GetSequenceOption(aliasModel);

            if (!Enum.TryParse<Type>(option.Option, out var type))
            {
                return (false, default, null);
            }

            return (true, type, lastCommand.Language);
        }

        private async Task ProcessAddCommandAsync(ChatId chatId, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Command.Add.ToString(), language,
                false);
            await _databaseService.SaveAsync();

            var replyKeyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, ArgumentsSequenceType.Type,
                        Type.Library.ToString(), language)),
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, ArgumentsSequenceType.Type,
                        Type.Book.ToString(), language)),
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, ArgumentsSequenceType.Type,
                        Type.TvShow.ToString(), language))
                },
                oneTimeKeyboard: true
            );

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(Message.Choose, language),
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddLibraryCommandAsync(string language, ChatId chatId)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier, Command.Add.ToString(), language,
                true);
            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.Library.ToString(), language, false);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library));

            var replyKeyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, ArgumentsSequenceType.LibraryType,
                        Type.Book.ToString(), language)),
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, ArgumentsSequenceType.LibraryType,
                        Type.TvShow.ToString(), language))
                },
                oneTimeKeyboard: true
            );

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(Message.Choose, language),
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddBooksLibraryCommandAsync(string language, ChatId chatId)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.Library.ToString(), language, true);
            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.Book.ToString(), language, false);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.Book));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(Message.EnterName, language));
        }

        private async Task ProcessAddTvShowsLibraryCommandAsync(string language, ChatId chatId)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.Library.ToString(), language, true);
            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.TvShow.ToString(), language, false);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.TvShow));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(Message.EnterName, language));
        }

        private async Task ProcessAddBooksLibraryNameCommandAsync(string name, string language, ChatId chatId)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.Library.ToString(), language, true);
            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.Book.ToString(), language, true);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.Book, name));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(Message.Success, language));

            var isCleared = await _databaseService.ClearSequence(chatId.Identifier);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await _databaseService.SaveAsync();
        }

        private async Task ProcessAddTvShowsLibraryNameCommandAsync(string name, string language, ChatId chatId)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.Library.ToString(), language, true);
            await _databaseService.AddOrUpdateSequenceCommandAsync(chatId.Identifier,
                Type.TvShow.ToString(), language, true);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.TvShow, name));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(Message.Success, language));

            var isCleared = await _databaseService.ClearSequence(chatId.Identifier);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await _databaseService.SaveAsync();
        }
    }
}