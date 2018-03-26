using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortableLibraryTelegramBot.Configuration;
using Telegram.Bot;

namespace PortableLibraryTelegramBot
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var authConfiguration = await GetAuthConfiguration(GetAuthConfigurationFilePath(args));
                var configuration = await GetConfiguration(GetConfigurationFilePath(args));

                var client = new TelegramBotClient(authConfiguration.Token);

                var bot = new Bot(client, configuration);
                await bot.RunAsync();
            }
            catch (Exception ex)
            {
                // log
            }
        }

        private static string GetAuthConfigurationFilePath(string[] args)
        {
            var configPath = args.Length > 0 ? args[0] : null;

            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException(nameof(args),
                    "Path to auth configuration file is not specified at index 0.");

            return configPath;
        }

        private static string GetConfigurationFilePath(string[] args)
        {
            var configPath = args.Length > 1 ? args[1] : null;

            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException(nameof(args),
                    "Path to configuration file is not specified at index 1.");

            return configPath;
        }

        private static async Task<TelegramAuthConfiguration> GetAuthConfiguration(string path)
        {
            var fileContent = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<TelegramAuthConfiguration>(fileContent);
        }

        private static async Task<TelegramConfiguration> GetConfiguration(string path)
        {
            var fileContent = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<TelegramConfiguration>(fileContent);
        }
    }
}