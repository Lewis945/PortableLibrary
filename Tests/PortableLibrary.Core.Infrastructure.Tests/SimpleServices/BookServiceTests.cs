using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Automapper;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.SimpleServices;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.Tests.SimpleServices
{
    public class BookServiceTests
    {
        #region Fields

        private readonly Mapper _mapper;

        #endregion

        #region .ctor

        public BookServiceTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<ExternalBooksProfile>(); });
            _mapper = new Mapper(config);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task Should_Add_Library_Book()
        {
            const string libraryTitle = "Books library";
            const string bookTitle = "Огненный факультет";
            const string author = "Алекс Кош";

            string libraryAlias = libraryTitle.FormatAlias();

            using (var context =
                new PortableLibraryDataContext(GetDatabaseOptions<PortableLibraryDataContext>("bookserviceaddlibbook")))
            {
                var service = new BookService(context);

                context.BookLibraries.Add(new BooksLibrary
                {
                    Name = libraryTitle,
                    Alias = libraryAlias
                });
                await context.SaveChangesAsync();

                var result = await service.AddLibraryBookAsync(bookTitle, author, libraryAlias, null);
                Assert.True(result);

                var booksLibrary = await context.BookLibraries.FirstOrDefaultAsync(l => l.Name == libraryTitle);
                Assert.NotNull(booksLibrary);

                var libraryBook = booksLibrary.Books.FirstOrDefault(b => b.Name == bookTitle);
                Assert.NotNull(libraryBook);
                Assert.Equal(author, libraryBook.Author);
            }
        }

        #endregion

        #region Private Methods

        private DbContextOptions<T> GetDatabaseOptions<T>(string dbName)
            where T : DbContext =>
            new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

        #endregion
    }
}