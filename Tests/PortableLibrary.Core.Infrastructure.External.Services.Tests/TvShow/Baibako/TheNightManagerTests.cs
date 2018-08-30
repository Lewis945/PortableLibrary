using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.Baibako
{
    public class TheNightManagerTests : BaibakoTestsBase
    {
        [Fact]
        public async Task Should_Extract_The_Night_Manager()
        {
            var model = await Service.ExtractTvShowAsync("http://baibako.tv/iplayer/thenightmanager");

            #region Tv Show

            Assert.Equal("Ночной администратор", model.Title, true);

            Assert.NotNull(model.Seasons);
            Assert.Single(model.Seasons);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(6, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Ночной администратор", s1E1.Title, true);

            Assert.Equal(new DateTime(2016, 5, 27, 0, 0, 0, DateTimeKind.Utc), s1E1.DateReleased);

            #endregion

            #region Episode 3

            var s1E3 = season1.Episodes.First(e => e.Index == 3);

            Assert.Equal("Ночной администратор", s1E3.Title, true);

            Assert.Equal(new DateTime(2016, 5, 27, 0, 0, 0, DateTimeKind.Utc), s1E3.DateReleased);

            #endregion

            #region Episode 6

            var s1E6 = season1.Episodes.First(e => e.Index == 6);

            Assert.Equal("Ночной администратор", s1E6.Title, true);

            Assert.Equal(new DateTime(2016, 5, 27, 0, 0, 0, DateTimeKind.Utc), s1E6.DateReleased);

            #endregion

            #endregion
        }
    }
}
