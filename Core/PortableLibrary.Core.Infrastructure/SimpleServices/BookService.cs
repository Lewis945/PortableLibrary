using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.SimpleServices;
using PortableLibrary.Core.SimpleServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<LibraryBookListModel>> GetBooks(string libraryAlias, string userId)
        {
            try
            {
                var library =
                    await _context.BookLibraries.FirstOrDefaultAsync(lib => !lib.IsDeleted &&
                        lib.AppUserId == userId && lib.Alias == libraryAlias);

                if (library == null)
                    return null;

                return library.Books.Select(book => new LibraryBookListModel
                {
                    Title = book.Name,
                    Author = book.Author
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddLibraryBookAsync(string bookTitle, string author, string libraryAlias, string userId)
        {
            try
            {
                var library =
                    await _context.BookLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.AppUserId == userId && l.Alias == libraryAlias);

                if (library == null)
                    return false;

                string bookAlias = bookTitle.FormatAlias(author);
                var libraryBook = library.Books.FirstOrDefault(b => b.Alias == bookAlias && b.Author == author);

                if (libraryBook != null)
                    return false;

                var now = DateTime.Now;

                libraryBook = new LibraryBook
                {
                    Name = bookTitle,
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

        public async Task<bool> RemoveLibraryBookAsync(string bookTitle, string author, string libraryAlias, string userId)
        {
            try
            {
                var libraryExists = await _context.TvShowsLibraries.AnyAsync(lib => lib.Alias == libraryAlias && lib.AppUserId == userId);
                if (!libraryExists)
                    return false;

                string bookAlias = bookTitle.FormatAlias(author);
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