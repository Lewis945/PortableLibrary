using PortableLibraryTelegramBot.Configuration;
using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Messaging.Mappings;
using PortableLibraryTelegramBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PortableLibraryTelegramBot.Processing.Inline
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
