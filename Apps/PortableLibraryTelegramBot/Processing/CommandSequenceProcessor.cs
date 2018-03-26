using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibraryTelegramBot.Configuration;
using PortableLibraryTelegramBot.Extensions;
using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PortableLibraryTelegramBot.Processing
{
    public class CommandSequenceProcessor
    {
        private readonly TelegramBotClient _client;
        private readonly TelegramConfiguration _configuration;
        private DatabaseService _databaseService;

        private AddCommandSequenceProcessor _addCommandSequenceProcessor;

        public CommandSequenceProcessor(TelegramBotClient client, TelegramConfiguration configuration,
            DatabaseService databaseService)
        {
            _client = client;
            _configuration = configuration;
            _databaseService = databaseService;
            
            _addCommandSequenceProcessor = new AddCommandSequenceProcessor(client, configuration, databaseService);
        }

        #region Public Methods

        public async Task<(bool, string)> IsCommandSequence(ChatId chatId)
        {
            var sequenceKey = await _databaseService.GetCommandSequenceKey(chatId.Identifier);
            return (!string.IsNullOrWhiteSpace(sequenceKey), sequenceKey);
        }
        
        public async Task ProcessCommandSequence(ChatId chatId, string sequenceKey, string message)
        {
            // add if check whether it is add sequence
            await _addCommandSequenceProcessor.ProcessCommandSequence(chatId, sequenceKey, message);
        }

        #endregion
    }
}