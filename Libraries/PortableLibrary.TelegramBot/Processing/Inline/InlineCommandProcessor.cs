﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Configuration.Commands.Enums.Inline;
using PortableLibrary.TelegramBot.Configuration.Commands.Models.Inline;
using PortableLibrary.TelegramBot.EventHandlers;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Messaging.Mappings;
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

        #region Events

        public event AddLibraryEventHandler AddLibraryEvent;

        #endregion
        
        #region .ctor

        public InlineCommandProcessor(ITelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
        {
            _client = client;
            _configuration = configuration;
            _databaseService = databaseService;

            _addCommandInlineProcessor = new AddCommandInlineProcessor(client, configuration, databaseService);
            _addCommandInlineProcessor.AddLibraryEvent += AddLibraryEvent;
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

        private static List<ArgumentModel> GetArguments(CommandMapping command, string arguments, string language,
            out string name)
        {
            var argumentsLines = command.ArgumentsLines.Where(a => a.Language == language).ToList();
            if (argumentsLines.Count == 0)
                throw new Exception("");

            foreach (var argumentsLine in argumentsLines)
            {
                var formattedPattern =
                    FormatArgumentsPattern(argumentsLine.Line, argumentsLine.Arguments);

                var match = Regex.Match(arguments, formattedPattern);
                if (!match.Success)
                    continue;

                var items = GetArguments(match, argumentsLine);
                if (items == null)
                    continue;

                name = argumentsLine.Name;
            }

            name = null;
            return null;
        }

        private static string FormatArgumentsPattern(string pattern, IEnumerable<InlineArgument> arguments)
        {
            foreach (var argument in arguments)
            {
                switch (argument.Type)
                {
                    //^(?<type>(?i)library(?-i))\sof\s(?<libtype>(?<bookstype>(?i)books(?-i))|(?<tvtype>(?i)tv shows(?-i)))(?<name>[a-zA-Z0-9-_'\s]+)$
                    case ArgumentType.Match:
                        pattern = pattern.Replace($"{{{argument.Argument}}}",
                            $@"(?<{argument.Argument}>{
                                    string.Join("|", argument.Options.Select(o => $"(?<{o.Option}>(?i){o.Alias}(?-i))"))
                                })");
                        break;
                    case ArgumentType.String:
                        pattern = pattern.Replace($"{{{argument.Argument}}}",
                            $@"(?<{argument.Argument}>[a-zA-Z0-9-_'\s]+)");
                        break;
                    default:
                        break;
                }
            }

            pattern = pattern.Replace(" ", @"\s");
            return pattern;
        }

        private static IEnumerable<ArgumentModel> GetArguments(Match match,
            ArgumentsLine argumentsLine)
        {
            foreach (var argument in argumentsLine.Arguments)
            {
                var argumentGroup = match.Groups.FirstOrDefault(g => g.Name == argument.Argument);
                if (argumentGroup == null)
                {
                    //validation
                    yield break;
                }

                var argumentModel = new ArgumentModel
                {
                    Argument = argumentGroup.Name,
                    Alias = argumentGroup.Value
                };

                var options = argument.Options.Select(o => o.Option).ToList();
                var option = match.Groups.FirstOrDefault(g => options.Contains(g.Name));
                if (option != null)
                    argumentModel.Option = option.Name;

                yield return argumentModel;
            }
        }

        #endregion
    }
}