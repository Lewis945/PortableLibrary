using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.Book;
using PortableLibrary.Core.SimpleServices;

namespace PortableLibrary.Core.Infrastructure.SimpleServices
{
    public class BookService : IBookService
    {
        #region Fields

        private readonly PortableLibraryDataContext _context;

        #endregion

        #region .ctor

        public BookService(PortableLibraryDataContext context)
        {
            _context = context;
        }

        #endregion

        #region IBookService

        public async Task<bool> AddLibraryBookAsync(string bookName, string author, string libraryName)
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