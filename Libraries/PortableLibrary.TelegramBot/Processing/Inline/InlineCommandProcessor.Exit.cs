using PortableLibrary.TelegramBot.EventHandlers;
using PortableLibrary.TelegramBot.Messaging.Commands.Exit.Enums;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Processing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PortableLibrary.TelegramBot.Processing.Inline
{
    public partial class InlineCommandProcessor
    {
        #region Events

        public event ExitLibraryEventHandler OnExitLibrary;

        #endregion

        #region Public Methods

        public async Task<bool> ProcessExitInlineCommand(ChatId chatId, List<ArgumentModel> arguments,
            string argumentsLineName, string language)
        {
            await _client.SendChatActionAsync(chatId, ChatAction.Typing);
            await Task.Delay(1000);

            if (arguments.Count > 0)
            {
                //send message
                return false;
            }

            Enum.TryParse<ExitCommandInlineArgumentsLine>(argumentsLineName, out var type);

            switch (type)
            {
                case ExitCommandInlineArgumentsLine.ExitLibrary:
                    OnExitLibrary?.Invoke();
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
