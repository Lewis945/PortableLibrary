using Microsoft.EntityFrameworkCore;
using Moq;
using PortableLibrary.TelegramBot.Configuration;
using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PortableLibraryTelegramBotTests.Commands
{
    public class BaseInlineCommandTests
    {
        #region Fields

        protected TelegramConfiguration Configuration;
        protected Mock<ITelegramBotClient> ClientMock;

        protected ChatId ChatId;

        #endregion

        #region .ctor

        public BaseInlineCommandTests()
        {
            Configuration = PortableLibraryTelegramBot.Configuration.GetConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "Configuration", "TelegramBotConfiguration.json")).Result;

            ChatId = new ChatId(12);

            ClientMock = new Mock<ITelegramBotClient>();
            ClientMock.Setup(foo => foo.SendChatActionAsync(It.IsAny<ChatId>(), It.IsIn<ChatAction>(), default(System.Threading.CancellationToken)));
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
