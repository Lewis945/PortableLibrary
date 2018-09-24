using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Automapper;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.SimpleServices;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.Infrastructure.SimpleServices
{
    public class LibraryService : ILibraryService
    {
        #region Fields

        private readonly PortableLibraryDataContext _context;

        #endregion

        #region .ctor

        public LibraryService(PortableLibraryDataContext context)
        {
            _context = context;
        }

        #endregion

        #region ILibraryService

        public IAsyncEnumerable<LibraryListModel> GetLibrariesAsync(string userId, LibraryType type = LibraryType.None,
            bool extended = false)
        {
            try
            {
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    if (extended)
                        cfg.AddProfile<ExtendedLibraryProfile>();
                    else
                        cfg.AddProfile<LibraryProfile>();
                });

                switch (type)
                {
                    case LibraryType.Book:
                        return _context.BookLibraries
                            .ProjectTo<LibraryListModel>(mapperConfig)
                            .ToAsyncEnumerable();
                    case LibraryType.TvShow:
                        return _context.TvShowsLibraries
                            .ProjectTo<LibraryListModel>(mapperConfig)
                            .ToAsyncEnumerable();
                    default:
                        var bookLibraries = _context.BookLibraries.ProjectTo<LibraryListModel>(mapperConfig);
                        var tvShowLibraries = _context.TvShowsLibraries.ProjectTo<LibraryListModel>(mapperConfig);
                        return bookLibraries.Concat(tvShowLibraries).ToAsyncEnumerable();
                }
            }
            catch (Exception ex)
            {
                // log errors
                throw;
            }
        }

        public async Task<bool> AddLibraryAsync(string userId, string name, LibraryType type)
        {
            try
            {
                string alias = name.FormatAlias();
                var library = await GetLibraryAsync(userId, alias, type);

                if (library != null)
                    return false;

                switch (type)
                {
                    case LibraryType.Book:
                        library = new BooksLibrary();
                        _context.BookLibraries.Add((BooksLibrary)library);
                        break;
                    case LibraryType.TvShow:
                        library = new TvShowsLibrary();
                        _context.TvShowsLibraries.Add((TvShowsLibrary)library);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                var now = DateTime.Now;

                library.Name = name;
                library.DateCreated = now;
                library.DateModified = now;
                library.IsPublished = true;
                library.Alias = alias;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveLibraryAsync(string userId, string name, LibraryType type)
        {
            try
            {
                var library = await GetLibraryAsync(userId, name.FormatAlias(), type);

                if (library == null)
                    return false;

                library.IsDeleted = true;

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

        private async Task<BaseLibrary> GetLibraryAsync(string userId, string alias, LibraryType type)
        {
            switch (type)
            {
                case LibraryType.Book:
                    return await _context.BookLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.AppUserId == userId && l.Alias == alias);
                case LibraryType.TvShow:
                    return await _context.TvShowsLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.AppUserId == userId && l.Alias == alias);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}