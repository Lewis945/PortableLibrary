﻿using System;
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

        public IAsyncEnumerable<LibraryListModel> GetLibrariesAsync(LibraryType type = LibraryType.None,
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

        public async Task<bool> AddLibraryAsync(string name, LibraryType type)
        {
            try
            {
                var library = await GetLibraryAsync(name, type);

                if (library != null)
                    return false;

                switch (type)
                {
                    case LibraryType.Book:
                        library = new BooksLibrary();
                        _context.BookLibraries.Add((BooksLibrary) library);
                        break;
                    case LibraryType.TvShow:
                        library = new TvShowsLibrary();
                        _context.TvShowsLibraries.Add((TvShowsLibrary) library);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                var now = DateTime.Now;

                library.Name = name;
                library.DateCreated = now;
                library.DateModified = now;
                library.IsPublished = true;
                library.Alias = await GenerateAliasAsync(name, type);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveLibraryAsync(string name, LibraryType type)
        {
            try
            {
                var library = await GetLibraryAsync(name, type);

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

        public async Task<string> GenerateAliasAsync(string name, LibraryType type)
        {
            var alias = name.FormatAlias();
            var library = await GetLibraryAsync(alias, type, true);
            if (library != null)
                throw new Exception($"Library with the given alias ({alias}) already exists.");
            return alias;
        }

        #endregion

        #region Private Methods

        private async Task<BaseLibrary> GetLibraryAsync(string name, LibraryType type, bool isAlias = false)
        {
            switch (type)
            {
                case LibraryType.Book:
                    return await _context.BookLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && isAlias
                        ? l.Alias == name
                        : l.Name == name);
                case LibraryType.TvShow:
                    return await _context.TvShowsLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && isAlias
                        ? l.Alias == name
                        : l.Name == name);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}