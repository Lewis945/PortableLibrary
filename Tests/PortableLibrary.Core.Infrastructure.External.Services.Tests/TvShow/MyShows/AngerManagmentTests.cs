using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.MyShows
{
    public class AngerManagmentTests : MyShowsTestsBase
    {
        #region Extract Anger Managment TvShow Tests

        [Fact]
        public async Task Should_Extract_Anger_Managment_Id_English()
        {
            var model = await EnglishService.ExtractTvShowAsync(23992);
            ValidateAngerManagment(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_Id_Russian()
        {
            var model = await RussianService.ExtractTvShowAsync(23992);
            ValidateAngerManagment(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_Uri()
        {
            var model = await EnglishService.ExtractTvShowAsync("https://myshows.me/view/23992/");
            ValidateAngerManagment(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_English_Title()
        {
            string fullTitle = GetAngerManagmentTitle(Language.English);
            var models = await EnglishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(23992, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_Russian_Title()
        {
            string fullTitle = GetAngerManagmentTitle(Language.Russian);
            var models = await RussianService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(23992, model.Id);
        }

        private static string GetAngerManagmentTitle(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "Anger Management";
                case Language.Russian:
                    return "Управление гневом";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

        private void ValidateAngerManagment(TvShowDataExtractionModel model, Language language)
        {
            #region Tv Show

            string GetDescription()
            {
                switch (language)
                {
                    case Language.English:
                        return string.Empty;
                    case Language.Russian:
                        return
                            "До своей карьеры терапевта, Чарли был в тупике из-за отсутствия перспектив в низшей " +
                            "бейсбольной лиге. Проблема неумения контролировать свой гнев сбила его с пути в " +
                            "высшую лигу. После прохождения курса терапии по борьбе с гневом, он вывел свою команду " +
                            "в высшую лигу и провел один потрясающий сезон, прежде чем старые проблемы вновь не " +
                            "напомнили о себе. В своем финальном матче он пытался ударить себя битой по ноге, в " +
                            "результате чего получил травму, которая положила конец его карьере.Но в итоге эта же " +
                            "травма привела его к нынешней профессии. В то время как Чарли борется со своим гневом, " +
                            "в его жизни процветает хаос. Всё осложняется его отношениями с собственным терапевтом и " +
                            "лучшим другом, бывшей женой, чьи позитивные взгляды на будущее, но при этом плохой выбор " +
                            "мужчин, расстраивают Чарли и их 13-летнюю дочь, имеющую психические расстройства.";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal(23992, model.Id);

            ValidateTvShow(model, title: GetAngerManagmentTitle(language),
                originalTitle: GetAngerManagmentTitle(Language.English),
                imageUri: "https://media.myshows.me/shows/normal/3/3b/3b8013ec507437324e069d8c998f8c9c.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: GetGenres().ToList(), description: GetDescription(),
                seasonsCount: 2);

            Assert.Equal("US", model.Country, true);

            Assert.True(model.Year.HasValue);
            Assert.Equal(2012, model.Year.Value);

            Assert.True(model.KinopoiskId.HasValue);
            Assert.Equal(596247, model.KinopoiskId.Value);

            Assert.True(model.TvrageId.HasValue);
            Assert.Equal(29999, model.TvrageId.Value);

            Assert.True(model.Runtime.HasValue);
            Assert.Equal(22, model.Runtime.Value);

            Assert.True(model.ImdbId.HasValue);
            Assert.Equal(1986770, model.ImdbId.Value);

            Assert.True(model.Rating.HasValue);
            Assert.Equal(3.55m, model.Rating.Value, 2);

            Assert.True(model.Started.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2012, 6, 28, 0, 0, 0, DateTimeKind.Utc)),
                model.Started.Value);

            Assert.True(model.Ended.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2014, 12, 22, 0, 0, 0, DateTimeKind.Utc)), model.Ended.Value);

            IEnumerable<string> GetGenres()
            {
                switch (language)
                {
                    case Language.English:
                        return new List<string> { "Comedy" };
                    case Language.Russian:
                        return new List<string> { "Комедия" };
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 10);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, id: 1411218, title: "Charlie Goes Back to Therapy",
                originalTitle: "Charlie Goes Back to Therapy", shortName: "s01e01",
                image: "https://media.myshows.me/episodes/normal/a/9f/a9f9079688eb61352666e3e38157da14.jpg",
                dateReleasedOrigianl: new DateTime(2012, 6, 29, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.Index == 5);

            ValidateEpisode(s1E5, id: 1515544, title: "Charlie Tries to Prove Therapy is Legit",
                originalTitle: "Charlie Tries to Prove Therapy is Legit", shortName: "s01e05",
                image: "https://media.myshows.me/episodes/normal/6/44/644fbc577091c796fb428a1b274c169b.jpg",
                dateReleasedOrigianl: new DateTime(2012, 7, 20, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 10

            var s1E10 = season1.Episodes.First(e => e.Index == 10);

            ValidateEpisode(s1E10, id: 1515549, title: "Charlie Gets Romantic", originalTitle: "Charlie Gets Romantic",
                shortName: "s01e10",
                image: "https://media.myshows.me/episodes/normal/8/aa/8aa9a6917e6bb7fa8389ccb720881bba.jpg",
                dateReleasedOrigianl: new DateTime(2012, 8, 24, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 90);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, id: 1579536, title: "Charlie Loses It at a Baby Shower",
                originalTitle: "Charlie Loses It at a Baby Shower", shortName: "s02e01",
                image: "https://media.myshows.me/episodes/normal/b/4e/b4ebede2195719823f18395ba56ed645.jpg",
                dateReleasedOrigianl: new DateTime(2013, 1, 18, 3, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 45

            var s2E45 = season2.Episodes.First(e => e.Index == 45);

            ValidateEpisode(s2E45, id: 1979110, title: "Charlie and Lacey Shack Up",
                originalTitle: "Charlie and Lacey Shack Up", shortName: "s02e45",
                dateReleasedOrigianl: new DateTime(2013, 12, 13, 2, 30, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 90

            var s2E90 = season2.Episodes.First(e => e.Index == 90);

            ValidateEpisode(s2E90, id: 2459243, title: "Charlie and the 100th Episode",
                originalTitle: "Charlie and the 100th Episode", shortName: "s02e90",
                dateReleasedOrigianl: new DateTime(2014, 12, 23, 3, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }

        #endregion
    }
}
