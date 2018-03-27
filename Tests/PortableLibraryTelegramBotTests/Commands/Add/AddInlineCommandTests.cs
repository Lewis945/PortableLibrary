using Microsoft.EntityFrameworkCore;
using Moq;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Processing.Inline;
using PortableLibrary.TelegramBot.Services;
using PortableLibraryTelegramBot;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace PortableLibraryTelegramBotTests.Commands.Add
{
    public class AddInlineCommandTests
    {
        #region Fields

        private TelegramConfiguration _configuration;
        private Mock<ITelegramBotClient> _client;

        private DbContextOptions<BotDataContext> _options;
        private ChatId _chatId;

        #endregion

        #region .ctor

        public AddInlineCommandTests()
        {
            _configuration = Configuration.GetConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "Configuration", "TelegramBotConfiguration.json")).Result;

            _options = new DbContextOptionsBuilder<BotDataContext>()
                .UseInMemoryDatabase(databaseName: "TestPortableLibrary")
                .Options;

            _chatId = new ChatId(12);

            _client = new Mock<ITelegramBotClient>();
            _client.Setup(foo => foo.SendChatActionAsync(It.IsAny<ChatId>(), It.IsIn<ChatAction>(), default(System.Threading.CancellationToken)));
        }

        #endregion

        #region Inline Command Tests

        [Fact]
        public async Task Should_Process_English_Add_Command_With_Arguments()
        {
            await SuccessfullyProcessAddCommand("add", "library of books Test library", "eng");
        }

        #endregion

        #region Private Test Methods

        private async Task SuccessfullyProcessAddCommand(string commandAlias, string arguments,string language)
        {
            using (var context = new BotDataContext(GetDatabaseOptions($"addcommand{language}db")))
            {
                var databaseService = new DatabaseService(context);
                var commandSequenceProcessor = new InlineCommandProcessor(_client.Object, _configuration, databaseService);
                await commandSequenceProcessor.ProcessInlineCommand(_chatId, commandAlias, arguments);
            }
        }

        #endregion

        #region Private Methods

        private DbContextOptions<BotDataContext> GetDatabaseOptions(string dbName) =>
          new DbContextOptionsBuilder<BotDataContext>()
              .UseInMemoryDatabase(databaseName: dbName)
              .Options;

        #endregion
    }
}