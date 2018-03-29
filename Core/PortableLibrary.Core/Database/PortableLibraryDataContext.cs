using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database.Entities.Book;
using PortableLibrary.Core.Database.Entities.TvShow;

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
        public DbSet<TvShowsLibrary> TvShowsLibraries { get; set; }

        #endregion
    }
}