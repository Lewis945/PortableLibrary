using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.Baibako;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
{
    public class BaibakoExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public BaibakoExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        #region Extract TvShow Tests

        [Fact]
        public async Task Should_Extract_Limitless()
        {
            var service = new BaibakoExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://baibako.tv/iplayer/limitless");

            #region Tv Show

            Assert.Equal("Области тьмы", model.Title, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(1, model.Seasons.Count);

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

        [Fact]
        public async Task Should_Extract_The_Night_Manager()
        {
            var service = new BaibakoExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://baibako.tv/iplayer/thenightmanager");

            #region Tv Show

            Assert.Equal("Ночной администратор", model.Title, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(1, model.Seasons.Count);

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

        [Fact]
        public async Task Should_Extract_Downton_Abbey()
        {
            var service = new BaibakoExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://baibako.tv/iplayer/downton-abbey");

            #region Tv Show

            Assert.Equal("Аббатство Даунтон", model.Title, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(6, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(7, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Аббатство Даунтон", s1E1.Title, true);

            Assert.Equal(new DateTime(2011, 6, 14, 0, 0, 0, DateTimeKind.Utc), s1E1.DateReleased);

            #endregion

            #region Episode 3

            var s1E3 = season1.Episodes.First(e => e.Index == 3);

            Assert.Equal("Аббатство Даунтон", s1E3.Title, true);

            Assert.Equal(new DateTime(2011, 6, 14, 0, 0, 0, DateTimeKind.Utc), s1E3.DateReleased);

            #endregion

            #region Episode 7

            var s1E7 = season1.Episodes.First(e => e.Index == 7);

            Assert.Equal("Аббатство Даунтон", s1E7.Title, true);

            Assert.Equal(new DateTime(2011, 6, 14, 0, 0, 0, DateTimeKind.Utc), s1E7.DateReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(10, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Аббатство Даунтон", s2E1.Title, true);

            Assert.Equal(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), s2E1.DateReleased);

            #endregion

            #region Episode 5

            var s2E5 = season2.Episodes.First(e => e.Index == 5);

            Assert.Equal("Аббатство Даунтон", s2E5.Title, true);

            Assert.Equal(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), s2E5.DateReleased);

            #endregion

            #region Episode 10

            var s2E10 = season2.Episodes.First(e => e.Index == 10);

            Assert.Equal("Аббатство Даунтон", s2E10.Title, true);

            Assert.Equal(new DateTime(2012, 1, 26, 0, 0, 0, DateTimeKind.Utc), s2E10.DateReleased);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(10, season3.Episodes.Count);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            Assert.Equal("Аббатство Даунтон", s3E1.Title, true);

            Assert.Equal(new DateTime(2012, 9, 21, 0, 0, 0, DateTimeKind.Utc), s3E1.DateReleased);

            #endregion

            #region Episode 5

            var s3E5 = season3.Episodes.First(e => e.Index == 5);

            Assert.Equal("Аббатство Даунтон", s3E5.Title, true);

            Assert.Equal(new DateTime(2012, 10, 28, 0, 0, 0, DateTimeKind.Utc), s3E5.DateReleased);

            #endregion

            #region Episode 10

            var s3E10 = season3.Episodes.First(e => e.Index == 10);

            Assert.Equal("Аббатство Даунтон", s3E10.Title, true);

            Assert.Equal(new DateTime(2013, 1, 5, 0, 0, 0, DateTimeKind.Utc), s3E10.DateReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(10, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Аббатство Даунтон", s4E1.Title, true);

            Assert.Equal(new DateTime(2013, 10, 2, 0, 0, 0, DateTimeKind.Utc), s4E1.DateReleased);

            #endregion

            #region Episode 5

            var s4E5 = season4.Episodes.First(e => e.Index == 5);

            Assert.Equal("Аббатство Даунтон", s4E5.Title, true);

            Assert.Equal(new DateTime(2013, 12, 3, 0, 0, 0, DateTimeKind.Utc), s4E5.DateReleased);

            #endregion

            #region Episode 10

            var s4E10 = season4.Episodes.First(e => e.Index == 10);

            Assert.Equal("Аббатство Даунтон", s4E10.Title, true);

            Assert.Equal(new DateTime(2014, 1, 18, 0, 0, 0, DateTimeKind.Utc), s4E10.DateReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(11, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Аббатство Даунтон", s5E1.Title, true);

            Assert.Equal(new DateTime(2014, 9, 28, 0, 0, 0, DateTimeKind.Utc), s5E1.DateReleased);

            #endregion

            #region Episode 5

            var s5E5 = season5.Episodes.First(e => e.Index == 5);

            Assert.Equal("Аббатство Даунтон", s5E5.Title, true);

            Assert.Equal(new DateTime(2014, 11, 20, 0, 0, 0, DateTimeKind.Utc), s5E5.DateReleased);

            #endregion

            #region Episode 11

            var s5E11 = season5.Episodes.First(e => e.Index == 11);

            Assert.Equal("Аббатство Даунтон", s5E11.Title, true);

            Assert.Equal(new DateTime(2015, 1, 8, 0, 0, 0, DateTimeKind.Utc), s5E11.DateReleased);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(10, season6.Episodes.Count);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            Assert.Equal("Аббатство Даунтон", s6E1.Title, true);

            Assert.Equal(new DateTime(2015, 10, 7, 0, 0, 0, DateTimeKind.Utc), s6E1.DateReleased);

            #endregion

            #region Episode 5

            var s6E5 = season6.Episodes.First(e => e.Index == 5);

            Assert.Equal("Аббатство Даунтон", s6E5.Title, true);

            Assert.Equal(new DateTime(2015, 12, 16, 0, 0, 0, DateTimeKind.Utc), s6E5.DateReleased);

            #endregion

            #region Episode 10

            var s6E10 = season6.Episodes.First(e => e.Index == 10);

            Assert.Equal("Аббатство Даунтон", s6E10.Title, true);

            Assert.Equal(new DateTime(2016, 4, 20, 0, 0, 0, DateTimeKind.Utc), s6E10.DateReleased);

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Arrow()
        {
            var service = new BaibakoExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("http://baibako.tv/iplayer/arrow");

            #region Tv Show

            Assert.Equal("Стрела", model.Title, true);

            Assert.NotNull(model.Seasons);
            Assert.True(model.Seasons.Count >= 5);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(23, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Стрела", s1E1.Title, true);

            Assert.Equal(new DateTime(2013, 4, 1, 0, 0, 0, DateTimeKind.Utc), s1E1.DateReleased);

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.Index == 12);

            Assert.Equal("Стрела", s1E12.Title, true);

            Assert.Equal(new DateTime(2013, 4, 12, 0, 0, 0, DateTimeKind.Utc), s1E12.DateReleased);

            #endregion

            #region Episode 23

            var s1E23 = season1.Episodes.First(e => e.Index == 23);

            Assert.Equal("Стрела", s1E23.Title, true);

            Assert.Equal(new DateTime(2013, 5, 17, 0, 0, 0, DateTimeKind.Utc), s1E23.DateReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(23, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Стрела", s2E1.Title, true);

            Assert.Equal(new DateTime(2013, 10, 11, 0, 0, 0, DateTimeKind.Utc), s2E1.DateReleased);

            #endregion

            #region Episode 12

            var s2E12 = season2.Episodes.First(e => e.Index == 12);

            Assert.Equal("Стрела", s2E12.Title, true);

            Assert.Equal(new DateTime(2014, 1, 31, 0, 0, 0, DateTimeKind.Utc), s2E12.DateReleased);

            #endregion

            #region Episode 23

            var s2E23 = season2.Episodes.First(e => e.Index == 23);

            Assert.Equal("Стрела", s2E23.Title, true);

            Assert.Equal(new DateTime(2014, 5, 16, 0, 0, 0, DateTimeKind.Utc), s2E23.DateReleased);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(23, season3.Episodes.Count);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            Assert.Equal("Стрела", s3E1.Title, true);

            Assert.Equal(new DateTime(2014, 10, 10, 0, 0, 0, DateTimeKind.Utc), s3E1.DateReleased);

            #endregion

            #region Episode 12

            var s3E12 = season3.Episodes.First(e => e.Index == 12);

            Assert.Equal("Стрела", s3E12.Title, true);

            Assert.Equal(new DateTime(2015, 2, 6, 0, 0, 0, DateTimeKind.Utc), s3E12.DateReleased);

            #endregion

            #region Episode 23

            var s3E23 = season3.Episodes.First(e => e.Index == 23);

            Assert.Equal("Стрела", s3E23.Title, true);

            Assert.Equal(new DateTime(2015, 5, 15, 0, 0, 0, DateTimeKind.Utc), s3E23.DateReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(23, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Стрела", s4E1.Title, true);

            Assert.Equal(new DateTime(2015, 10, 10, 0, 0, 0, DateTimeKind.Utc), s4E1.DateReleased);

            #endregion

            #region Episode 12

            var s4E12 = season4.Episodes.First(e => e.Index == 12);

            Assert.Equal("Стрела", s4E12.Title, true);

            Assert.Equal(new DateTime(2016, 2, 6, 0, 0, 0, DateTimeKind.Utc), s4E12.DateReleased);

            #endregion

            #region Episode 23

            var s4E23 = season4.Episodes.First(e => e.Index == 23);

            Assert.Equal("Стрела", s4E23.Title, true);

            //2016-05-27T23:38:14+00:00
            Assert.Equal(new DateTime(2016, 5, 27, 0, 0, 0, DateTimeKind.Utc), s4E23.DateReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(23, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Стрела", s5E1.Title, true);

            Assert.Equal(new DateTime(2016, 10, 7, 0, 0, 0, DateTimeKind.Utc), s5E1.DateReleased);

            #endregion

            #region Episode 12

            var s5E12 = season5.Episodes.First(e => e.Index == 12);

            Assert.Equal("Стрела", s5E12.Title, true);

            //2017-02-10T04:37:31+00:00
            Assert.Equal(new DateTime(2017, 2, 10, 0, 0, 0, DateTimeKind.Utc), s5E12.DateReleased);

            #endregion

            #region Episode 23

            var s5E23 = season5.Episodes.First(e => e.Index == 23);

            Assert.Equal("Стрела", s5E23.Title, true);

            Assert.Equal(new DateTime(2017, 5, 25, 0, 0, 0, DateTimeKind.Utc), s5E23.DateReleased);

            #endregion

            #endregion
        }

        #endregion
    }
}