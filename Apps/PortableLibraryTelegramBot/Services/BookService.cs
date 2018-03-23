using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace PortableLibraryTelegramBot.Services
{
    public class BookService
    {
        private TelegramBotClient _client;

        public BookService(TelegramBotClient client)
        {
            _client = client;
        }

        public void AddBookLibrary(string name, string language)
        {
        }

        public void AddBook(string name, string language)
        {
        }
    }
}
