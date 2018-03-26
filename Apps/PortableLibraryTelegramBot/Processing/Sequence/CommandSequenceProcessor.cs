﻿using PortableLibraryTelegramBot.Configuration;
using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Messaging.Mappings;
using PortableLibraryTelegramBot.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PortableLibraryTelegramBot.Processing.Sequence
{
    public class CommandSequenceProcessor
    {
        #region Fields

        private readonly TelegramBotClient _client;
        private readonly TelegramConfiguration _configuration;
        private DatabaseService _databaseService;

        private AddCommandSequenceProcessor _addCommandSequenceProcessor;

        #endregion

        #region .ctor

        public CommandSequenceProcessor(TelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
        {
            _client = client;
            _configuration = configuration;
            _databaseService = databaseService;

            _addCommandSequenceProcessor = new AddCommandSequenceProcessor(client, configuration, databaseService);
        }

        #endregion

        #region Public Methods

        public async Task<bool> StartCommandSequence(ChatId chatId, string commandAlias)
        {
            var command = _configuration.GetCommand(commandAlias, out var alias);

            if (command == null)
            {
                //send message back
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
                    await _addCommandSequenceProcessor.ProcessStartCommandSequence(chatId, alias.Language);
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

        public async Task<bool> ProcessCommandSequence(ChatId chatId, string message)
        {
            var sequenceKey = await _databaseService.GetCommandSequenceKey(chatId.Identifier);

            if (!string.IsNullOrWhiteSpace(sequenceKey))
            {
                var firstCommand = await _databaseService.GetFirstCommand(chatId.Identifier);

                if (!Enum.TryParse<Command>(firstCommand?.Command, out var command))
                    return false;

                switch (command)
                {
                    case Command.Enter:
                        break;
                    case Command.Exit:
                        break;
                    case Command.Cancel:
                        break;
                    case Command.Add:
                        await _addCommandSequenceProcessor.ProcessCommandSequence(chatId, sequenceKey, message);
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

            return true;
        }

        #endregion
    }
}