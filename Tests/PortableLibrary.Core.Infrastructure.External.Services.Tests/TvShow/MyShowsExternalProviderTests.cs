using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow
{
    public class MyShowsExternalProviderTests : TvShowExternalProviderTestsBase
    {
        #region Fields

        private readonly MyShowsExternalProvider _englishService;
        private readonly MyShowsExternalProvider _russianService;

        #endregion

        #region .ctor

        public MyShowsExternalProviderTests()
        {
            IHttpService httpService = new HttpService();
            IRetryService retryService = new RetryService();
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<MyShowsMappingProfile>(); });
            IMapper mapper = new Mapper(config);

            _englishService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.English);
            _russianService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.Russian);
        }

        #endregion

        #region Extract Dirk Gently's Holistic Detective Agency TvShow Tests

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Id_English()
        {
            var model = await _englishService.ExtractTvShowAsync(49623);
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Id_Russian()
        {
            var model = await _russianService.ExtractTvShowAsync(49623);
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Uri()
        {
            var model = await _englishService.ExtractTvShowAsync("https://myshows.me/view/49623/");
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.English);
        }

        [Fact]
        public async Task Should_Find_Dirk_Gentlys_Holistic_Detective_Agency_By_English_Title()
        {
            string fullTitle = GetDirkGentlysHolisticDetectiveAgencyTitle(Language.English);
            var models = await _englishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(49623, model.Id);
        }

        [Fact]
        public async Task Should_Find_Dirk_Gentlys_Holistic_Detective_Agency_By_Russian_Title()
        {
            string fullTitle = GetDirkGentlysHolisticDetectiveAgencyTitle(Language.Russian);
            var models = await _russianService.FindTvShowAsync(fullTitle);
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

            Assert.True(model.Rating.HasValue);
            Assert.Equal(4.52m, model.Rating.Value, 2);

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
                        return new List<string> {"Comedy", "Sci-Fi", "Mystery"};
                    case Language.Russian:
                        return new List<string> {"Комедия", "Фантастика", "Детектив"};
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

        #region Extract Grimm TvShow Tests

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_English()
        {
            var model = await _englishService.ExtractTvShowAsync(17186);
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_Russian()
        {
            var model = await _russianService.ExtractTvShowAsync(17186);
            ValidateGrimm(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Uri()
        {
            var model = await _englishService.ExtractTvShowAsync("https://myshows.me/view/17186/");
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_English_Title()
        {
            string fullTitle = GetGrimmTitle(Language.English);
            var models = await _englishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Russian_Title()
        {
            string fullTitle = GetGrimmTitle(Language.Russian);
            var models = await _russianService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        private static string GetGrimmTitle(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "Grimm";
                case Language.Russian:
                    return "Гримм";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

        private void ValidateGrimm(TvShowDataExtractionModel model, Language language)
        {
            #region Tv Show

            string GetDescription()
            {
                switch (language)
                {
                    case Language.English:
                        return
                            "An ordinary homicide detective Nick Burkhardt lives a pretty simple life with his beautiful " +
                            "fiancee and makes Portland streets more safe with his partner. When his aunt dies in " +
                            "suspicious circumstances, he suddenly finds out that her old trailer is full of strange " +
                            "books and weapon... " +
                            "Trivia " +
                            "Hunters of the dark creatures are one of the most favorite themes for writers and directors" +
                            " - and one of the most threadbare. But Grimm isn't as banal as you could think. All these " +
                            "demons aren't hiding under your bed - they are managers, policemen, hairdressers.. " +
                            "They are around all the time, and Nick who discovers that his family was hunting demons " +
                            "and witches since the beginning of times has no idea what to do if his best friend is one of " +
                            "them?... He changes his gun for a sword and starts to clean his city of all the " +
                            "bad guys. Only this time most of them aren't gangsters at all.";
                    case Language.Russian:
                        return
                            "Когда обычный портлендский детектив Ник Беркхард, живущий вместе с красавицей невестой " +
                            "и потихоньку избавляющий город на пару с коллегой от преступников, получает в наследство" +
                            " от тетушки, умершей при странных обстоятельствах, старенький трейлер, он и понятия не " +
                            "имеет о том, что на полках того – вовсе не старые семейные фотоальбомы… " +
                            "В чем суть? " +
                            "Охотники на нечисть – тема, не дающая покоя ни писателям, ни сценаристам. А потому " +
                            "«Гримм» поначалу кажется едва ли не вторичным. Но монстры здесь не сидят под кроватью, " +
                            "и в том главное очарование этого сериала. Когда понимаешь, что эта самая нечисть не " +
                            "по углам прячется, а вот, вполне себе, чинит старые часы, работает в офисах, владеет " +
                            "магазинами или даже в полиции служат. А окружающие люди и понятия не имеют о том, что " +
                            "их начальники и супруги обладают вовсе не плохим характером, а вполне себе чародейскими " +
                            "замашками. Главный герой Ник, однажды узнавший о том, что является потомком Гриммов - " +
                            "охотников, безжалостно расправлявшихся с оборотнями и ведьмами многие века, меняет " +
                            "пистолет на арбалет и начинает расправляться с «плохими парнями» рангом повыше, " +
                            "чем местные гангстеры.";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal(17186, model.Id);

            ValidateTvShow(model, title: GetGrimmTitle(language),
                originalTitle: GetGrimmTitle(Language.English),
                imageUri: "https://media.myshows.me/shows/normal/c/c2/c27a40b5d158fffc0e8096d29cc1df01.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: GetGenres().ToList(), description: GetDescription(),
                seasonsCount: 6);

            Assert.Equal("US", model.Country, true);

            Assert.True(model.Year.HasValue);
            Assert.Equal(2011, model.Year.Value);

            Assert.True(model.KinopoiskId.HasValue);
            Assert.Equal(582314, model.KinopoiskId.Value);

            Assert.True(model.TvrageId.HasValue);
            Assert.Equal(28352, model.TvrageId.Value);

            Assert.True(model.Runtime.HasValue);
            Assert.Equal(43, model.Runtime.Value);

            Assert.True(model.ImdbId.HasValue);
            Assert.Equal(1830617, model.ImdbId.Value);

            Assert.True(model.Rating.HasValue);
            Assert.Equal(4.05m, model.Rating.Value, 2);

            Assert.True(model.Started.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2011, 10, 28, 0, 0, 0, DateTimeKind.Utc)),
                model.Started.Value);

            Assert.True(model.Ended.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2017, 3, 31, 0, 0, 0, DateTimeKind.Utc)), model.Ended.Value);

            IEnumerable<string> GetGenres()
            {
                switch (language)
                {
                    case Language.English:
                        return new List<string> {"Drama", "Crime", "Supernatural"};
                    case Language.Russian:
                        return new List<string> {"Драма", "Криминал", "Сверхъестественное"};
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 22);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, id: 1079861, title: "Pilot", originalTitle: "Pilot", shortName: "s01e01",
                image: "https://media.myshows.me/episodes/normal/a/dd/add6e0e8eb00d31e8c27646e282da7e4.jpg",
                dateReleasedOrigianl: new DateTime(2011, 10, 29, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 11

            var s1E11 = season1.Episodes.First(e => e.Index == 11);

            ValidateEpisode(s1E11, id: 1356548, title: "Tarantella", originalTitle: "Tarantella", shortName: "s01e11",
                image: "https://media.myshows.me/episodes/normal/3/21/321b16834cca69227eda3c85c5411dab.jpg",
                dateReleasedOrigianl: new DateTime(2012, 2, 11, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 22

            var s1E22 = season1.Episodes.First(e => e.Index == 22);

            ValidateEpisode(s1E22, id: 1454628, title: "Woman in Black", originalTitle: "Woman in Black",
                shortName: "s01e22",
                image: "https://media.myshows.me/episodes/normal/2/bc/2bc08974b48d72794c44898f9d9af6e3.jpg",
                dateReleasedOrigianl: new DateTime(2012, 5, 19, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 22, 1);

            #region Special 1

            var s2S1 = season2.Specials.First(e => e.ShortName == "s02 special-1");

            ValidateEpisode(s2S1, id: 2097143, title: "Bad Hair Day", originalTitle: "Bad Hair Day",
                shortName: "s02 special-1",
                dateReleasedOrigianl: new DateTime(2013, 1, 17, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, id: 1488480, title: "Bad Teeth", originalTitle: "Bad Teeth", shortName: "s02e01",
                image: "https://media.myshows.me/episodes/normal/d/e2/de217bef1a0c8eddc9fce0c609bdf48b.jpg",
                dateReleasedOrigianl: new DateTime(2012, 8, 14, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 11

            var s2E11 = season2.Episodes.First(e => e.Index == 11);

            ValidateEpisode(s2E11, id: 1636953, title: "To Protect and Serve Man",
                originalTitle: "To Protect and Serve Man", shortName: "s02e11",
                image: "https://media.myshows.me/episodes/normal/c/2a/c2aadef981cfcc5f87a3c79f5eb93646.jpg",
                dateReleasedOrigianl: new DateTime(2012, 11, 10, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 22

            var s2E22 = season2.Episodes.First(e => e.Index == 22);

            ValidateEpisode(s2E22, id: 1770962, title: "Goodnight, Sweet Grimm",
                originalTitle: "Goodnight, Sweet Grimm", shortName: "s02e22",
                image: "https://media.myshows.me/episodes/normal/d/0c/d0ceac423a5ff915e7d1adb9c1a61e6b.jpg",
                dateReleasedOrigianl: new DateTime(2013, 5, 22, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            ValidateSeason(season3, 22, 2);

            #region Special 1

            var s3S1 = season3.Specials.First(e => e.ShortName == "s03 special-1");

            ValidateEpisode(s3S1, id: 2097144, title: "Meltdown", originalTitle: "Meltdown", shortName: "s03 special-1",
                dateReleasedOrigianl: new DateTime(2013, 10, 5, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Special 2

            var s3S2 = season3.Specials.First(e => e.ShortName == "s03 special-2");

            ValidateEpisode(s3S2, id: 2097145, title: "Love is In the Air", originalTitle: "Love is In the Air",
                shortName: "s03 special-2",
                dateReleasedOrigianl: new DateTime(2014, 1, 31, 10, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s3E1, id: 1845977, title: "The Ungrateful Dead", originalTitle: "The Ungrateful Dead",
                shortName: "s03e01",
                image: "https://media.myshows.me/episodes/normal/1/6e/16ed0d8b568a998b428d773c65e76ba1.jpg",
                dateReleasedOrigianl: new DateTime(2013, 10, 26, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 11

            var s3E11 = season3.Episodes.First(e => e.Index == 11);

            ValidateEpisode(s3E11, id: 1984662, title: "The Good Soldier", originalTitle: "The Good Soldier",
                shortName: "s03e11",
                image: "https://media.myshows.me/episodes/normal/c/3f/c3f8a1c0adc41661c37c8509be0ac66c.jpg",
                dateReleasedOrigianl: new DateTime(2014, 1, 18, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 22

            var s3E22 = season3.Episodes.First(e => e.Index == 22);

            ValidateEpisode(s3E22, id: 2131838, title: "Blond Ambition", originalTitle: "Blond Ambition",
                shortName: "s03e22",
                image: "https://media.myshows.me/episodes/normal/4/68/468e7f91b849d20ba7d6014f4d04c850.jpg",
                dateReleasedOrigianl: new DateTime(2014, 5, 17, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            ValidateSeason(season4, 22);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s4E1, id: 2206361, title: "Thanks for the Memories",
                originalTitle: "Thanks for the Memories", shortName: "s04e01",
                image: "https://media.myshows.me/episodes/normal/d/5b/d5b944e31bdba1386b07eb8ddea61f51.jpg",
                dateReleasedOrigianl: new DateTime(2014, 10, 24, 21, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 11

            var s4E11 = season4.Episodes.First(e => e.Index == 11);

            ValidateEpisode(s4E11, id: 2599027, title: "Death Do Us Part", originalTitle: "Death Do Us Part",
                shortName: "s04e11",
                image: "https://media.myshows.me/episodes/normal/f/3c/f3c86d6017fa0ee09d32c34c3a823c3a.jpg",
                dateReleasedOrigianl: new DateTime(2015, 1, 30, 23, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 22

            var s4E22 = season4.Episodes.First(e => e.Index == 22);

            ValidateEpisode(s4E22, id: 2650940, title: "Cry Havoc", originalTitle: "Cry Havoc", shortName: "s04e22",
                image: "https://media.myshows.me/episodes/normal/a/7b/a7b1e9c111c1df75537949a4914cb146.jpg",
                dateReleasedOrigianl: new DateTime(2015, 5, 15, 21, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            ValidateSeason(season5, 22);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s5E1, id: 2701637, title: "The Grimm Identity", originalTitle: "The Grimm Identity",
                shortName: "s05e01",
                image: "https://media.myshows.me/episodes/normal/5/2a/52afddb5645a39443ef710e7e536521c.jpg",
                dateReleasedOrigianl: new DateTime(2015, 10, 30, 21, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 11

            var s5E11 = season5.Episodes.First(e => e.Index == 11);

            ValidateEpisode(s5E11, id: 15478357, title: "Key Move", originalTitle: "Key Move", shortName: "s05e11",
                image: "https://media.myshows.me/episodes/normal/4/31/43162a00298f053c48f3765f936ed09a.jpg",
                dateReleasedOrigianl: new DateTime(2016, 3, 5, 2, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 22

            var s5E22 = season5.Episodes.First(e => e.Index == 22);

            ValidateEpisode(s5E22, id: 15512584, title: "Beginning of the End - Part Two",
                originalTitle: "Beginning of the End - Part Two", shortName: "s05e22",
                image: "https://media.myshows.me/episodes/normal/9/16/91675ceed70ab1f9e9a995d19c03b0d0.jpg",
                dateReleasedOrigianl: new DateTime(2016, 5, 21, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            ValidateSeason(season6, 13);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s6E1, id: 15824011, title: "Fugitive", originalTitle: "Fugitive", shortName: "s06e01",
                image: "https://media.myshows.me/episodes/normal/d/b8/db8ad9019d8ca576c5ece6cd594fb3ce.jpg",
                dateReleasedOrigianl: new DateTime(2017, 1, 7, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 6

            var s6E6 = season6.Episodes.First(e => e.Index == 6);

            ValidateEpisode(s6E6, id: 15914954, title: "Breakfast in Bed", originalTitle: "Breakfast in Bed",
                shortName: "s06e06",
                image: "https://media.myshows.me/episodes/normal/f/07/f072f764b9ed371fe47eb92ea9e93e0b.jpg",
                dateReleasedOrigianl: new DateTime(2017, 2, 11, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 13

            var s6E13 = season6.Episodes.First(e => e.Index == 13);

            ValidateEpisode(s6E13, id: 15982104, title: "The End", originalTitle: "The End", shortName: "s06e13",
                image: "https://media.myshows.me/episodes/normal/a/f8/af8215e081bc806aee9592a6a946d6f9.jpg",
                dateReleasedOrigianl: new DateTime(2017, 4, 1, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }

        #endregion

        #region Extract Friends TvShow Tests

        [Fact]
        public async Task Should_Extract_Friends_By_Id_English()
        {
            var model = await _englishService.ExtractTvShowAsync(20);
            ValidateFriends(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Id_Russian()
        {
            var model = await _russianService.ExtractTvShowAsync(20);
            ValidateFriends(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Uri()
        {
            var model = await _englishService.ExtractTvShowAsync("https://myshows.me/view/20/");
            ValidateFriends(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_English_Title()
        {
            string fullTitle = GetFriendsTitle(Language.English);
            var models = await _englishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => (m.Titles?.Contains(fullTitle) ?? false) && m.Year == 1994);

            Assert.NotNull(model);
            Assert.Equal(20, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Russian_Title()
        {
            string fullTitle = GetFriendsTitle(Language.Russian);
            var models = await _russianService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => (m.Titles?.Contains(fullTitle) ?? false) && m.Year == 1994);

            Assert.NotNull(model);
            Assert.Equal(20, model.Id);
        }

        private static string GetFriendsTitle(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "Friends";
                case Language.Russian:
                    return "Друзья";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

        private void ValidateFriends(TvShowDataExtractionModel model, Language language)
        {
            #region Tv Show

            Assert.Equal(20, model.Id);

            ValidateTvShow(model, title: GetFriendsTitle(language),
                originalTitle: GetFriendsTitle(Language.English),
                imageUri: "https://media.myshows.me/shows/normal/3/39/3902fe3a363a08eb23b02d0743a2461d.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: GetGenres().ToList(), description: GetDescription(),
                seasonsCount: 10);

            string GetDescription()
            {
                switch (language)
                {
                    case Language.English:
                        return string.Empty;
                    case Language.Russian:
                        return
                            "Культовый ситком о жизни шестерых друзей-соседей, признанный одним из лучших " +
                            "комедийных сериалов в истории. В чем суть? Шестеро друзей – Моника, Фиби, Рэйчел, Росс, " +
                            "Чендлер и Джо, - живут на Манхэттене в двух съемных квартирах и изо всех сил " +
                            "пытаются жить нормальной жизнью: ссорятся, мирятся, влюбляются, выясняют отношения… " +
                            "Сложно найти человека, который не видел ни одной серии «Друзей» - этот сериал " +
                            "действительно положил начало целому жанру и до сих пор многими считается одним из " +
                            "самых любимых. Действие происходит с середины девяностых по середину двухтысячных. " +
                            "Несмотря на то, что некоторые сюжетные линии кажутся абсурдными, а поведение героев " +
                            "далеко не всегда хотя бы отдаленно похоже на поступки нормальных людей, это отличное " +
                            "зеркало целой эпохи, созданное с любовью и актуальное до сих пор.";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal("US", model.Country, true);

            Assert.True(model.Year.HasValue);
            Assert.Equal(1994, model.Year.Value);

            Assert.True(model.KinopoiskId.HasValue);
            Assert.Equal(77044, model.KinopoiskId.Value);

            Assert.True(model.TvrageId.HasValue);
            Assert.Equal(3616, model.TvrageId.Value);

            Assert.True(model.Runtime.HasValue);
            Assert.Equal(22, model.Runtime.Value);

            Assert.True(model.ImdbId.HasValue);
            Assert.Equal(108778, model.ImdbId.Value);

            Assert.True(model.Rating.HasValue);
            Assert.Equal(4.75m, model.Rating.Value, 2);

            Assert.True(model.Started.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(1994, 9, 22, 0, 0, 0, DateTimeKind.Utc)),
                model.Started.Value);

            Assert.True(model.Ended.HasValue);
            Assert.Equal(new DateTimeOffset(new DateTime(2004, 5, 6, 0, 0, 0, DateTimeKind.Utc)), model.Ended.Value);

            IEnumerable<string> GetGenres()
            {
                switch (language)
                {
                    case Language.English:
                        return new List<string> {"Comedy"};
                    case Language.Russian:
                        return new List<string> {"Комедия"};
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 24);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, id: 962, title: "The One Where It All Began",
                originalTitle: "The One Where It All Began", shortName: "s01e01",
                image: "https://media.myshows.me/episodes/normal/b/a9/ba9bd5398ae5324c86a0130f4ccdf9a0.jpg",
                dateReleasedOrigianl: new DateTime(1994, 9, 23, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s1E12, id: 973, title: "The One With the Dozen Lasagnas",
                originalTitle: "The One With the Dozen Lasagnas", shortName: "s01e12",
                image: "https://media.myshows.me/episodes/normal/a/ec/aec6ba3728af059d22daaeb75ee6d884.jpg",
                dateReleasedOrigianl: new DateTime(1995, 1, 13, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s1E24 = season1.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s1E24, id: 985, title: "The One Where Rachel Finds Out",
                originalTitle: "The One Where Rachel Finds Out", shortName: "s01e24",
                image: "https://media.myshows.me/episodes/normal/b/04/b04a1e0f1194c97464859cc9c768eb98.jpg",
                dateReleasedOrigianl: new DateTime(1995, 5, 19, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 24);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, id: 986, title: "The One With Ross's New Girlfriend",
                originalTitle: "The One With Ross's New Girlfriend", shortName: "s02e01",
                image: "https://media.myshows.me/episodes/normal/d/c7/dc7c9160361216bd3f19fa9894cde632.jpg",
                dateReleasedOrigianl: new DateTime(1995, 9, 22, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s2E12 = season2.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s2E12, id: 997, title: "The One After the Superbowl (1)",
                originalTitle: "The One After the Superbowl (1)", shortName: "s02e12",
                image: "https://media.myshows.me/episodes/normal/0/db/0dbfb16558a6232cf0d75449a69c4b43.jpg",
                dateReleasedOrigianl: new DateTime(1996, 1, 29, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s2E24 = season2.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s2E24, id: 1009, title: "The One With Barry and Mindy's Wedding",
                originalTitle: "The One With Barry and Mindy's Wedding", shortName: "s02e24",
                image: "https://media.myshows.me/episodes/normal/e/9d/e9d6818b784d4e9f35061e539239748b.jpg",
                dateReleasedOrigianl: new DateTime(1996, 5, 17, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            ValidateSeason(season3, 25);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s3E1, id: 1010, title: "The One With the Princess Leia Fantasy",
                originalTitle: "The One With the Princess Leia Fantasy", shortName: "s03e01",
                image: "https://media.myshows.me/episodes/normal/0/ea/0ea28360eeaa51d7be680a9a50ad58b9.jpg",
                dateReleasedOrigianl: new DateTime(1996, 9, 17, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s3E12 = season3.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s3E12, id: 1021, title: "The One With All the Jealousy",
                originalTitle: "The One With All the Jealousy", shortName: "s03e12",
                image: "https://media.myshows.me/episodes/normal/2/5f/25f88762e12ad5510c11a7badd9138df.jpg",
                dateReleasedOrigianl: new DateTime(1997, 1, 17, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 25

            var s3E25 = season3.Episodes.First(e => e.Index == 25);

            ValidateEpisode(s3E25, id: 1034, title: "The One at the Beach",
                originalTitle: "The One at the Beach", shortName: "s03e25",
                image: "https://media.myshows.me/episodes/normal/4/c3/4c3051486e49ebae6c88d5ce907b84b7.jpg",
                dateReleasedOrigianl: new DateTime(1997, 5, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            ValidateSeason(season4, 24);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s4E1, id: 1035, title: "The One With the Jellyfish",
                originalTitle: "The One With the Jellyfish", shortName: "s04e01",
                image: "https://media.myshows.me/episodes/normal/e/af/eafa78de0610ec8475744262e07a429d.jpg",
                dateReleasedOrigianl: new DateTime(1997, 9, 26, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s4E12 = season4.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s4E12, id: 1046, title: "The One With the Embryos",
                originalTitle: "The One With the Embryos", shortName: "s04e12",
                dateReleasedOrigianl: new DateTime(1998, 1, 16, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s4E24 = season4.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s4E24, id: 1058, title: "The One With Ross's Wedding (2)",
                originalTitle: "The One With Ross's Wedding (2)", shortName: "s04e24",
                dateReleasedOrigianl: new DateTime(1998, 5, 8, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            ValidateSeason(season5, 24);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s5E1, id: 1059, title: "The One After Ross Says Rachel",
                originalTitle: "The One After Ross Says Rachel", shortName: "s05e01",
                dateReleasedOrigianl: new DateTime(1998, 9, 25, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s5E12 = season5.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s5E12, id: 1070, title: "The One With Chandler's Work Laugh",
                originalTitle: "The One With Chandler's Work Laugh", shortName: "s05e12",
                dateReleasedOrigianl: new DateTime(1999, 1, 22, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s5E24 = season5.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s5E24, id: 1082, title: "The One in Vegas (2)",
                originalTitle: "The One in Vegas (2)", shortName: "s05e24",
                dateReleasedOrigianl: new DateTime(1999, 5, 21, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            ValidateSeason(season6, 25);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s6E1, id: 1083, title: "The One After Vegas",
                originalTitle: "The One After Vegas", shortName: "s06e01",
                dateReleasedOrigianl: new DateTime(1999, 9, 24, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s6E12 = season6.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s6E12, id: 1094, title: "The One With the Joke",
                originalTitle: "The One With the Joke", shortName: "s06e12",
                dateReleasedOrigianl: new DateTime(2000, 1, 14, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 25

            var s6E25 = season6.Episodes.First(e => e.Index == 25);

            ValidateEpisode(s6E25, id: 1107, title: "The One With the Proposal (2)",
                originalTitle: "The One With the Proposal (2)", shortName: "s06e25",
                dateReleasedOrigianl: new DateTime(2000, 5, 19, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);

            ValidateSeason(season7, 24, 1);

            #region Episode 1

            var s7E1 = season7.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s7E1, id: 1108, title: "The One With Monica's Thunder",
                originalTitle: "The One With Monica's Thunder", shortName: "s07e01",
                image: "https://media.myshows.me/episodes/normal/0/95/0959d895d4a6b97625ff10c655940302.jpg",
                dateReleasedOrigianl: new DateTime(2000, 10, 13, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s7E12 = season7.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s7E12, id: 1119, title: "The One Where They're Up All Night",
                originalTitle: "The One Where They're Up All Night", shortName: "s07e12",
                dateReleasedOrigianl: new DateTime(2001, 1, 12, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s7E24 = season7.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s7E24, id: 1131, title: "The One with Monica and Chandler's Wedding (2)",
                originalTitle: "The One with Monica and Chandler's Wedding (2)",
                shortName: "s07e24",
                dateReleasedOrigianl: new DateTime(2001, 5, 18, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);

            ValidateSeason(season8, 24);

            #region Episode 1

            var s8E1 = season8.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s8E1, id: 1132, title: "The One After \"I Do\"",
                originalTitle: "The One After \"I Do\"", shortName: "s08e01",
                dateReleasedOrigianl: new DateTime(2001, 9, 28, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s8E12 = season8.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s8E12, id: 1143, title: "The One Where Joey Dates Rachel",
                originalTitle: "The One Where Joey Dates Rachel", shortName: "s08e12",
                dateReleasedOrigianl: new DateTime(2002, 1, 11, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s8E24 = season8.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s8E24, id: 1155, title: "The One Where Rachel Has a Baby (2)",
                originalTitle: "The One Where Rachel Has a Baby (2)", shortName: "s08e24",
                dateReleasedOrigianl: new DateTime(2002, 5, 17, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 9

            var season9 = model.Seasons.First(s => s.Index == 9);

            ValidateSeason(season9, 24);

            #region Episode 1

            var s9E1 = season9.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s9E1, id: 1156, title: "The One Where No One Proposes",
                originalTitle: "The One Where No One Proposes", shortName: "s09e01",
                dateReleasedOrigianl: new DateTime(2002, 9, 27, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s9E12 = season9.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s9E12, id: 1167, title: "The One With Phoebe's Rats",
                originalTitle: "The One With Phoebe's Rats", shortName: "s09e12",
                dateReleasedOrigianl: new DateTime(2003, 1, 17, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s9E24 = season9.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s9E24, id: 1179, title: "The One in Barbados (2)",
                originalTitle: "The One in Barbados (2)", shortName: "s09e24",
                dateReleasedOrigianl: new DateTime(2003, 5, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 10

            var season10 = model.Seasons.First(s => s.Index == 10);

            ValidateSeason(season10, 18, 3);

            #region Episode 1

            var s10E1 = season10.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s10E1, id: 1180, title: "The One After Joey And Rachel Kiss",
                originalTitle: "The One After Joey And Rachel Kiss", shortName: "s10e01",
                dateReleasedOrigianl: new DateTime(2003, 9, 26, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 9

            var s10E9 = season10.Episodes.First(e => e.Index == 9);

            ValidateEpisode(s10E9, id: 1188, title: "The One With the Birth Mother",
                originalTitle: "The One With the Birth Mother", shortName: "s10e09",
                dateReleasedOrigianl: new DateTime(2004, 1, 9, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 18

            var s10E18 = season10.Episodes.First(e => e.Index == 18);

            ValidateEpisode(s10E18, id: 1197, title: "The Last One (2)",
                originalTitle: "The Last One (2)", shortName: "s10e18",
                dateReleasedOrigianl: new DateTime(2004, 5, 7, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Special 3 FRIENDS REUNION - Tribute To Director James Burrows

            var s10S3 = season10.Specials.First(e => e.ShortName == "s10 special-3");

            ValidateEpisode(s10S3, id: 15668758, title: "FRIENDS REUNION - Tribute To Director James Burrows",
                originalTitle: "FRIENDS REUNION - Tribute To Director James Burrows",
                shortName: "s10 special-3",
                image: "https://media.myshows.me/episodes/normal/6/4b/64b85d4cc7efeb5fbd2ac6452b34b4cc.jpg",
                dateReleasedOrigianl: new DateTime(2016, 2, 22, 1, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }

        #endregion

        #region Extract Anger Managment TvShow Tests

        [Fact]
        public async Task Should_Extract_Anger_Managment_Id_English()
        {
            var model = await _englishService.ExtractTvShowAsync(23992);
            ValidateAngerManagment(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_Id_Russian()
        {
            var model = await _russianService.ExtractTvShowAsync(23992);
            ValidateAngerManagment(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_Uri()
        {
            var model = await _englishService.ExtractTvShowAsync("https://myshows.me/view/23992/");
            ValidateAngerManagment(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_English_Title()
        {
            string fullTitle = GetAngerManagmentTitle(Language.English);
            var models = await _englishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(23992, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_Russian_Title()
        {
            string fullTitle = GetAngerManagmentTitle(Language.Russian);
            var models = await _russianService.FindTvShowAsync(fullTitle);
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
                        return new List<string> {"Comedy"};
                    case Language.Russian:
                        return new List<string> {"Комедия"};
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

        #region Extract Steins;Gate Anime Show Tests

        [Fact]
        public async Task Should_Extract_Steins_Gate_Id_English()
        {
            var model = await _englishService.ExtractTvShowAsync(15897);
            ValidateSteinsGate(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_Id_Russian()
        {
            var model = await _russianService.ExtractTvShowAsync(15897);
            ValidateSteinsGate(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_Uri()
        {
            var model = await _englishService.ExtractTvShowAsync("https://myshows.me/view/15897/");
            ValidateSteinsGate(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_English_Title()
        {
            string fullTitle = GetSteinsGateTitle(Language.English);
            var models = await _englishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(15897, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_Russian_Title()
        {
            string fullTitle = GetSteinsGateTitle(Language.Russian);
            var models = await _russianService.FindTvShowAsync(fullTitle);
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
            Assert.Equal(new List<string> {GetSteinsGateTitle(language)}, model.Titles);
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
                        return new List<string> {"Drama", "Sci-Fi", "Fantasy", "Anime"};
                    case Language.Russian:
                        return new List<string> {"Драма", "Фантастика", "Фэнтези", "Аниме"};
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal(GetGenres(), model.Genres);

            Assert.NotNull(model.Seasons);
            Assert.Equal(1, model.TotalSeasons);
            Assert.Equal(1, model.Seasons.Count);

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