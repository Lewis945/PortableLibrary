using Newtonsoft.Json;
using PortableLibraryTelegramBot.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PortableLibraryTelegramBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var configuration = await GetConfiguration(GetConfigurationFilePath(args));

                var client = new TelegramBotClient(configuration.Token);

                var bot = new Bot(client, configuration);
                await bot.RunAsync();
            }
            catch (Exception ex)
            {
                // log
            }
        }

        private static string GetConfigurationFilePath(string[] args)
        {
            var configPath = args.Length > 0 ? args[0] : null;

            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException("args[0]", "Path to configuration file is not specified.");

            return configPath;
        }

        private static async Task<TelegramConfiguration> GetConfiguration(string path)
        {
            var fileContent = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<TelegramConfiguration>(fileContent);
        }
    }
}
