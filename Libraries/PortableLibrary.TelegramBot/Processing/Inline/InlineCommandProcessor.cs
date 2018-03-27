using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Processing.Models;
using PortableLibrary.TelegramBot.Services;
using PortableLibraryTelegramBot.Messaging.Mappings.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PortableLibrary.TelegramBot.Processing.Inline
{
    public class InlineCommandProcessor
    {
        #region Fields

        private readonly ITelegramBotClient _client;
        private readonly TelegramConfiguration _configuration;
        private DatabaseService _databaseService;

        private readonly AddCommandInlineProcessor _addCommandInlineProcessor;

        #endregion

        #region .ctor

        public InlineCommandProcessor(ITelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
        {
            _client = client;
            _configuration = configuration;
            _databaseService = databaseService;

            _addCommandInlineProcessor = new AddCommandInlineProcessor(client, configuration, databaseService);
        }

        #endregion

        #region Public Methods

        public async Task<bool> ProcessInlineCommand(ChatId chatId, string commandAlias, string arguments)
        {
            var command = _configuration.GetCommand(commandAlias, out var aliasModel);

            if (command == null)
            {
                //send message back
                return false;
            }

            var argumentsModel = command.InlineArguments.FirstOrDefault(a => a.Language == aliasModel.Language);
            if (argumentsModel == null)
                throw new Exception("");

            var argumentsList = GetArguments(arguments, argumentsModel);

            switch (command.Command)
            {
                case Command.Enter:
                    break;
                case Command.Exit:
                    break;
                case Command.Cancel:
                    break;
                case Command.Add:
                    await _addCommandInlineProcessor.ProcessAddInlineCommand(chatId, argumentsList, aliasModel.Language);
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
                    return false;
            }

            return true;
        }

        #endregion

        #region Private Methods

        private List<OptionModel> GetArguments(string arguments, InlineArgumentModel argumentsModel)
        {
            var options = argumentsModel.Options.Select(o => o.Option).Distinct();

            var pattern = argumentsModel.Arguments;
            foreach (var option in argumentsModel.Options)
            {
                if (option.Type == InlineArgumentOptionsType.Match)
                    pattern = pattern.Replace($"{{{option.Option}}}", $@"(?<{option.Option}>(?i){option.Value})(?-i)");
                else
                    pattern = pattern.Replace($"{{{option.Option}}}", $@"(?<{option.Option}>[a-zA-Z0-9-_'\s]+)");
            }
            pattern = pattern.Replace(" ", @"\s");

            var match = Regex.Match(arguments, pattern);

            var argumentsList = match.Groups.Join(argumentsModel.Options, g => g.Name, o => o.Option, (g, o) => new OptionModel
            {
                Option = g.Name,
                Value = g.Value
            }).ToList();

            return argumentsList;
        }

        #endregion
    }
}