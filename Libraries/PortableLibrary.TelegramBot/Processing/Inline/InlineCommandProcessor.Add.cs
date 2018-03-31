using PortableLibrary.Core.Enums;
using PortableLibrary.TelegramBot.Configuration.Commands.Enums.Inline.Add;
using PortableLibrary.TelegramBot.EventHandlers;
using PortableLibrary.TelegramBot.Extensions;
using PortableLibrary.TelegramBot.Messaging.Commands.Add.Enums;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Processing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PortableLibrary.TelegramBot.Processing.Inline
{
    public partial class InlineCommandProcessor
    {
        #region Events

        public event AddLibraryEventHandler OnAddLibrary;

        #endregion

        #region Public Methods

        public async Task<bool> ProcessAddInlineCommand(ChatId chatId, List<ArgumentModel> arguments,
            string argumentsLineName, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            if (arguments.Count < 2)
            {
                //send message
                return false;
            }

            Enum.TryParse<AddCommandInlineArgumentsLine>(argumentsLineName, out var type);

            switch (type)
            {
                case AddCommandInlineArgumentsLine.AddLibrary:
                    var libraryType = arguments.FirstOrDefault(a => a.Argument.Matches(AddLibraryArgument.LibraryType));
                    var name = arguments.FirstOrDefault(a => a.Argument.Matches(AddLibraryArgument.Name));

                    Enum.TryParse<LibraryType>(libraryType.Option, out var libType);

                    OnAddLibrary?.Invoke(name.Alias, libType);
                    break;
                case AddCommandInlineArgumentsLine.AddBook:
                    break;
                case AddCommandInlineArgumentsLine.AddTvShow:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await _client.SendTextMessageAsync(chatId,
                _configuration.GetGeneralMessage(GeneralMessage.Success, language));

            return true;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}