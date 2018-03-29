using PortableLibrary.Core.Database;
using Telegram.Bot;

namespace PortableLibrary.TelegramBot.Services
{
    public class BookService : IBookService
    {
        private PortableLibraryDataContext _context;

        public BookService(PortableLibraryDataContext context)
        {
            _context = context;
        }
    }
}
