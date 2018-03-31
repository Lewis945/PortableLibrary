using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Messaging.Commands.Add.Enums;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using GeneralMessage = PortableLibrary.TelegramBot.Messaging.Enums.GeneralMessage;
using Type = PortableLibrary.TelegramBot.Messaging.Enums.Type;

namespace PortableLibrary.TelegramBot.Processing.Sequence
{
    public class AddCommandSequenceProcessor : BaseCommandSequenceProcessor
    {
        private readonly ITelegramBotClient _client;
        private readonly DatabaseService _databaseService;

        public const int SequenceLength = 4;

        public AddCommandSequenceProcessor(ITelegramBotClient client, TelegramConfiguration configuration,
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
                        await ProcessAddLibraryCommandAsync(chatId, result.Language);
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
                        await ProcessAddBooksLibraryCommandAsync(chatId, result.Language);
                    }
                    else if (result.Type == Type.TvShow)
                    {
                        await ProcessAddTvShowsLibraryCommandAsync(chatId, result.Language);
                    }
                }
            }
            else if (GenerateKey(Command.Add, Type.Library, Type.Book) == sequenceKey)
            {
                var lastCommand = await _databaseService.GetLastCommand(chatId.Identifier);
                await ProcessAddBooksLibraryNameCommandAsync(chatId, message, lastCommand.Language);
            }
            else if (GenerateKey(Command.Add, Type.Library, Type.TvShow) == sequenceKey)
            {
                var lastCommand = await _databaseService.GetLastCommand(chatId.Identifier);
                await ProcessAddTvShowsLibraryNameCommandAsync(chatId, message, lastCommand.Language);
            }
        }

        #endregion

        #region Private Methods

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

            await _databaseService.AddSequenceCommandAsync(chatId.Identifier, Command.Add.ToString(), language);
            await _databaseService.SaveAsync();

            var replyKeyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, AddCommandArgumentType.Type.ToString(),
                        Type.Library.ToString(), language)),
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, AddCommandArgumentType.Type.ToString(),
                        Type.Book.ToString(), language)),
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, AddCommandArgumentType.Type.ToString(),
                        Type.TvShow.ToString(), language))
                },
                oneTimeKeyboard: true
            );

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(GeneralMessage.Choose, language),
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddLibraryCommandAsync(ChatId chatId, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddSequenceCommandAsync(chatId.Identifier,
                Type.Library.ToString(), language);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library));

            var replyKeyboard = new ReplyKeyboardMarkup(
                new[]
                {
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, AddCommandArgumentType.LibraryType.ToString(),
                        Type.Book.ToString(), language)),
                    new KeyboardButton(Configuration.GetSequenceOption(Command.Add, AddCommandArgumentType.LibraryType.ToString(),
                        Type.TvShow.ToString(), language))
                },
                oneTimeKeyboard: true
            );

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(GeneralMessage.Choose, language),
                replyMarkup: replyKeyboard);
        }

        private async Task ProcessAddBooksLibraryCommandAsync(ChatId chatId, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddSequenceCommandAsync(chatId.Identifier,
                Type.Book.ToString(), language);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.Book));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(GeneralMessage.EnterName, language));
        }

        private async Task ProcessAddTvShowsLibraryCommandAsync(ChatId chatId, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _databaseService.AddSequenceCommandAsync(chatId.Identifier,
                Type.TvShow.ToString(), language);
            await _databaseService.SaveAsync();

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.TvShow));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(GeneralMessage.EnterName, language));
        }

        private async Task ProcessAddBooksLibraryNameCommandAsync(ChatId chatId, string name, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.Book, name));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(GeneralMessage.Success, language));

            var isCleared = await _databaseService.ClearSequence(chatId.Identifier, SequenceLength - 1);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await _databaseService.SaveAsync();
        }

        private async Task ProcessAddTvShowsLibraryNameCommandAsync(ChatId chatId, string name, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            await _client.SendTextMessageAsync(chatId,
                GenerateCurrentStateMessage(language, Command.Add, Type.Library,
                    Type.TvShow, name));

            await _client.SendTextMessageAsync(chatId,
                Configuration.GetGeneralMessage(GeneralMessage.Success, language));

            var isCleared = await _databaseService.ClearSequence(chatId.Identifier, SequenceLength - 1);
            if (!isCleared)
                throw new Exception("At this point sequence must be complete.");
            await _databaseService.SaveAsync();
        }

        #endregion
    }
}