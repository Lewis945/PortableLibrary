using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PortableLibrary.TelegramBot.Processing.Inline
{
    public class InlineCommandProcessor
    {
        #region Fields

        private readonly TelegramBotClient _client;
        private readonly TelegramConfiguration _configuration;
        private DatabaseService _databaseService;

        #endregion

        #region .ctor

        public InlineCommandProcessor(TelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
        {
            _client = client;
            _configuration = configuration;
            _databaseService = databaseService;
        }

        #endregion

        #region Public Methods

        public async Task<bool> ProcessInlineCommand(ChatId chatId, List<string> items)
        {
            var commandAlias = items.First();
            commandAlias = commandAlias.Replace("/", "");

            items.RemoveAt(0);

            var command = _configuration.GetCommand(commandAlias, out var aliasModel);

            if (command == null)
            {
                //send message back
                return false;
            }

            var arguments = string.Join(" ", items);

            var argumentsModel = command.InlineArguments.FirstOrDefault(a => a.Language == aliasModel.Language);
            if (argumentsModel == null)
                throw new Exception("");

            var options = argumentsModel.Options.Select(o => o.Option).Distinct();

            var line = Regex.Replace(argumentsModel.Arguments, string.Join("|", options), "");

            switch (command.Command)
            {
                case Command.Enter:
                    break;
                case Command.Exit:
                    break;
                case Command.Cancel:
                    break;
                case Command.Add:
                    await ProcessAddInlineCommand(chatId);
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

        private async Task<bool> ProcessAddInlineCommand(ChatId chatId)
        {
            return true;
        }

        #endregion
    }
}