using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
{
    public class NewStudioExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public NewStudioExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        #region Extract TvShow Tests

        [Fact]
        public async Task Should_Extract_Sherlock()
        {
            var service = new NewStudioExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=152");

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

        [Fact]
        public async Task Should_Extract_White_Collar()
        {
            var service = new NewStudioExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=137");

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

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency()
        {
            var service = new NewStudioExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=520");

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

        [Fact]
        public async Task Should_Extract_The_Night_Manager()
        {
            var service = new NewStudioExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://newstudio.tv/viewforum.php?f=454");
            
            #region Tv Show

            Assert.Equal("Ночной администратор", model.Title, true);
            Assert.Equal("The Night Manager", model.OriginalTitle, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(1, model.Seasons.Count);

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

        #endregion
    }
}