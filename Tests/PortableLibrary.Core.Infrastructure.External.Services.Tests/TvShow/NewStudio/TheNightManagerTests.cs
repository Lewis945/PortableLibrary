using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.NewStudio;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.NewStudio
{
    public class TheNightManagerTests : NewstudioTestsBase
    {
        [Fact]
        public async Task Should_Extract_The_Night_Manager()
        {
            var model = await Service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=454");

            #region Tv Show

            Assert.Equal("Ночной администратор", model.Title, true);
            Assert.Equal("The Night Manager", model.OriginalTitle, true);

            Assert.NotNull(model.Seasons);
            Assert.Single(model.Seasons);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(6, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Ночной администратор", s1E1.TvShowTitle, true);
            Assert.Equal("The Night Manager", s1E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s1E1.Quality, true);

            Assert.Equal(new DateTime(2016, 3, 8, 23, 13, 0), s1E1.DateReleased);

            #endregion

            #region Episode 2

            var s1E2 = season1.Episodes.First(e => e.Index == 2);

            Assert.Equal("Ночной администратор", s1E2.TvShowTitle, true);
            Assert.Equal("The Night Manager", s1E2.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s1E2.Quality, true);

            Assert.Equal(new DateTime(2016, 3, 8, 22, 7, 0), s1E2.DateReleased);

            #endregion

            #region Episode 3

            var s1E3 = season1.Episodes.First(e => e.Index == 3);

            Assert.Equal("Ночной администратор", s1E3.TvShowTitle, true);
            Assert.Equal("The Night Manager", s1E3.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s1E3.Quality, true);

            Assert.Equal(new DateTime(2016, 3, 9, 13, 35, 0), s1E3.DateReleased);

            #endregion

            #region Episode 4

            var s1E4 = season1.Episodes.First(e => e.Index == 4);

            Assert.Equal("Ночной администратор", s1E4.TvShowTitle, true);
            Assert.Equal("The Night Manager", s1E4.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s1E4.Quality, true);

            Assert.Equal(new DateTime(2016, 3, 16, 21, 30, 0), s1E4.DateReleased);

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.Index == 5);

            Assert.Equal("Ночной администратор", s1E5.TvShowTitle, true);
            Assert.Equal("The Night Manager", s1E5.OriginalTvShowSeasonTitle, true);

            Assert.Equal("BDRip 1080p", s1E5.Quality, true);

            Assert.Equal(new DateTime(2016, 3, 29, 0, 49, 0), s1E5.DateReleased);

            #endregion

            #region Episode 6

            var s1E6 = season1.Episodes.First(e => e.Index == 6);

            Assert.Equal("Ночной администратор", s1E6.TvShowTitle, true);
            Assert.Equal("The Night Manager", s1E6.OriginalTvShowSeasonTitle, true);

            Assert.Equal("BDRip 1080p", s1E6.Quality, true);

            Assert.Equal(new DateTime(2016, 4, 1, 9, 15, 0), s1E6.DateReleased);

            #endregion

            #endregion
        }
    }
}
