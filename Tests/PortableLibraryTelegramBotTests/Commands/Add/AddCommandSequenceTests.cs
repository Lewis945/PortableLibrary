using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.TelegramBot.Data.Database;
using PortableLibrary.TelegramBot.Processing.Sequence;
using PortableLibrary.TelegramBot.Services;
using PortableLibraryTelegramBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Xunit;

namespace PortableLibraryTelegramBotTests.Commands.Add
{
    public class AddCommandSequenceTests
    {
        [Fact]
        public async Task Test1()
        {
            var authConfiguration = await Configuration.GetAuthConfiguration("");
            var configuration = await Configuration.GetConfiguration("");

            var client = new TelegramBotClient(authConfiguration.Token);
            
            var options = new DbContextOptionsBuilder<BotDataContext>()
                .UseInMemoryDatabase(databaseName: "TestPortableLibrary")
                .Options;

            const string commandAlias = "add";
            
            using (var context = new BotDataContext(options))
            {
                var databaseService = new DatabaseService(context);
                var commandSequenceProcessor = new CommandSequenceProcessor(client, configuration, databaseService);
                await commandSequenceProcessor.StartCommandSequence(new ChatId(12), commandAlias);
            }
        }
    }
}