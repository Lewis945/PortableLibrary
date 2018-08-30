using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.Baibako
{
    public class ArrowTests : BaibakoTestsBase
    {
        [Fact]
        public async Task Should_Extract_Arrow()
        {
            var model = await Service.ExtractTvShowAsync("http://baibako.tv/iplayer/arrow");

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
    }
}
