using PortableLibrary.Core.Database;
using PortableLibrary.Core.Services;

namespace PortableLibrary.Core.Infrastructure.Services
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
