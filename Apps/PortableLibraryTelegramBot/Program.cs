﻿using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortableLibrary.TelegramBot.Configuration;
using Telegram.Bot;

namespace PortableLibraryTelegramBot
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var authConfiguration = await Configuration.GetAuthConfiguration(Configuration.GetAuthConfigurationFilePath(args));
                var configuration = await Configuration.GetConfiguration(Configuration.GetConfigurationFilePath(args));

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