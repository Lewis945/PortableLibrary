using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.Baibako
{
    public class DowntonAbbeyTests : BaibakoTestsBase
    {
        [Fact]
        public async Task Should_Extract_Downton_Abbey()
        {
            var model = await Service.ExtractTvShowAsync("http://baibako.tv/iplayer/downton-abbey");

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
    }
}
