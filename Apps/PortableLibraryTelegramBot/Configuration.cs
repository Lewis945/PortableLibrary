using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortableLibrary.TelegramBot.Configuration;

namespace PortableLibraryTelegramBot
{
    public class Configuration
    {
        internal static string GetAuthConfigurationFilePath(string[] args)
        {
            var configPath = args.Length > 0 ? args[0] : null;

            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException(nameof(args),
                    "Path to auth configuration file is not specified at index 0.");

            return configPath;
        }

        internal static string GetConfigurationFilePath(string[] args)
        {
            var configPath = args.Length > 1 ? args[1] : null;

            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException(nameof(args),
                    "Path to configuration file is not specified at index 1.");

            return configPath;
        }

        public static async Task<TelegramAuthConfiguration> GetAuthConfiguration(string path)
        {
            var fileContent = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<TelegramAuthConfiguration>(fileContent);
        }

        public static async Task<TelegramConfiguration> GetConfiguration(string path)
        {
            var fileContent = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<TelegramConfiguration>(fileContent);
        }
    }
}