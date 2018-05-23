using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.Tests.SimpleServices
{
    public class TvShowServiceTests
    {
        #region Tests

        [Fact]
        public async Task Should()
        {
            
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