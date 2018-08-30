using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PortableLibraryTelegramBot
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var authConfiguration =
                    await Configuration.GetAuthConfiguration(Configuration.GetAuthConfigurationFilePath(args));
                var configuration = await Configuration.GetConfigurationAsync(Configuration.GetConfigurationFilePath(args));

                var client = new TelegramBotClient(authConfiguration.Token);

                var bot = new Bot(client, configuration);
                await bot.RunAsync();
            }
            catch (Exception ex)
            {
                // log
            }
        }
    }
}