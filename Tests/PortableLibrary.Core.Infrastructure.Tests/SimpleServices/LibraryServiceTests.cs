using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Automapper;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.SimpleServices;
using PortableLibrary.Core.SimpleServices.Models;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.Tests.SimpleServices
{
    public class LibraryServiceTests
    {
        #region Fields

        private readonly IMapper _mapper;

        #endregion

        #region .ctor

        public LibraryServiceTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<LibraryProfile>(); });
            _mapper = new Mapper(config);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task Should_Add_Books_Library()
        {
            const string title = "Books library";

            using (var context =
                new PortableLibraryDataContext(GetDatabaseOptions<PortableLibraryDataContext>("libserviceaddlibrary")))
            {
                var service = new LibraryService(context);

                var result = await service.AddLibraryAsync(title, LibraryType.Book, null);
                Assert.True(result);

                var booksLibrary = await context.BookLibraries.FirstOrDefaultAsync(l => l.Name == title);
                Assert.NotNull(booksLibrary);
                Assert.Equal(title.FormatAlias(), booksLibrary.Alias);
            }
        }

        [Fact]
        public async Task Should_Remove_Books_Library()
        {
            const string title = "Books library";

            using (var context =
                new PortableLibraryDataContext(
                    GetDatabaseOptions<PortableLibraryDataContext>("libserviceremovelibrary")))
            {
                context.BookLibraries.Add(new BooksLibrary
                {
                    Name = title,
                    Alias = title.FormatAlias()
                });

                await context.SaveChangesAsync();

                var service = new LibraryService(context);
                var result = await service.RemoveLibraryAsync(title, LibraryType.Book, null);
                Assert.True(result);

                var library = await context.BookLibraries.FirstOrDefaultAsync(l => l.Name == title);
                Assert.NotNull(library);
                Assert.True(library.IsDeleted);
            }
        }

        [Fact]
        public async Task GetLibraryAsync_WithExtendedFalse_ShouldReturnLibraryWithLimitedSetOfProperties()
        {
            const string title = "Books library";
            string alias = title.FormatAlias();

            using (var context =
                new PortableLibraryDataContext(
                    GetDatabaseOptions<PortableLibraryDataContext>("libservicegetlibrary")))
            {
                context.BookLibraries.Add(new BooksLibrary
                {
                    Name = title,
                    Alias = alias
                });

                await context.SaveChangesAsync();

                var service = new LibraryService(context);
                var result = await service.GetLibraryAsync(LibraryType.Book, alias, null);
                var library = result as LibraryExtendedModel;
                Assert.NotNull(library);
                Assert.Equal(title, library.Title);
                Assert.Equal(LibraryType.Book, library.Type);
                Assert.Equal(0, library.Items);
                Assert.False( library.Public);
            }
        }

        #endregion

        #region Private Methods

        private DbContextOptions<T> GetDatabaseOptions<T>(string dbName)
            where T : DbContext
            =>
                new DbContextOptionsBuilder<T>()
                    .UseInMemoryDatabase(databaseName: dbName)
                    .Options;

        #endregion
    }
}