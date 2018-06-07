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

        public static CountByFlagsQueryResponse GetCountByFlags(this BaseLibrary library)
        {
            CountByFlagsQueryResponse GetCounts(IEnumerable<BaseLibraryEntity> entities)
            {
                var baseLibraryEntities = entities.ToList();
                var result = baseLibraryEntities.Select(e => new
                {
                    e.IsPublished,
                    e.IsFavourite,
                    e.IsProcessing,
                    e.IsProcessed,
                    e.IsProcessingPlanned,
                    e.IsWaitingToBecomeGlobal
                }).ToList();

                return new CountByFlagsQueryResponse
                {
                    All = result.Count,
                    IsPublished = result.Count(r => r.IsPublished),
                    IsFavourite = result.Count(r => r.IsFavourite),
                    IsProcessing = result.Count(r => r.IsProcessing),
                    IsProcessed = result.Count(r => r.IsProcessed),
                    IsProcessingPlanned = result.Count(r => r.IsProcessingPlanned),
                    IsWaitingToBecomeGlobal = result.Count(r => r.IsWaitingToBecomeGlobal)
                };
            }

            if (library is BooksLibrary booksLibrary)
                return GetCounts(booksLibrary.Books);

            if (library is TvShowsLibrary tvShowsLibrary)
                return GetCounts(tvShowsLibrary.TvShows);

            return default(CountByFlagsQueryResponse);
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