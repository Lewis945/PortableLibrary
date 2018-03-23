using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace PortableLibraryTelegramBot.Services
{
    public class TvShowService
    {
        private TelegramBotClient _client;

        public TvShowService(TelegramBotClient client)
        {
            _client = client;
        }

        public void AddTvShowLibrary(string name, string language)
        {
        }

        public void AddTvShow(string name, string language)
        {
        }
    }
}
