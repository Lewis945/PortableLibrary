using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.Baibako
{
    public class LimitlessTests: BaibakoTestsBase
    {
        [Fact]
        public async Task Should_Extract_Limitless()
        {
            var model = await Service.ExtractTvShowAsync("http://baibako.tv/iplayer/limitless");

            #region Tv Show

            Assert.Equal("Области тьмы", model.Title, true);

            Assert.NotNull(model.Seasons);
            Assert.Single(model.Seasons);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(22, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Области тьмы", s1E1.Title, true);

            Assert.Equal(new DateTime(2015, 9, 20, 0, 0, 0), s1E1.DateReleased);

            #endregion

            #region Episode 11

            var s1E11 = season1.Episodes.First(e => e.Index == 11);

            Assert.Equal("Области тьмы", s1E11.Title, true);

            Assert.Equal(new DateTime(2015, 12, 18, 0, 0, 0), s1E11.DateReleased);

            #endregion

            #region Episode 22

            var s1E22 = season1.Episodes.First(e => e.Index == 22);

            Assert.Equal("Области тьмы", s1E22.Title, true);

            Assert.Equal(new DateTime(2016, 4, 28, 0, 0, 0, DateTimeKind.Utc), s1E22.DateReleased);

            #endregion

            #endregion
        }
    }
}
