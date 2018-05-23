using System.Threading.Tasks;
using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Processing.Inline;
using PortableLibrary.TelegramBot.Services;
using Xunit;

namespace PortableLibraryTelegramBot.Tests.Commands.Exit
{
    public class ExitInlineCommandTests:BaseInlineCommandTests
    {
        #region Inline Command Tests

        [Fact]
        public async Task Should_Process_English_Exit_Library_Command()
        {
            await SuccessfullyProcessExitLibraryCommand("exit", null, $"exitlibengdb");
        }


        [Fact]
        public async Task Should_Process_Russian_Exit_Library_Command()
        {
            await SuccessfullyProcessExitLibraryCommand("выйти", null, $"exitlibrusdb");
        }

        #endregion

        #region Private Test Methods

        private async Task SuccessfullyProcessExitLibraryCommand(string commandAlias, string arguments, string dbName)
        {
            bool isExitLibraryTriggered = false;

            using (var context = new BotDataContext(GetDatabaseOptions<BotDataContext>(dbName)))
            {
                var databaseService = new DatabaseService(context);
                var commandSequenceProcessor = new InlineCommandProcessor(ClientMock.Object, Configuration, databaseService);

                commandSequenceProcessor.OnExitLibrary += () =>
                {
                    isExitLibraryTriggered = true;
                };

                await commandSequenceProcessor.ProcessInlineCommand(ChatId, commandAlias, arguments);
            }

            Assert.True(isExitLibraryTriggered);
        }

        #endregion
    }
}
