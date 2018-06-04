using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortableLibrary.Core.Enums;
using PortableLibrary.TelegramBot.Configuration;
using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Processing.Inline;
using PortableLibrary.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace PortableLibraryTelegramBot.Tests.Commands.Add
{
    public class AddInlineCommandTests
    {
        #region Fields

        private readonly TelegramConfiguration _configuration;
        private readonly Mock<ITelegramBotClient> _clientMock;

        private DbContextOptions<BotDataContext> _options;
        private readonly ChatId _chatId;

        #endregion

        #region .ctor

        public AddInlineCommandTests()
        {
            _configuration = Configuration.GetConfigurationAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "Configuration", "TelegramBotConfiguration.json")).Result;

            _options = new DbContextOptionsBuilder<BotDataContext>()
                .UseInMemoryDatabase(databaseName: "TestPortableLibrary")
                .Options;

            _chatId = new ChatId(12);

            _clientMock = new Mock<ITelegramBotClient>();
            _clientMock.Setup(foo => foo.SendChatActionAsync(It.IsAny<ChatId>(), It.IsIn<ChatAction>(),
                default(System.Threading.CancellationToken)));

            //_libraryServiceMock = new Mock<ILibraryService>();
            //_libraryServiceMock.Setup(s => s.AddLibrary(It.IsAny<string>(), It.IsIn<LibraryType>()));

            //_bookServiceMock = new Mock<IBookService>();

            //_tvShowServiceMock = new Mock<ITvShowService>();
        }

        #endregion

        #region Inline Command Tests

        [Fact]
        public async Task Should_Process_English_Add_Books_Library_Command_With_Arguments()
        {
            const string libraryName = "Test library of books";
            await SuccessfullyProcessAddCommand("add", $"library of books {libraryName}", $"addbooklibengdb",
                libraryName, LibraryType.Book);
        }

        [Fact]
        public async Task Should_Process_English_Add_TvShows_Library_Command_With_Arguments()
        {
            const string libraryName = "Test library of tv shows";
            await SuccessfullyProcessAddCommand("add", $"library of tv shows {libraryName}", $"addtvshowlibengdb",
                libraryName, LibraryType.TvShow);
        }


        [Fact]
        public async Task Should_Process_Russian_Add_Books_Library_Command_With_Arguments()
        {
            const string libraryName = "Тестовая бибилотека книг";
            await SuccessfullyProcessAddCommand("добавить", $"библиотеку книг {libraryName}", $"addbooklibrusdb",
                libraryName, LibraryType.Book);
        }

        [Fact]
        public async Task Should_Process_Russian_Add_TvShows_Library_Command_With_Arguments()
        {
            const string libraryName = "Тестовая бибилотека сериалов";
            await SuccessfullyProcessAddCommand("добавить", $"библиотеку сериалов {libraryName}", $"addtvshowlibrusdb",
                libraryName, LibraryType.TvShow);
        }

        #endregion

        #region Private Test Methods

        private async Task SuccessfullyProcessAddCommand(string commandAlias, string arguments, string dbName,
            string libraryName, LibraryType libraryType)
        {
            bool isAddLibraryTriggered = false;

            using (var context = new BotDataContext(GetDatabaseOptions<BotDataContext>(dbName)))
            {
                var databaseService = new DatabaseService(context);
                var commandSequenceProcessor =
                    new InlineCommandProcessor(_clientMock.Object, _configuration, databaseService);

                commandSequenceProcessor.OnAddLibrary += (string name, LibraryType type) =>
                {
                    isAddLibraryTriggered = true;
                    Assert.Equal(libraryName, name);
                    Assert.Equal(libraryType, type);
                    return Task.CompletedTask;
                };

                await commandSequenceProcessor.ProcessInlineCommand(_chatId, commandAlias, arguments);
            }

            Assert.True(isAddLibraryTriggered);
        }

        #endregion

        #region Private Methods

        private DbContextOptions<T> GetDatabaseOptions<T>(string dbName)
            where T : DbContext
            =>
                new DbContextOptionsBuilder<T>()
                    .UseInMemoryDatabase(databaseName: dbName)
                    .Options;

        #endregion
    }
}