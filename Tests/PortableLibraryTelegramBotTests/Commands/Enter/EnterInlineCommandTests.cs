using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Processing.Inline;
using PortableLibrary.TelegramBot.Services;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibraryTelegramBotTests.Commands.Enter
{
    public class EnterInlineCommandTests : BaseInlineCommandTests
    {
        #region Inline Command Tests

        [Fact]
        public async Task Should_Process_English_Enters_Library_Command_With_Arguments()
        {
            const string libraryName = "Test library of books";
            await SuccessfullyProcessEnterLibraryCommand("enter", libraryName, $"enterlibengdb", libraryName);
        }


        [Fact]
        public async Task Should_Process_Russian_Enter_Library_Command_With_Arguments()
        {
            const string libraryName = "Тестовая бибилотека книг";
            await SuccessfullyProcessEnterLibraryCommand("войти", libraryName, $"enterlibrusdb", libraryName);
        }

        #endregion

        #region Private Test Methods

        private async Task SuccessfullyProcessEnterLibraryCommand(string commandAlias, string arguments, string dbName,
            string libraryName)
        {
            bool isEnterLibraryTriggered = false;

            using (var context = new BotDataContext(GetDatabaseOptions<BotDataContext>(dbName)))
            {
                var databaseService = new DatabaseService(context);
                var commandSequenceProcessor = new InlineCommandProcessor(ClientMock.Object, Configuration, databaseService);

                commandSequenceProcessor.OnEnterLibrary += (name) =>
                {
                    isEnterLibraryTriggered = true;
                    Assert.Equal(libraryName, name);
                };

                await commandSequenceProcessor.ProcessInlineCommand(ChatId, commandAlias, arguments);
            }

            Assert.True(isEnterLibraryTriggered);
        }

        #endregion
    }
}
