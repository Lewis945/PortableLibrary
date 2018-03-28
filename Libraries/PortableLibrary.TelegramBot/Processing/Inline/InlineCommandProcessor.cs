using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Messaging.Mappings;
using PortableLibrary.TelegramBot.Messaging.Mappings.Models;
using PortableLibrary.TelegramBot.Processing.Models;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

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

            var argumentsList = GetArguments(command, arguments, aliasModel.Language, out var argumentsLineName);
            if (argumentsList == null)
            {
                //send message: arguments are wrong
                return false;
            }

            switch (command.Command)
            {
                case Command.Enter:
                    break;
                case Command.Exit:
                    break;
                case Command.Cancel:
                    break;
                case Command.Add:
                    await _addCommandInlineProcessor.ProcessAddInlineCommand(chatId, argumentsList,
                        argumentsLineName, aliasModel.Language);
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

        private static List<OptionModel> GetArguments(CommandMapping command, string arguments, string language,
            out string name)
        {
            var argumentsModels = command.InlineArguments.Where(a => a.Language == language).ToList();
            if (argumentsModels.Count == 0)
                throw new Exception("");

            foreach (var argumentsModel in argumentsModels)
            {
                var formattedPattern =
                    FormatArgumentsPattern(argumentsModel.Arguments, argumentsModel.Options);

                if (!Regex.IsMatch(arguments, formattedPattern)) continue;
                
                name = argumentsModel.Name;
                return GetArguments(arguments, formattedPattern, argumentsModel);
            }

            name = null;
            return null;
        }

        private static string FormatArgumentsPattern(string pattern, IEnumerable<InlineOptionModel> options)
        {
            foreach (var option in options)
            {
                if (option.Type == InlineArgumentOptionsType.Match)
                {
                    foreach (var value in option.Values)
                        pattern = pattern.Replace($"{{{option.Option}}}", $@"(?<{option.Option}>(?i){value})(?-i)");
                }
                else
                    pattern = pattern.Replace($"{{{option.Option}}}", $@"(?<{option.Option}>[a-zA-Z0-9-_'\s]+)");
            }

            pattern = pattern.Replace(" ", @"\s");
            return pattern;
        }

        private static List<OptionModel> GetArguments(string arguments, string pattern,
            InlineArgumentModel argumentsModel)
        {
            var match = Regex.Match(arguments, pattern);

            var argumentsList = match.Groups.Join(argumentsModel.Options, g => g.Name, o => o.Option, (g, o) =>
                new OptionModel
                {
                    Option = g.Name,
                    Value = g.Value
                }).ToList();

            return argumentsList;
        }

        #endregion
    }
}