using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Extensions;
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

        public async Task<bool> AddLibraryBookAsync(string bookName, string author, string userId, string libraryName)
        {
            try
            {
                string libraryAlias = libraryName.FormatAlias();
                var library =
                    await _context.BookLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.AppUserId == userId && l.Alias == libraryAlias);

                if (library == null)
                    return false;

                string bookAlias = bookName.FormatAlias(author);
                var libraryBook = library.Books.FirstOrDefault(b => b.Alias == bookAlias && b.Author == author);

                if (libraryBook != null)
                    return false;

                var now = DateTime.Now;

                libraryBook = new LibraryBook
                {
                    Name = bookName,
                    Author = author,
                    DateCreated = now,
                    DateModified = now,
                    Alias = bookAlias,
                    IsPublished = true
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

        public async Task<bool> RemoveLibraryBookAsync(string name, string author, string userId, string libraryName)
        {
            try
            {
                string libraryAlias = libraryName.FormatAlias();
                string bookAlias = name.FormatAlias(author);
                var book = await _context.LibrariesBooks.FirstOrDefaultAsync(b => b.BooksLibrary.Alias == libraryAlias &&
                b.BooksLibrary.AppUserId == userId && !b.BooksLibrary.IsDeleted && b.Alias == bookAlias && b.Author == author);

                if (book == null)
                    return false;

                book.IsDeleted = true;

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