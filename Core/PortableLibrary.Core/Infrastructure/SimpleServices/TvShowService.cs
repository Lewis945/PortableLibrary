using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.SimpleServices;

namespace PortableLibrary.Core.Infrastructure.SimpleServices
{
    public class TvShowService : ITvShowService
    {
        #region Fields

        private PortableLibraryDataContext _context;

        #endregion

        #region .ctor

        public TvShowService(PortableLibraryDataContext context)
        {
            _context = context;
        }

        #endregion

        #region ITvShowService

        public async Task<bool> AddLibraryTvShowAsync(string tvShowName, string author, string libraryName)
        {
            try
            {
                var library =
                    await _context.BookLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.Name == libraryName);

                if (library == null)
                    return false;

                var libraryBook = library.Books.FirstOrDefault(b => b.Name == bookName && b.Author == author);

                if (libraryBook != null)
                    return false;

                libraryBook = new LibraryBook
                {
                    Name = bookName,
                    Author = author
                };

                library.Books.Add(libraryBook);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Private Methods

        

        #endregion
    }
}
