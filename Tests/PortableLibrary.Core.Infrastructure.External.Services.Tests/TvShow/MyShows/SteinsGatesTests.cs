using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.MyShows
{
    public class SteinsGatesTests : MyShowsTestsBase
    {
        #region Extract Steins;Gate Anime Show Tests

        [Fact]
        public async Task Should_Extract_Steins_Gate_Id_English()
        {
            var model = await EnglishService.ExtractTvShowAsync(15897);
            ValidateSteinsGate(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_Id_Russian()
        {
            var model = await RussianService.ExtractTvShowAsync(15897);
            ValidateSteinsGate(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_Uri()
        {
            var model = await EnglishService.ExtractTvShowAsync("https://myshows.me/view/15897/");
            ValidateSteinsGate(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_English_Title()
        {
            string fullTitle = GetSteinsGateTitle(Language.English);
            var models = await EnglishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(15897, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_Russian_Title()
        {
            string fullTitle = GetSteinsGateTitle(Language.Russian);
            var models = await RussianService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(15897, model.Id);
        }

        private static string GetSteinsGateTitle(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "Steins;Gate";
                case Language.Russian:
                    return "Врата Штейна";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

        private void ValidateSteinsGate(TvShowDataExtractionModel model, Language language)
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
                            "Акихабара &#151; интереснейшее место, где обитают самые разные люди &#151; от слегка " +
                            "сдвинутых по фазе до больных на всю голову. Именно такая компания собралась в " +
                            "«Лаборатории проблем времени», что над лавкой старых телевизоров. Возглавляет " +
                            "ее «настоящий буйный», 18-летний Ринтаро Окабэ, сумасшедший ученый и борец с " +
                            "мировым заговором. В серьезном деле нельзя без хакера &#151; вот и он, Итару " +
                            "Хасида, конечно же, толстяк и истинный отаку. Добрая фея лаборатории &#151; " +
                            "Маюри Сиина, подруга детства Ринтаро, официантка мейд-кафе и фанатка косплея, " +
                            "а научную мощь бригады резко повысила юный гений Курису Макисэ, стосковавшаяся " +
                            "по интеллектуальным приключениям и простому человеческому теплу.Конечно же, " +
                            "ребятам удалось построить машину времени из микроволновки и барахла с ближайшей " +
                            "распродажи. Великие открытия делают любители &#151; профессионалы строят «Титаники». " +
                            "Вот только потом началось такое, что «парадокс дедушки» нервно удалился курить в " +
                            "сторонку, а главный герой трижды проклял тот день, когда сдуру открыл «врата Штейна». " +
                            "Он был готов рисковать своей жизнью, но не чужими&#133; Впрочем, сделанного, как " +
                            "известно, не воротишь. Или&#133; все же можно?";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal(15897, model.Id);

            Assert.NotNull(model.Titles);
            Assert.Equal(new List<string> { GetSteinsGateTitle(language) }, model.Titles);
            Assert.Equal(GetSteinsGateTitle(Language.English), model.TitleOriginal, true);

            Assert.Equal(GetDescription().ClearString(), model.Description.ClearString(), true);

            Assert.Equal("JP", model.Country, true);

            Assert.Equal("https://media.myshows.me/shows/normal/1/1b/1ba31529a26258159a5f2b6de7351a65.jpg",
                model.ImageUri,
                true);

            Assert.True(model.Year.HasValue);
            Assert.Equal(2011, model.Year.Value);

            Assert.True(model.KinopoiskId.HasValue);
            Assert.Equal(586251, model.KinopoiskId.Value);

            Assert.True(model.TvrageId.HasValue);
            Assert.Equal(28009, model.TvrageId.Value);

            Assert.True(model.Runtime.HasValue);
            Assert.Equal(25, model.Runtime.Value);

            Assert.True(model.ImdbId.HasValue);
            Assert.Equal(1910272, model.ImdbId.Value);

            Assert.True(model.Rating.HasValue);
            Assert.Equal(4.44m, model.Rating.Value, 2);

            Assert.Equal(TvShowStatus.CanceledOrEnded, model.Status);

            Assert.True(model.Started.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2011, 4, 6, 0, 0, 0, DateTimeKind.Utc)),
                model.Started.Value);

            Assert.True(model.Ended.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2015, 12, 2, 0, 0, 0, DateTimeKind.Utc)), model.Ended.Value);

            IEnumerable<string> GetGenres()
            {
                switch (language)
                {
                    case Language.English:
                        return new List<string> { "Drama", "Sci-Fi", "Fantasy", "Anime" };
                    case Language.Russian:
                        return new List<string> { "Драма", "Фантастика", "Фэнтези", "Аниме" };
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal(GetGenres(), model.Genres);

            Assert.NotNull(model.Seasons);
            Assert.Equal(1, model.TotalSeasons);
            Assert.Single(model.Seasons);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 24, 7);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, id: 1021681, title: "Prologue of the Beginning and the End",
                originalTitle: "Prologue of the Beginning and the End", shortName: "s01e01",
                image: "https://media.myshows.me/episodes/normal/9/79/979790b845c14f22cbf65676b35def02.jpg",
                dateReleasedOrigianl: new DateTime(2011, 4, 6, 3, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s1E12, id: 1100177, title: "Dogma of Static Limit", originalTitle: "Dogma of Static Limit",
                shortName: "s01e12",
                image: "https://media.myshows.me/episodes/normal/4/7c/47cc636b76ca10d8507418f4557034d7.jpg",
                dateReleasedOrigianl: new DateTime(2011, 6, 22, 3, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s1E24 = season1.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s1E24, id: 1197405, title: "The Prologue Begins With the End",
                originalTitle: "The Prologue Begins With the End", shortName: "s01e24",
                image: "https://media.myshows.me/episodes/normal/7/e3/7e33df48114661a3cd9f22388e23c104.jpg",
                dateReleasedOrigianl: new DateTime(2011, 9, 14, 3, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Special 4

            var s1S4 = season1.Specials.First(e => e.ShortName == "s01 special-4");

            ValidateEpisode(s1S4, id: 2608194, title: "Soumei Eichi no Cognitive Computing Episode 4: Meeting Chapter",
                originalTitle: "Soumei Eichi no Cognitive Computing Episode 4: Meeting Chapter",
                shortName: "s01 special-4",
                image: "https://media.myshows.me/episodes/normal/3/6b/36bec630c22324fe32c735d9a6f9c2ea.jpg",
                dateReleasedOrigianl: new DateTime(2014, 10, 22, 4, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }

        #endregion
    }
}
