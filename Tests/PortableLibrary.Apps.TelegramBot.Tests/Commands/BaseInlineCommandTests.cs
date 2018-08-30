using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortableLibrary.TelegramBot.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PortableLibraryTelegramBot.Tests.Commands
{
    public class BaseInlineCommandTests
    {
        #region Fields

        protected static readonly TelegramConfiguration Configuration;
        protected static readonly Mock<ITelegramBotClient> ClientMock;

        protected static readonly ChatId ChatId;

        #endregion

        #region .ctor

        static BaseInlineCommandTests()
        {
            ChatId = new ChatId(12);

            Configuration = PortableLibraryTelegramBot.Configuration.GetConfigurationAsync(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "Configuration", "TelegramBotConfiguration.json")).Result;

            ClientMock = new Mock<ITelegramBotClient>();
            ClientMock.Setup(foo => foo.SendChatActionAsync(It.IsAny<ChatId>(), It.IsIn<ChatAction>(),
                default(System.Threading.CancellationToken)));
        }

        public BaseInlineCommandTests()
        {
        }

        #endregion

        #region Protected Methods

        protected DbContextOptions<T> GetDatabaseOptions<T>(string dbName)
            where T : DbContext
            =>
                new DbContextOptionsBuilder<T>()
                    .UseInMemoryDatabase(databaseName: dbName)
                    .Options;

        #endregion
    }
}