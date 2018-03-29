using PortableLibrary.Core.Database;
using PortableLibrary.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace PortableLibrary.TelegramBot.Services
{
    public class LibraryService : ILibraryService
    {
        private PortableLibraryDataContext _context;

        public LibraryService(PortableLibraryDataContext context)
        {
            _context = context;
        }

        public void AddLibrary(string name, LibraryType type)
        {
        }
    }
}
