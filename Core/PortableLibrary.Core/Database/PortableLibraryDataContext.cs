﻿using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;

namespace PortableLibrary.Core.Database
{
    public class PortableLibraryDataContext : DbContext
    {
        #region .ctor

        public PortableLibraryDataContext(DbContextOptions options)
            : base(options)
        { }

        #endregion

        #region DbSets

        public DbSet<BooksLibrary> BookLibraries { get; set; }
        public DbSet<LibraryBook> LibrariesBooks { get; set; }

        public DbSet<TvShowsLibrary> TvShowsLibraries { get; set; }
        public DbSet<LibraryTvShow> LibrariesTvShows { get; set; }

        #endregion
    }
}