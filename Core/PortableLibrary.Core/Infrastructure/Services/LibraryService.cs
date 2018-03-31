using PortableLibrary.Core.Database;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Services;

namespace PortableLibrary.Core.Infrastructure.Services
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
