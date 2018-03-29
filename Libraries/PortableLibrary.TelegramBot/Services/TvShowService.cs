using PortableLibrary.Core.Database;
using Telegram.Bot;

namespace PortableLibrary.TelegramBot.Services
{
    public class TvShowService : ITvShowService
    {
        private PortableLibraryDataContext _context;

        public TvShowService(PortableLibraryDataContext context)
        {
            _context = context;
        }
    }
}
