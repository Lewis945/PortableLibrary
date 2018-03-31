using PortableLibrary.Core.Enums;
using PortableLibrary.TelegramBot.Configuration.Commands.Enums.Inline.Add;
using PortableLibrary.TelegramBot.EventHandlers;
using PortableLibrary.TelegramBot.Extensions;
using PortableLibrary.TelegramBot.Messaging.Commands.Enter.Enums;
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

        public event EnterLibraryEventHandler OnEnterLibrary;

        #endregion

        #region Public Methods

        public async Task<bool> ProcessEnterInlineCommand(ChatId chatId, List<ArgumentModel> arguments,
            string argumentsLineName, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            if (arguments.Count < 1)
            {
                //send message
                return false;
            }

            Enum.TryParse<EnterCommandInlineArgumentsLine>(argumentsLineName, out var type);

            switch (type)
            {
                case EnterCommandInlineArgumentsLine.EnterLibrary:
                    var name = arguments.FirstOrDefault(a => a.Argument.Matches(EnterCommandArgumentType.Name));

                    OnEnterLibrary?.Invoke(name.Alias);
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