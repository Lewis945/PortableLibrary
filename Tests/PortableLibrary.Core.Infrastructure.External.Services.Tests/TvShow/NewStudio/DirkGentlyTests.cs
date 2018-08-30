using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.NewStudio;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.NewStudio
{
    public class DirkGentlyTests : NewstudioTestsBase
    {
        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency()
        {
            var model = await Service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=520");

            #region Tv Show

            Assert.Equal("Холистическое Детективное агентство Дирка Джентли", model.Title, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", model.OriginalTitle, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(2, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(8, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Холистическое Детективное агентство Дирка Джентли", s1E1.TvShowTitle, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", s1E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s1E1.Quality, true);

            Assert.Equal(new DateTime(2016, 11, 24, 21, 48, 0), s1E1.DateReleased);

            #endregion

            #region Episode 4

            var s1E4 = season1.Episodes.First(e => e.Index == 4);

            Assert.Equal("Холистическое Детективное агентство Дирка Джентли", s1E4.TvShowTitle, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", s1E4.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s1E4.Quality, true);

            Assert.Equal(new DateTime(2016, 11, 23, 11, 1, 0), s1E4.DateReleased);

            #endregion

            #region Episode 8

            var s1E8 = season1.Episodes.First(e => e.Index == 8);

            Assert.Equal("Холистическое Детективное агентство Дирка Джентли", s1E8.TvShowTitle, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", s1E8.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s1E8.Quality, true);

            Assert.Equal(new DateTime(2016, 12, 16, 18, 14, 0), s1E8.DateReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(10, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Холистическое Детективное агентство Дирка Джентли", s2E1.TvShowTitle, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", s2E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s2E1.Quality, true);

            Assert.Equal(new DateTime(2017, 11, 5, 23, 48, 0), s2E1.DateReleased);

            #endregion

            #region Episode 5

            var s2E5 = season2.Episodes.First(e => e.Index == 5);

            Assert.Equal("Холистическое Детективное агентство Дирка Джентли", s2E5.TvShowTitle, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", s2E5.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s2E5.Quality, true);

            Assert.Equal(new DateTime(2017, 11, 17, 22, 44, 0), s2E5.DateReleased);

            #endregion

            #region Episode 10

            var s2E10 = season2.Episodes.First(e => e.Index == 10);

            Assert.Equal("Холистическое Детективное агентство Дирка Джентли", s2E10.TvShowTitle, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", s2E10.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s2E10.Quality, true);

            Assert.Equal(new DateTime(2017, 12, 23, 9, 25, 0), s2E10.DateReleased);

            #endregion

            #endregion
        }
    }
}
