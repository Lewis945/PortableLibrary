using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Processing.Sequence;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace PortableLibraryTelegramBot.Tests.Commands.Add
{
    public class AddCommandSequenceTests
    {
        #region Fields

        private readonly TelegramConfiguration _configuration;
        private readonly Mock<ITelegramBotClient> _client;

        private DbContextOptions<BotDataContext> _options;
        private readonly ChatId _chatId;

        #endregion

        #region .ctor

        public AddCommandSequenceTests()
        {
            _configuration = Configuration.GetConfigurationAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "Configuration", "TelegramBotConfiguration.json")).Result;

            _options = new DbContextOptionsBuilder<BotDataContext>()
                .UseInMemoryDatabase(databaseName: "TestPortableLibrary")
                .Options;

            _chatId = new ChatId(12);

            _client = new Mock<ITelegramBotClient>();
            _client.Setup(foo => foo.SendChatActionAsync(It.IsAny<ChatId>(), It.IsIn<ChatAction>(),
                default(System.Threading.CancellationToken)));
        }

        #endregion

        #region Start Command Sequence Tests

        [Fact]
        public async Task Should_Process_English_Add_Command()
        {
            await SuccessfullyProcessAddCommand("add", "library", "book", "Test library", "eng");
        }

        [Fact]
        public async Task Should_Process_Russian_Add_Command()
        {
            await SuccessfullyProcessAddCommand("добавить", "библиотека", "книга", "Тестовая библиотека", "rus");
        }

        #endregion

        #region Private Test Methods

        private async Task SuccessfullyProcessAddCommand(string commandAlias, string type, string libraryType,
            string name, string language)
        {
            using (var context = new BotDataContext(GetDatabaseOptions($"addcommand{language}db")))
            {
                var databaseService = new DatabaseService(context);
                var commandSequenceProcessor =
                    new CommandSequenceProcessor(_client.Object, _configuration, databaseService);
                await commandSequenceProcessor.StartCommandSequence(_chatId, commandAlias);

                var sequence = await context.ChatCommandSequencesState.ToListAsync();
                Assert.Single(sequence);
                var item = sequence.First();
                Assert.Equal(Command.Add.ToString(), item.Command);
                Assert.Equal(_chatId.Identifier, item.ChatId);
                Assert.Equal(language, item.Language);

                await commandSequenceProcessor.ProcessCommandSequence(_chatId, type);

                sequence = await context.ChatCommandSequencesState.ToListAsync();
                Assert.Equal(2, sequence.Count);
                item = sequence.Last();
                Assert.Equal(PortableLibrary.TelegramBot.Messaging.Enums.Type.Library.ToString(), item.Command);
                Assert.Equal(_chatId.Identifier, item.ChatId);
                Assert.Equal(language, item.Language);

                await commandSequenceProcessor.ProcessCommandSequence(_chatId, libraryType);

                sequence = await context.ChatCommandSequencesState.ToListAsync();
                Assert.Equal(3, sequence.Count);
                item = sequence.Last();
                Assert.Equal(PortableLibrary.TelegramBot.Messaging.Enums.Type.Book.ToString(), item.Command);
                Assert.Equal(_chatId.Identifier, item.ChatId);
                Assert.Equal(language, item.Language);

                await commandSequenceProcessor.ProcessCommandSequence(_chatId, name);

                sequence = await context.ChatCommandSequencesState.ToListAsync();
                Assert.Empty(sequence);
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