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

                var now = DateTime.Now;

                libraryBook = new LibraryBook
                {
                    Name = bookName,
                    Author = author,
                    DateCreated = now,
                    DateModified = now,
                    Alias = await GenerateLibraryBookAliasAsync(bookName, author),
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

        public async Task<bool> RemoveLibraryBookAsync(string name, string author)
        {
            try
            {
                var book = await _context.LibrariesBooks.FirstOrDefaultAsync(b => b.Name == name && b.Author == author);

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

        public async Task<string> GenerateLibraryBookAliasAsync(string name, string author)
        {
            var alias = name.FormatAlias(author);
            var book = await _context.LibrariesBooks.FirstOrDefaultAsync(b => b.Alias == alias);
            if (book != null)
                throw new Exception($"Book with the given alias ({alias}) already exists.");
            return alias;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}