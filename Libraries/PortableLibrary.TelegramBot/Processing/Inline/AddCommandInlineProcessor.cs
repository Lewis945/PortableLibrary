﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Processing.Models;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PortableLibrary.TelegramBot.Processing.Inline
{
    public class AddCommandInlineProcessor
    {
        #region Fields

        private readonly ITelegramBotClient _client;
        private readonly TelegramConfiguration _configuration;
        private DatabaseService _databaseService;

        #endregion

        #region .ctor

        public AddCommandInlineProcessor(ITelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
        {
            _client = client;
            _configuration = configuration;
            _databaseService = databaseService;
        }

        #endregion

        #region Public Methods

        public async Task<bool> ProcessAddInlineCommand(ChatId chatId, List<OptionModel> arguments,
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
                    // save library to database
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
    }
}