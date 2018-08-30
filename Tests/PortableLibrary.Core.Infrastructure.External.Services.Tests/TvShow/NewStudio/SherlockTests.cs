using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.NewStudio
{
    public class SherlockTests : NewstudioTestsBase
    {
        [Fact]
        public async Task Should_Extract_Sherlock()
        {
            var model = await Service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=152");

            #region Tv Show

            Assert.Equal("Шерлок Холмс", model.Title, true);
            Assert.Equal("Sherlock", model.OriginalTitle, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(3, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(3, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Шерлок Холмс", s1E1.TvShowTitle, true);
            Assert.Equal("Sherlock", s1E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("BDRip", s1E1.Quality, true);

            Assert.Equal(new DateTime(2013, 11, 25, 16, 11, 0), s1E1.DateReleased);

            #endregion

            #region Episode 2

            var s1E2 = season1.Episodes.First(e => e.Index == 2);

            Assert.Equal("Шерлок Холмс", s1E2.TvShowTitle, true);
            Assert.Equal("Sherlock", s1E2.OriginalTvShowSeasonTitle, true);

            Assert.Equal("BDRip", s1E2.Quality, true);

            Assert.Equal(new DateTime(2013, 1, 2, 7, 41, 0), s1E2.DateReleased);

            #endregion

            #region Episode 3

            var s1E3 = season1.Episodes.First(e => e.Index == 3);

            Assert.Equal("Шерлок Холмс", s1E3.TvShowTitle, true);
            Assert.Equal("Sherlock", s1E3.OriginalTvShowSeasonTitle, true);

            Assert.Equal("BDRip", s1E3.Quality, true);

            Assert.Equal(new DateTime(2012, 1, 6, 17, 4, 0), s1E3.DateReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(3, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Шерлок Холмс", s2E1.TvShowTitle, true);
            Assert.Equal("Sherlock", s2E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("HDTVRip", s2E1.Quality, true);

            Assert.Equal(new DateTime(2012, 1, 11, 12, 45, 0), s2E1.DateReleased);

            #endregion

            #region Episode 2

            var s2E2 = season2.Episodes.First(e => e.Index == 2);

            Assert.Equal("Шерлок Холмс", s2E2.TvShowTitle, true);
            Assert.Equal("Sherlock", s2E2.OriginalTvShowSeasonTitle, true);

            Assert.Equal("HDTVRip", s2E2.Quality, true);

            Assert.Equal(new DateTime(2017, 1, 10, 9, 50, 0), s2E2.DateReleased);

            #endregion

            #region Episode 3

            var s2E3 = season2.Episodes.First(e => e.Index == 3);

            Assert.Equal("Шерлок Холмс", s2E3.TvShowTitle, true);
            Assert.Equal("Sherlock", s2E3.OriginalTvShowSeasonTitle, true);

            Assert.Equal("HDTVRip", s2E3.Quality, true);

            Assert.Equal(new DateTime(2017, 1, 10, 9, 52, 0), s2E3.DateReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(2, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Шерлок", s4E1.TvShowTitle, true);
            Assert.Equal("Sherlock", s4E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s4E1.Quality, true);

            Assert.Equal(new DateTime(2017, 1, 18, 22, 44, 0), s4E1.DateReleased);

            #endregion

            #region Episode 2

            var s4E2 = season4.Episodes.First(e => e.Index == 2);

            Assert.Equal("Шерлок", s4E2.TvShowTitle, true);
            Assert.Equal("Sherlock", s4E2.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s4E2.Quality, true);

            Assert.Equal(new DateTime(2017, 10, 4, 18, 55, 0), s4E2.DateReleased);

            #endregion

            #endregion
        }
    }
}
