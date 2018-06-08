using System.Collections.Generic;
using System.Linq;
using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Database.Models.Query.LibraryEntity;
using PortableLibrary.Core.Enums;

namespace PortableLibrary.Core.Extensions
{
    public static class LibraryExtensions
    {
        public static int GetItemsCount(this BaseLibrary library)
        {
            if (library is BooksLibrary booksLibrary)
                return booksLibrary.Books.Count;

            if (library is TvShowsLibrary tvShowsLibrary)
                return tvShowsLibrary.TvShows.Count;

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