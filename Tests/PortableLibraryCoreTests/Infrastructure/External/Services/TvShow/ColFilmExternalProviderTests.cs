using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
{
    public class ColFilmExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public ColFilmExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        #region Extract TvShow Tests

        [Fact]
        public async Task Should_Extract_Grimm()
        {
            var service = new ColdFilmExternalProvider(_retryService);

            var model = await service.FindAndExtractTvShowAsync("Гримм");

            #region Tv Show

            Assert.Equal("Гримм", model.Title, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(5, model.Seasons.Count);

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(22, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Гримм", s2E1.Title, true);

            Assert.Equal(new DateTime(2013, 4, 5), s2E1.DateReleased);

            #endregion

            #region Episode 11

            var s2E11 = season2.Episodes.First(e => e.Index == 11);

            Assert.Equal("Гримм", s2E11.Title, true);

            Assert.Equal(new DateTime(2013, 4, 5), s2E11.DateReleased);

            #endregion

            #region Episode 22

            var s2E22 = season2.Episodes.First(e => e.Index == 22);

            Assert.Equal("Гримм", s2E22.Title, true);

            Assert.Equal(new DateTime(2013, 5, 22), s2E22.DateReleased);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(22, season3.Episodes.Count);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            Assert.Equal("Гримм", s3E1.Title, true);

            Assert.Equal(new DateTime(2013, 10, 26), s3E1.DateReleased);

            #endregion

            #region Episode 11

            var s3E11 = season3.Episodes.First(e => e.Index == 11);

            Assert.Equal("Гримм", s3E11.Title, true);

            Assert.Equal(new DateTime(2014, 1, 18), s3E11.DateReleased);

            #endregion

            #region Episode 22

            var s3E22 = season3.Episodes.First(e => e.Index == 22);

            Assert.Equal("Гримм", s3E22.Title, true);

            Assert.Equal(new DateTime(2014, 5, 16), s3E22.DateReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(22, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Гримм", s4E1.Title, true);

            Assert.Equal(new DateTime(2014, 10, 25), s4E1.DateReleased);

            #endregion

            #region Episode 11

            var s4E11 = season4.Episodes.First(e => e.Index == 11);

            Assert.Equal("Гримм", s4E11.Title, true);

            Assert.Equal(new DateTime(2015, 1, 31), s4E11.DateReleased);

            #endregion

            #region Episode 22

            var s4E22 = season4.Episodes.First(e => e.Index == 22);

            Assert.Equal("Гримм", s4E22.Title, true);

            Assert.Equal(new DateTime(2015, 5, 16), s4E22.DateReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(22, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Гримм", s5E1.Title, true);

            Assert.Equal(new DateTime(2015, 10, 31), s5E1.DateReleased);

            #endregion

            #region Episode 11

            var s5E11 = season5.Episodes.First(e => e.Index == 11);

            Assert.Equal("Гримм", s5E11.Title, true);

            Assert.Equal(new DateTime(2016, 3, 5), s5E11.DateReleased);

            #endregion

            #region Episode 22

            var s5E22 = season5.Episodes.First(e => e.Index == 22);

            Assert.Equal("Гримм", s5E22.Title, true);

            Assert.Equal(new DateTime(2016, 5, 21), s5E22.DateReleased);

            #endregion

            #endregion
            
            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(13, season6.Episodes.Count);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            Assert.Equal("Гримм", s6E1.Title, true);

            Assert.Equal(new DateTime(2017, 1, 7), s6E1.DateReleased);

            #endregion

            #region Episode 6

            var s6E6 = season6.Episodes.First(e => e.Index == 6);

            Assert.Equal("Гримм", s6E6.Title, true);

            Assert.Equal(new DateTime(2017, 2, 11), s6E6.DateReleased);

            #endregion

            #region Episode 13

            var s6E13 = season6.Episodes.First(e => e.Index == 13);

            Assert.Equal("Гримм", s6E13.Title, true);

            Assert.Equal(new DateTime(2017, 4, 1), s6E13.DateReleased);

            #endregion

            #endregion
        }

        #endregion

        #region Extract TvShow Seasons Tests

        [Fact]
        public async Task Should_Extract_Agents_of_Shield_4th_Season()
        {
            var service = new ColdFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowSeasonAsync(
                "http://coldfilm.info/news/agenty_shhit_4_sezon_smotret_onlajn/1-0-435");
        }

        [Fact]
        public async Task Should_Extract_Flash_1st_Season()
        {
            var service = new ColdFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowSeasonAsync(
                "http://coldfilm.info/news/flesh_1_sezon_smotret_onlajn/1-0-89");
        }

        [Fact]
        public async Task Should_Extract_Flash_3d_Season()
        {
            var service = new ColdFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowSeasonAsync(
                "http://coldfilm.info/news/flehsh_3_sezon_smotret_onlajn/1-0-389");
        }

        [Fact]
        public async Task Should_Extract_Arrow_5th_Season()
        {
            var service = new ColdFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowSeasonAsync(
                "http://coldfilm.info/news/strela_5_sezon_smotret_onlajn/1-0-391");
        }

        #endregion
    }
}