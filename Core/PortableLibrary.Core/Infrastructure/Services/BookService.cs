using PortableLibrary.Core.Database;
using PortableLibrary.Core.Services;

namespace PortableLibrary.Core.Infrastructure.Services
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
