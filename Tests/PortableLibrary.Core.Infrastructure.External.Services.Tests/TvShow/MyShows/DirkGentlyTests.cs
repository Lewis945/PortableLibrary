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
    public class DirkGentlyTests : MyShowsTestsBase
    {
        #region Extract Dirk Gently's Holistic Detective Agency TvShow Tests

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Id_English()
        {
            var model = await EnglishService.ExtractTvShowAsync(49623);
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Id_Russian()
        {
            var model = await RussianService.ExtractTvShowAsync(49623);
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Uri()
        {
            var model = await EnglishService.ExtractTvShowAsync("https://myshows.me/view/49623/");
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.English);
        }

        [Fact]
        public async Task Should_Find_Dirk_Gentlys_Holistic_Detective_Agency_By_English_Title()
        {
            string fullTitle = GetDirkGentlysHolisticDetectiveAgencyTitle(Language.English);
            var models = await EnglishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(49623, model.Id);
        }

        [Fact]
        public async Task Should_Find_Dirk_Gentlys_Holistic_Detective_Agency_By_Russian_Title()
        {
            string fullTitle = GetDirkGentlysHolisticDetectiveAgencyTitle(Language.Russian);
            var models = await RussianService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(49623, model.Id);
        }

        private static string GetDirkGentlysHolisticDetectiveAgencyTitle(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "Dirk Gently's Holistic Detective Agency";
                case Language.Russian:
                    return "Холистическое детективное агентство Дирка Джентли";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

        private void ValidateDirkGentlysHolisticDetectiveAgency(TvShowDataExtractionModel model, Language language)
        {
            #region Tv Show

            Assert.Equal(49623, model.Id);

            ValidateTvShow(model, title: GetDirkGentlysHolisticDetectiveAgencyTitle(language),
                originalTitle: GetDirkGentlysHolisticDetectiveAgencyTitle(Language.English),
                imageUri: "https://media.myshows.me/shows/normal/9/9b/9b214e17391f62bd8e4d85df4e6b0b5a.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: GetGenres().ToList(), description: GetDescription(),
                seasonsCount: 2);

            string GetDescription()
            {
                switch (language)
                {
                    case Language.English:
                        return string.Empty;
                    case Language.Russian:
                        return
                            "Сериал, рассказывающий о приключениях частного детектива Джентли и его невольного " +
                            "помощника Тодда. Каждый сезон они расследуют некую большую, казалось бы, совершенно " +
                            "безумную тайну и сталкиваются со странными и временами опасными персонажами. " +
                            "В каждой серии они приближаются к разгадке на несколько шагов, хотя их " +
                            "расследование страдает от недостатка логики.";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal("US", model.Country, true);

            Assert.True(model.Year.HasValue);
            Assert.Equal(2016, model.Year.Value);

            Assert.True(model.KinopoiskId.HasValue);
            Assert.Equal(968767, model.KinopoiskId.Value);

            Assert.True(model.TvrageId.HasValue);
            Assert.Equal(11405, model.TvrageId.Value);

            Assert.True(model.Runtime.HasValue);
            Assert.Equal(43, model.Runtime.Value);

            Assert.True(model.ImdbId.HasValue);
            Assert.Equal(4047038, model.ImdbId.Value);

            Assert.True(model.Started.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2016, 10, 22, 0, 0, 0, DateTimeKind.Utc)),
                model.Started.Value);

            Assert.True(model.Ended.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2017, 12, 16, 0, 0, 0, DateTimeKind.Utc)), model.Ended.Value);

            IEnumerable<string> GetGenres()
            {
                switch (language)
                {
                    case Language.English:
                        return new List<string> { "Comedy", "Sci-Fi", "Mystery" };
                    case Language.Russian:
                        return new List<string> { "Комедия", "Фантастика", "Детектив" };
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, episodesCount: 8);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, id: 15803466, title: "Horizons", originalTitle: "Horizons", shortName: "s01e01",
                image: "https://media.myshows.me/episodes/normal/1/f2/1f24f4f38bc01a8f98d11dd7e7a01c53.jpg",
                dateReleasedOrigianl: new DateTime(2016, 10, 22, 16, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 4

            var s1E4 = season1.Episodes.First(e => e.Index == 4);

            ValidateEpisode(s1E4, id: 15807716, title: "Watkin", originalTitle: "Watkin", shortName: "s01e04",
                image: "https://media.myshows.me/episodes/normal/2/42/242eb236a3b60e12adb592bbf6e04039.jpg",
                dateReleasedOrigianl: new DateTime(2016, 11, 12, 17, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 8

            var s1E8 = season1.Episodes.First(e => e.Index == 8);

            ValidateEpisode(s1E8, id: 15807720, title: "Two Sane Guys Doing Normal Things",
                originalTitle: "Two Sane Guys Doing Normal Things", shortName: "s01e08",
                image: "https://media.myshows.me/episodes/normal/d/34/d3464a7e1a065e107b3aa15cc358fff8.jpg",
                dateReleasedOrigianl: new DateTime(2016, 12, 10, 17, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, episodesCount: 10);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, id: 16232262, title: "Space Rabbit", originalTitle: "Space Rabbit",
                shortName: "s02e01",
                image: "https://media.myshows.me/episodes/normal/2/eb/2eb7af3e8251798935d8aac58b9db8c2.jpg",
                dateReleasedOrigianl: new DateTime(2017, 10, 15, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 5

            var s2E5 = season2.Episodes.First(e => e.Index == 5);

            ValidateEpisode(s2E5, id: 16296021, title: "Shapes and Colors", originalTitle: "Shapes and Colors",
                shortName: "s02e05",
                image: "https://media.myshows.me/episodes/normal/7/a6/7a6fcfa4e85eb0a9abd39e8765f25cc3.jpg",
                dateReleasedOrigianl: new DateTime(2017, 11, 12, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 10

            var s2E10 = season2.Episodes.First(e => e.Index == 10);

            ValidateEpisode(s2E10, id: 16322322, title: "Nice Jacket", originalTitle: "Nice Jacket",
                shortName: "s02e10",
                image: "https://media.myshows.me/episodes/normal/9/40/9404c94125cd0625fe77ca49477c06b4.jpg",
                dateReleasedOrigianl: new DateTime(2017, 12, 17, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }

        #endregion
    }
}
