using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.SimpleServices;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.SimpleServices
{
    public class LibraryServiceTests
    {
        #region Tests

        [Fact]
        public async Task Should_Add_Books_Library()
        {
            const string title = "Books library";

            using (var context =
                new PortableLibraryDataContext(GetDatabaseOptions<PortableLibraryDataContext>("libserviceaddlibrary")))
            {
                var service = new LibraryService(context);

                var result = await service.AddLibraryAsync(title, LibraryType.Book);
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
                new PortableLibraryDataContext(GetDatabaseOptions<PortableLibraryDataContext>("libserviceremovelibrary")))
            {
                context.BookLibraries.Add(new BooksLibrary
                {
                    Name = title,
                    Alias = title.FormatAlias()
                });

                await context.SaveChangesAsync();

                var service = new LibraryService(context);
                var result = await service.RemoveLibraryAsync(title, LibraryType.Book);
                Assert.True(result);

                var library = await context.BookLibraries.FirstOrDefaultAsync(l => l.Name == title);
                Assert.NotNull(library);
                Assert.True(library.IsDeleted);
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