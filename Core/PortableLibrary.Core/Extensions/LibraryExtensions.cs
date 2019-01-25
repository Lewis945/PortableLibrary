using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Enums;
using System;
using System.Linq;

namespace PortableLibrary.Core.Extensions
{
    public static class LibraryExtensions
    {
        public static int GetItemsCount(this BaseLibrary library, Func<BaseLibraryEntity, bool> predicate = null)
        {
            if (library is BooksLibrary booksLibrary)
            {
                if (predicate != null)
                    return booksLibrary.Books.Count(predicate);
                return booksLibrary.Books.Count;
            }

            if (library is TvShowsLibrary tvShowsLibrary)
            {
                if (predicate != null)
                    return tvShowsLibrary.TvShows.Count(predicate);
                return tvShowsLibrary.TvShows.Count;
            }

            return 0;
        }

        public static LibraryType GetLibraryType(this BaseLibrary library)
        {
            if (library is BooksLibrary)
                return LibraryType.Book;

            if (library is TvShowsLibrary)
                return LibraryType.TvShow;

            return LibraryType.None;
        }
    }
}