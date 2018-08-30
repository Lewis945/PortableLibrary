using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.NewStudio
{
    public class WhiteCollarTests : NewstudioTestsBase
    {
        [Fact]
        public async Task Should_Extract_White_Collar()
        {
            var model = await Service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=137");

            #region Tv Show

            Assert.Equal("Белый воротничок", model.Title, true);
            Assert.Equal("White Collar", model.OriginalTitle, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(3, model.Seasons.Count);

            #endregion

            #region Season 4

            var season1 = model.Seasons.First(s => s.Index == 4);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(6, season1.Episodes.Count);

            #region Episode 11

            var s4E11 = season1.Episodes.First(e => e.Index == 11);

            Assert.Equal("Белый воротничок", s4E11.TvShowTitle, true);
            Assert.Equal("White Collar", s4E11.OriginalTvShowSeasonTitle, true);

            Assert.Equal(new DateTime(2013, 2, 18, 2, 30, 0), s4E11.DateReleased);

            #endregion

            #region Episode 13

            var s4E13 = season1.Episodes.First(e => e.Index == 13);

            Assert.Equal("Белый воротничок", s4E13.TvShowTitle, true);
            Assert.Equal("White Collar", s4E13.OriginalTvShowSeasonTitle, true);

            Assert.Equal(new DateTime(2013, 2, 16, 20, 40, 0), s4E13.DateReleased);

            #endregion

            #region Episode 16

            var s4E16 = season1.Episodes.First(e => e.Index == 16);

            Assert.Equal("Белый воротничок", s4E16.TvShowTitle, true);
            Assert.Equal("White Collar", s4E16.OriginalTvShowSeasonTitle, true);

            Assert.Equal(new DateTime(2013, 3, 20, 23, 3, 0), s4E16.DateReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(13, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Белый воротничок", s5E1.TvShowTitle, true);
            Assert.Equal("White Collar", s5E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s5E1.Quality, true);

            Assert.Equal(new DateTime(2013, 10, 27, 21, 37, 0), s5E1.DateReleased);

            #endregion

            #region Episode 6

            var s5E6 = season5.Episodes.First(e => e.Index == 6);

            Assert.Equal("Белый воротничок", s5E6.TvShowTitle, true);
            Assert.Equal("White Collar", s5E6.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s5E6.Quality, true);

            Assert.Equal(new DateTime(2013, 12, 10, 16, 35, 0), s5E6.DateReleased);

            #endregion

            #region Episode 13

            var s5E13 = season5.Episodes.First(e => e.Index == 13);

            Assert.Equal("Белый воротничок", s5E13.TvShowTitle, true);
            Assert.Equal("White Collar", s5E13.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s5E13.Quality, true);

            Assert.Equal(new DateTime(2014, 2, 5, 10, 11, 0), s5E13.DateReleased);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(6, season6.Episodes.Count);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            Assert.Equal("Белый воротничок", s6E1.TvShowTitle, true);
            Assert.Equal("White Collar", s6E1.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s6E1.Quality, true);

            Assert.Equal(new DateTime(2014, 12, 19, 22, 40, 0), s6E1.DateReleased);

            #endregion

            #region Episode 3

            var s6E3 = season6.Episodes.First(e => e.Index == 3);

            Assert.Equal("Белый воротничок", s6E3.TvShowTitle, true);
            Assert.Equal("White Collar", s6E3.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s6E3.Quality, true);

            Assert.Equal(new DateTime(2014, 11, 29, 18, 27, 0), s6E3.DateReleased);

            #endregion

            #region Episode 6

            var s6E6 = season6.Episodes.First(e => e.Index == 6);

            Assert.Equal("Белый воротничок", s6E6.TvShowTitle, true);
            Assert.Equal("White Collar", s6E6.OriginalTvShowSeasonTitle, true);

            Assert.Equal("WEBDL 1080p", s6E6.Quality, true);

            Assert.Equal(new DateTime(2014, 12, 26, 17, 03, 0), s6E6.DateReleased);

            #endregion

            #endregion
        }
    }
}
