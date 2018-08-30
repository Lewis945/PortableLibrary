using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.ColdFilm;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.ColdFilm
{
    public class AgentsOfShieldTests : ColdFilmTestsBase
    {
        #region Extract TvShow Seasons Tests

        [Fact]
        public async Task Should_Extract_Agents_of_Shield_4th_Season()
        {
            var model = await Service.ExtractTvShowSeasonAsync(
                "http://coldfilm.cc/news/agenty_shhit_4_sezon_smotret_onlajn/1-0-435");

            #region Season 4

            Assert.NotNull(model.Episodes);
            Assert.Equal(22, model.Episodes.Count);

            #region Episode 1

            var s4E1 = model.Episodes.First(e => e.Index == 1);

            Assert.Equal("Агенты ЩИТ", s4E1.Title, true);

            Assert.Equal(new DateTime(2016, 9, 21), s4E1.DateReleased);

            #endregion

            #region Episode 11

            var s4E11 = model.Episodes.First(e => e.Index == 11);

            Assert.Equal("Агенты ЩИТ", s4E11.Title, true);

            Assert.Equal(new DateTime(2017, 1, 25), s4E11.DateReleased);

            #endregion

            #region Episode 22

            var s4E22 = model.Episodes.First(e => e.Index == 22);

            Assert.Equal("Агенты ЩИТ", s4E22.Title, true);

            Assert.Equal(new DateTime(2017, 5, 17), s4E22.DateReleased);

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Flash_1st_Season()
        {
            var model = await Service.ExtractTvShowSeasonAsync(
                "http://coldfilm.cc/news/flesh_1_sezon_smotret_onlajn/1-0-89");

            #region Season 1

            Assert.NotNull(model.Episodes);
            Assert.Equal(23, model.Episodes.Count);

            #region Episode 1

            var s1E1 = model.Episodes.First(e => e.Index == 1);

            Assert.Equal("Флэш", s1E1.Title, true);

            Assert.Equal(new DateTime(2014, 10, 7), s1E1.DateReleased);

            #endregion

            #region Episode 11

            var s1E11 = model.Episodes.First(e => e.Index == 11);

            Assert.Equal("Флэш", s1E11.Title, true);

            Assert.Equal(new DateTime(2015, 1, 28), s1E11.DateReleased);

            #endregion

            #region Episode 23

            var s1E23 = model.Episodes.First(e => e.Index == 23);

            Assert.Equal("Флэш", s1E23.Title, true);

            Assert.Equal(new DateTime(2015, 5, 20), s1E23.DateReleased);

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Flash_3d_Season()
        {
            var model = await Service.ExtractTvShowSeasonAsync(
                "http://coldfilm.cc/news/flehsh_3_sezon_smotret_onlajn/1-0-389");

            #region Season 3

            Assert.NotNull(model.Episodes);
            Assert.Equal(23, model.Episodes.Count);

            #region Episode 1

            var s3E1 = model.Episodes.First(e => e.Index == 1);

            Assert.Equal("Флэш", s3E1.Title, true);

            Assert.Equal(new DateTime(2016, 10, 5), s3E1.DateReleased);

            #endregion

            #region Episode 11

            var s3E11 = model.Episodes.First(e => e.Index == 11);

            Assert.Equal("Флэш", s3E11.Title, true);

            Assert.Equal(new DateTime(2017, 2, 1), s3E11.DateReleased);

            #endregion

            #region Episode 23

            var s3E23 = model.Episodes.First(e => e.Index == 23);

            Assert.Equal("Флэш", s3E23.Title, true);

            Assert.Equal(new DateTime(2017, 5, 24), s3E23.DateReleased);

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Arrow_5th_Season()
        {
            var model = await Service.ExtractTvShowSeasonAsync(
                "http://coldfilm.cc/news/strela_5_sezon_smotret_onlajn/1-0-391");

            #region Season 5

            Assert.NotNull(model.Episodes);
            Assert.Equal(23, model.Episodes.Count);

            #region Episode 1

            var s5E1 = model.Episodes.First(e => e.Index == 1);

            Assert.Equal("Стрела", s5E1.Title, true);

            Assert.Equal(new DateTime(2016, 10, 6), s5E1.DateReleased);

            #endregion

            #region Episode 11

            var s5E11 = model.Episodes.First(e => e.Index == 11);

            Assert.Equal("Стрела", s5E11.Title, true);

            Assert.Equal(new DateTime(2017, 2, 2), s5E11.DateReleased);

            #endregion

            #region Episode 23

            var s5E23 = model.Episodes.First(e => e.Index == 23);

            Assert.Equal("Стрела", s5E23.Title, true);

            Assert.Equal(new DateTime(2017, 5, 25), s5E23.DateReleased);

            #endregion

            #endregion
        }

        #endregion
    }
}
