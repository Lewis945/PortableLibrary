using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow
{
    public class MyShowsExternalProviderTests
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
            var model = await _englishService.GetTvShowByIdAsync(49623);
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Id_Russian()
        {
            var model = await _russianService.GetTvShowByIdAsync(49623);
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Uri()
        {
            var model = await _englishService.GetTvShowByUriAsync("https://myshows.me/view/49623/");
            ValidateDirkGentlysHolisticDetectiveAgency(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_English_Title()
        {
            string fullTitle = GetDirkGentlysHolisticDetectiveAgencyTitle(Language.English);
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(49623, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency_By_Russian_Title()
        {
            string fullTitle = GetDirkGentlysHolisticDetectiveAgencyTitle(Language.Russian);
            var models = await _russianService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

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

        private void ValidateDirkGentlysHolisticDetectiveAgency(MyShowsTvShowModel model, Language language)
        {
            #region Tv Show

            Assert.Equal(49623, model.Id);

            Assert.Equal(GetDirkGentlysHolisticDetectiveAgencyTitle(language), model.Title, true);
            Assert.Equal(GetDirkGentlysHolisticDetectiveAgencyTitle(Language.English), model.TitleOriginal, true);

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

            Assert.Equal(GetDescription().ClearString(), model.Description.ClearString(), true);

            Assert.Equal("US", model.Country, true);

            Assert.Equal("https://media.myshows.me/shows/normal/9/9b/9b214e17391f62bd8e4d85df4e6b0b5a.jpg", model.Image,
                true);

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

            Assert.Equal(TvShowStatus.CanceledOrEnded, model.Status);

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

            Assert.Equal(GetGenres(), model.Genres);

            Assert.NotNull(model.Seasons);
            Assert.Equal(2, model.TotalSeasons);
            Assert.Equal(2, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(8, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(15803466, s1E1.Id);

            Assert.Equal("Horizons", s1E1.Title, true);
            Assert.Equal("s01e01", s1E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/1/f2/1f24f4f38bc01a8f98d11dd7e7a01c53.jpg",
                s1E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2016, 10, 22, 16, 0, 0, DateTimeKind.Utc)), s1E1.AirDate);

            #endregion

            #region Episode 4

            var s1E4 = season1.Episodes.First(e => e.EpisodeNumber == 4);

            Assert.Equal(15807716, s1E4.Id);

            Assert.Equal("Watkin", s1E4.Title, true);
            Assert.Equal("s01e04", s1E4.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/2/42/242eb236a3b60e12adb592bbf6e04039.jpg",
                s1E4.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2016, 11, 12, 17, 0, 0, DateTimeKind.Utc)), s1E4.AirDate);

            #endregion

            #region Episode 8

            var s1E8 = season1.Episodes.First(e => e.EpisodeNumber == 8);

            Assert.Equal(15807720, s1E8.Id);

            Assert.Equal("Two Sane Guys Doing Normal Things", s1E8.Title, true);
            Assert.Equal("s01e08", s1E8.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/d/34/d3464a7e1a065e107b3aa15cc358fff8.jpg",
                s1E8.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2016, 12, 10, 17, 0, 0, DateTimeKind.Utc)), s1E8.AirDate);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(10, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(16232262, s2E1.Id);

            Assert.Equal("Space Rabbit", s2E1.Title, true);
            Assert.Equal("s02e01", s2E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/2/eb/2eb7af3e8251798935d8aac58b9db8c2.jpg",
                s2E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2017, 10, 15, 1, 0, 0, DateTimeKind.Utc)), s2E1.AirDate);

            #endregion

            #region Episode 5

            var s2E5 = season2.Episodes.First(e => e.EpisodeNumber == 5);

            Assert.Equal(16296021, s2E5.Id);

            Assert.Equal("Shapes and Colors", s2E5.Title, true);
            Assert.Equal("s02e05", s2E5.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/7/a6/7a6fcfa4e85eb0a9abd39e8765f25cc3.jpg",
                s2E5.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2017, 11, 12, 2, 0, 0, DateTimeKind.Utc)), s2E5.AirDate);

            #endregion

            #region Episode 10

            var s2E10 = season2.Episodes.First(e => e.EpisodeNumber == 10);

            Assert.Equal(16322322, s2E10.Id);

            Assert.Equal("Nice Jacket", s2E10.Title, true);
            Assert.Equal("s02e10", s2E10.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/9/40/9404c94125cd0625fe77ca49477c06b4.jpg",
                s2E10.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2017, 12, 17, 2, 0, 0, DateTimeKind.Utc)), s2E10.AirDate);

            #endregion

            #endregion
        }

        #endregion

        #region Extract Grimm TvShow Tests

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_English()
        {
            var model = await _englishService.GetTvShowByIdAsync(17186);
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_Russian()
        {
            var model = await _russianService.GetTvShowByIdAsync(17186);
            ValidateGrimm(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Uri()
        {
            var model = await _englishService.GetTvShowByUriAsync("https://myshows.me/view/17186/");
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_English_Title()
        {
            string fullTitle = GetGrimmTitle(Language.English);
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Russian_Title()
        {
            string fullTitle = GetGrimmTitle(Language.Russian);
            var models = await _russianService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

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

        private void ValidateGrimm(MyShowsTvShowModel model, Language language)
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

            Assert.Equal(GetGrimmTitle(language), model.Title, true);
            Assert.Equal(GetGrimmTitle(Language.English), model.TitleOriginal, true);

            Assert.Equal(GetDescription(), model.Description, true);

            Assert.Equal("US", model.Country, true);

            Assert.Equal("https://media.myshows.me/shows/normal/c/c2/c27a40b5d158fffc0e8096d29cc1df01.jpg", model.Image,
                true);

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

            Assert.Equal(TvShowStatus.CanceledOrEnded, model.Status);

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

            Assert.Equal(GetGenres(), model.Genres);

            Assert.NotNull(model.Seasons);
            Assert.Equal(6, model.TotalSeasons);
            Assert.Equal(6, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(22, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1079861, s1E1.Id);

            Assert.Equal("Pilot", s1E1.Title, true);
            Assert.Equal("s01e01", s1E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/a/dd/add6e0e8eb00d31e8c27646e282da7e4.jpg",
                s1E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2011, 10, 29, 1, 0, 0, DateTimeKind.Utc)), s1E1.AirDate);

            #endregion

            #region Episode 11

            var s1E11 = season1.Episodes.First(e => e.EpisodeNumber == 11);

            Assert.Equal(1356548, s1E11.Id);

            Assert.Equal("Tarantella", s1E11.Title, true);
            Assert.Equal("s01e11", s1E11.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/3/21/321b16834cca69227eda3c85c5411dab.jpg",
                s1E11.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2012, 2, 11, 2, 0, 0, DateTimeKind.Utc)), s1E11.AirDate);

            #endregion

            #region Episode 22

            var s1E22 = season1.Episodes.First(e => e.EpisodeNumber == 22);

            Assert.Equal(1454628, s1E22.Id);

            Assert.Equal("Woman in Black", s1E22.Title, true);
            Assert.Equal("s01e22", s1E22.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/2/bc/2bc08974b48d72794c44898f9d9af6e3.jpg",
                s1E22.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2012, 5, 19, 1, 0, 0, DateTimeKind.Utc)), s1E22.AirDate);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.NotNull(season2.Specials);
            Assert.Equal(22, season2.Episodes.Count);
            Assert.Equal(1, season2.Specials.Count);

            #region Special 1

            var s2S1 = season2.Specials.First(e => e.ShortName == "s02 special-1");

            Assert.Equal(2097143, s2S1.Id);

            Assert.Equal("Bad Hair Day", s2S1.Title, true);

            Assert.Empty(s2S1.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 1, 17, 2, 0, 0, DateTimeKind.Utc)), s2S1.AirDate);

            #endregion

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1488480, s2E1.Id);

            Assert.Equal("Bad Teeth", s2E1.Title, true);
            Assert.Equal("s02e01", s2E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/d/e2/de217bef1a0c8eddc9fce0c609bdf48b.jpg",
                s2E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2012, 8, 14, 1, 0, 0, DateTimeKind.Utc)), s2E1.AirDate);

            #endregion

            #region Episode 11

            var s2E11 = season2.Episodes.First(e => e.EpisodeNumber == 11);

            Assert.Equal(1636953, s2E11.Id);

            Assert.Equal("To Protect and Serve Man", s2E11.Title, true);
            Assert.Equal("s02e11", s2E11.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/c/2a/c2aadef981cfcc5f87a3c79f5eb93646.jpg",
                s2E11.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2012, 11, 10, 2, 0, 0, DateTimeKind.Utc)), s2E11.AirDate);

            #endregion

            #region Episode 22

            var s2E22 = season2.Episodes.First(e => e.EpisodeNumber == 22);

            Assert.Equal(1770962, s2E22.Id);

            Assert.Equal("Goodnight, Sweet Grimm", s2E22.Title, true);
            Assert.Equal("s02e22", s2E22.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/d/0c/d0ceac423a5ff915e7d1adb9c1a61e6b.jpg",
                s2E22.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 5, 22, 1, 0, 0, DateTimeKind.Utc)), s2E22.AirDate);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            Assert.NotNull(season3.Episodes);
            Assert.NotNull(season3.Specials);
            Assert.Equal(22, season3.Episodes.Count);
            Assert.Equal(2, season3.Specials.Count);

            #region Special 1

            var s3S1 = season3.Specials.First(e => e.ShortName == "s03 special-1");

            Assert.Equal(2097144, s3S1.Id);

            Assert.Equal("Meltdown", s3S1.Title, true);

            Assert.Empty(s3S1.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 10, 5, 2, 0, 0, DateTimeKind.Utc)), s3S1.AirDate);

            #endregion

            #region Special 2

            var s3S2 = season3.Specials.First(e => e.ShortName == "s03 special-2");

            Assert.Equal(2097145, s3S2.Id);

            Assert.Equal("Love is In the Air", s3S2.Title, true);

            Assert.Empty(s3S2.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2014, 1, 31, 14, 0, 0, DateTimeKind.Utc)), s3S2.AirDate);

            #endregion

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1845977, s3E1.Id);

            Assert.Equal("The Ungrateful Dead", s3E1.Title, true);
            Assert.Equal("s03e01", s3E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/1/6e/16ed0d8b568a998b428d773c65e76ba1.jpg",
                s3E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 10, 26, 2, 0, 0, DateTimeKind.Utc)), s3E1.AirDate);

            #endregion

            #region Episode 11

            var s3E11 = season3.Episodes.First(e => e.EpisodeNumber == 11);

            Assert.Equal(1984662, s3E11.Id);

            Assert.Equal("The Good Soldier", s3E11.Title, true);
            Assert.Equal("s03e11", s3E11.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/c/3f/c3f8a1c0adc41661c37c8509be0ac66c.jpg",
                s3E11.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2014, 1, 18, 2, 0, 0, DateTimeKind.Utc)), s3E11.AirDate);

            #endregion

            #region Episode 22

            var s3E22 = season3.Episodes.First(e => e.EpisodeNumber == 22);

            Assert.Equal(2131838, s3E22.Id);

            Assert.Equal("Blond Ambition", s3E22.Title, true);
            Assert.Equal("s03e22", s3E22.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/4/68/468e7f91b849d20ba7d6014f4d04c850.jpg",
                s3E22.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2014, 5, 17, 1, 0, 0, DateTimeKind.Utc)), s3E22.AirDate);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(22, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(2206361, s4E1.Id);

            Assert.Equal("Thanks for the Memories", s4E1.Title, true);
            Assert.Equal("s04e01", s4E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/d/5b/d5b944e31bdba1386b07eb8ddea61f51.jpg",
                s4E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2014, 10, 24, 21, 0, 0, DateTimeKind.Utc)), s4E1.AirDate);

            #endregion

            #region Episode 11

            var s4E11 = season4.Episodes.First(e => e.EpisodeNumber == 11);

            Assert.Equal(2599027, s4E11.Id);

            Assert.Equal("Death Do Us Part", s4E11.Title, true);
            Assert.Equal("s04e11", s4E11.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/f/3c/f3c86d6017fa0ee09d32c34c3a823c3a.jpg",
                s4E11.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2015, 1, 30, 23, 0, 0, DateTimeKind.Utc)), s4E11.AirDate);

            #endregion

            #region Episode 22

            var s4E22 = season4.Episodes.First(e => e.EpisodeNumber == 22);

            Assert.Equal(2650940, s4E22.Id);

            Assert.Equal("Cry Havoc", s4E22.Title, true);
            Assert.Equal("s04e22", s4E22.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/a/7b/a7b1e9c111c1df75537949a4914cb146.jpg",
                s4E22.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2015, 5, 15, 21, 0, 0, DateTimeKind.Utc)), s4E22.AirDate);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(22, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(2701637, s5E1.Id);

            Assert.Equal("The Grimm Identity", s5E1.Title, true);
            Assert.Equal("s05e01", s5E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/5/2a/52afddb5645a39443ef710e7e536521c.jpg",
                s5E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2015, 10, 30, 21, 0, 0, DateTimeKind.Utc)), s5E1.AirDate);

            #endregion

            #region Episode 11

            var s5E11 = season5.Episodes.First(e => e.EpisodeNumber == 11);

            Assert.Equal(15478357, s5E11.Id);

            Assert.Equal("Key Move", s5E11.Title, true);
            Assert.Equal("s05e11", s5E11.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/4/31/43162a00298f053c48f3765f936ed09a.jpg",
                s5E11.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2016, 3, 5, 2, 0, 0, DateTimeKind.Utc)), s5E11.AirDate);

            #endregion

            #region Episode 22

            var s5E22 = season5.Episodes.First(e => e.EpisodeNumber == 22);

            Assert.Equal(15512584, s5E22.Id);

            Assert.Equal("Beginning of the End - Part Two", s5E22.Title, true);
            Assert.Equal("s05e22", s5E22.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/9/16/91675ceed70ab1f9e9a995d19c03b0d0.jpg",
                s5E22.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2016, 5, 21, 1, 0, 0, DateTimeKind.Utc)), s5E22.AirDate);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(13, season6.Episodes.Count);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(15824011, s6E1.Id);

            Assert.Equal("Fugitive", s6E1.Title, true);
            Assert.Equal("s06e01", s6E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/d/b8/db8ad9019d8ca576c5ece6cd594fb3ce.jpg",
                s6E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2017, 1, 7, 1, 0, 0, DateTimeKind.Utc)), s6E1.AirDate);

            #endregion

            #region Episode 6

            var s6E6 = season6.Episodes.First(e => e.EpisodeNumber == 6);

            Assert.Equal(15914954, s6E6.Id);

            Assert.Equal("Breakfast in Bed", s6E6.Title, true);
            Assert.Equal("s06e06", s6E6.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/f/07/f072f764b9ed371fe47eb92ea9e93e0b.jpg",
                s6E6.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2017, 2, 11, 1, 0, 0, DateTimeKind.Utc)), s6E6.AirDate);

            #endregion

            #region Episode 13

            var s6E13 = season6.Episodes.First(e => e.EpisodeNumber == 13);

            Assert.Equal(15982104, s6E13.Id);

            Assert.Equal("The End", s6E13.Title, true);
            Assert.Equal("s06e13", s6E13.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/a/f8/af8215e081bc806aee9592a6a946d6f9.jpg",
                s6E13.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2017, 4, 1, 0, 0, 0, DateTimeKind.Utc)), s6E13.AirDate);

            #endregion

            #endregion
        }

        #endregion

        #region Extract Friends TvShow Tests

        [Fact]
        public async Task Should_Extract_Friends_By_Id_English()
        {
            var model = await _englishService.GetTvShowByIdAsync(20);
            ValidateFriends(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Id_Russian()
        {
            var model = await _russianService.GetTvShowByIdAsync(20);
            ValidateFriends(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Uri()
        {
            var model = await _englishService.GetTvShowByUriAsync("https://myshows.me/view/20/");
            ValidateFriends(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_English_Title()
        {
            string fullTitle = GetFriendsTitle(Language.English);
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle && m.Year == 1994);

            Assert.NotNull(model);
            Assert.Equal(20, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Russian_Title()
        {
            string fullTitle = GetFriendsTitle(Language.Russian);
            var models = await _russianService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle && m.Year == 1994);

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

        private void ValidateFriends(MyShowsTvShowModel model, Language language)
        {
            #region Tv Show

            Assert.Equal(20, model.Id);

            Assert.Equal(GetFriendsTitle(language), model.Title, true);
            Assert.Equal(GetFriendsTitle(Language.English), model.TitleOriginal, true);

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

            Assert.Equal(GetDescription().ClearString(), model.Description.ClearString(), true);

            Assert.Equal("US", model.Country, true);

            Assert.Equal("https://media.myshows.me/shows/normal/3/39/3902fe3a363a08eb23b02d0743a2461d.jpg", model.Image,
                true);

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

            Assert.Equal(TvShowStatus.CanceledOrEnded, model.Status);

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

            Assert.Equal(GetGenres(), model.Genres);

            Assert.NotNull(model.Seasons);
            Assert.Equal(10, model.TotalSeasons);
            Assert.Equal(10, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(24, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(962, s1E1.Id);

            Assert.Equal("The One Where It All Began", s1E1.Title, true);
            Assert.Equal("s01e01", s1E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/b/a9/ba9bd5398ae5324c86a0130f4ccdf9a0.jpg",
                s1E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1994, 9, 23, 0, 0, 0, DateTimeKind.Utc)), s1E1.AirDate);

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(973, s1E12.Id);

            Assert.Equal("The One With the Dozen Lasagnas", s1E12.Title, true);
            Assert.Equal("s01e12", s1E12.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/a/ec/aec6ba3728af059d22daaeb75ee6d884.jpg",
                s1E12.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1995, 1, 13, 1, 0, 0, DateTimeKind.Utc)), s1E12.AirDate);

            #endregion

            #region Episode 24

            var s1E24 = season1.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(985, s1E24.Id);

            Assert.Equal("The One Where Rachel Finds Out", s1E24.Title, true);
            Assert.Equal("s01e24", s1E24.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/b/04/b04a1e0f1194c97464859cc9c768eb98.jpg",
                s1E24.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1995, 5, 19, 0, 0, 0, DateTimeKind.Utc)), s1E24.AirDate);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(24, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(986, s2E1.Id);

            Assert.Equal("The One With Ross's New Girlfriend", s2E1.Title, true);
            Assert.Equal("s02e01", s2E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/d/c7/dc7c9160361216bd3f19fa9894cde632.jpg",
                s2E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1995, 9, 22, 0, 0, 0, DateTimeKind.Utc)), s2E1.AirDate);

            #endregion

            #region Episode 12

            var s2E12 = season2.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(997, s2E12.Id);

            Assert.Equal("The One After the Superbowl (1)", s2E12.Title, true);
            Assert.Equal("s02e12", s2E12.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/0/db/0dbfb16558a6232cf0d75449a69c4b43.jpg",
                s2E12.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1996, 1, 29, 1, 0, 0, DateTimeKind.Utc)), s2E12.AirDate);

            #endregion

            #region Episode 24

            var s2E24 = season2.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(1009, s2E24.Id);

            Assert.Equal("The One With Barry and Mindy's Wedding", s2E24.Title, true);
            Assert.Equal("s02e24", s2E24.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/e/9d/e9d6818b784d4e9f35061e539239748b.jpg",
                s2E24.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1996, 5, 17, 0, 0, 0, DateTimeKind.Utc)), s2E24.AirDate);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(25, season3.Episodes.Count);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1010, s3E1.Id);

            Assert.Equal("The One With the Princess Leia Fantasy", s3E1.Title, true);
            Assert.Equal("s03e01", s3E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/0/ea/0ea28360eeaa51d7be680a9a50ad58b9.jpg",
                s3E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1996, 9, 17, 0, 0, 0, DateTimeKind.Utc)), s3E1.AirDate);

            #endregion

            #region Episode 12

            var s3E12 = season3.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1021, s3E12.Id);

            Assert.Equal("The One With All the Jealousy", s3E12.Title, true);
            Assert.Equal("s03e12", s3E12.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/2/5f/25f88762e12ad5510c11a7badd9138df.jpg",
                s3E12.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1997, 1, 17, 1, 0, 0, DateTimeKind.Utc)), s3E12.AirDate);

            #endregion

            #region Episode 25

            var s3E25 = season3.Episodes.First(e => e.EpisodeNumber == 25);

            Assert.Equal(1034, s3E25.Id);

            Assert.Equal("The One at the Beach", s3E25.Title, true);
            Assert.Equal("s03e25", s3E25.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/4/c3/4c3051486e49ebae6c88d5ce907b84b7.jpg",
                s3E25.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1997, 5, 16, 0, 0, 0, DateTimeKind.Utc)), s3E25.AirDate);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(24, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1035, s4E1.Id);

            Assert.Equal("The One With the Jellyfish", s4E1.Title, true);
            Assert.Equal("s04e01", s4E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/e/af/eafa78de0610ec8475744262e07a429d.jpg",
                s4E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(1997, 9, 26, 0, 0, 0, DateTimeKind.Utc)), s4E1.AirDate);

            #endregion

            #region Episode 12

            var s4E12 = season4.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1046, s4E12.Id);

            Assert.Equal("The One With the Embryos", s4E12.Title, true);
            Assert.Equal("s04e12", s4E12.ShortName, true);

            Assert.Empty(s4E12.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(1998, 1, 16, 1, 0, 0, DateTimeKind.Utc)), s4E12.AirDate);

            #endregion

            #region Episode 24

            var s4E24 = season4.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(1058, s4E24.Id);

            Assert.Equal("The One With Ross's Wedding (2)", s4E24.Title, true);
            Assert.Equal("s04e24", s4E24.ShortName, true);

            Assert.Empty(s4E24.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(1998, 5, 8, 0, 0, 0, DateTimeKind.Utc)), s4E24.AirDate);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(24, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1059, s5E1.Id);

            Assert.Equal("The One After Ross Says Rachel", s5E1.Title, true);
            Assert.Equal("s05e01", s5E1.ShortName, true);

            Assert.Empty(s5E1.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(1998, 9, 25, 0, 0, 0, DateTimeKind.Utc)), s5E1.AirDate);

            #endregion

            #region Episode 12

            var s5E12 = season5.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1070, s5E12.Id);

            Assert.Equal("The One With Chandler's Work Laugh", s5E12.Title, true);
            Assert.Equal("s05e12", s5E12.ShortName, true);

            Assert.Empty(s5E12.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(1999, 1, 22, 1, 0, 0, DateTimeKind.Utc)), s5E12.AirDate);

            #endregion

            #region Episode 24

            var s5E24 = season5.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(1082, s5E24.Id);

            Assert.Equal("The One in Vegas (2)", s5E24.Title, true);
            Assert.Equal("s05e24", s5E24.ShortName, true);

            Assert.Empty(s5E24.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(1999, 5, 21, 0, 0, 0, DateTimeKind.Utc)), s5E24.AirDate);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(25, season6.Episodes.Count);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1083, s6E1.Id);

            Assert.Equal("The One After Vegas", s6E1.Title, true);
            Assert.Equal("s06e01", s6E1.ShortName, true);

            Assert.Empty(s6E1.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(1999, 9, 24, 0, 0, 0, DateTimeKind.Utc)), s6E1.AirDate);

            #endregion

            #region Episode 12

            var s6E12 = season6.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1094, s6E12.Id);

            Assert.Equal("The One With the Joke", s6E12.Title, true);
            Assert.Equal("s06e12", s6E12.ShortName, true);

            Assert.Empty(s6E12.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2000, 1, 14, 1, 0, 0, DateTimeKind.Utc)), s6E12.AirDate);

            #endregion

            #region Episode 25

            var s6E25 = season6.Episodes.First(e => e.EpisodeNumber == 25);

            Assert.Equal(1107, s6E25.Id);

            Assert.Equal("The One With the Proposal (2)", s6E25.Title, true);
            Assert.Equal("s06e25", s6E25.ShortName, true);

            Assert.Empty(s6E25.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2000, 5, 19, 0, 0, 0, DateTimeKind.Utc)), s6E25.AirDate);

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);

            Assert.NotNull(season7.Episodes);
            Assert.NotNull(season7.Specials);
            Assert.Equal(24, season7.Episodes.Count);
            Assert.Equal(1, season7.Specials.Count);
            
            #region Episode 1

            var s7E1 = season7.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1108, s7E1.Id);

            Assert.Equal("The One With Monica's Thunder", s7E1.Title, true);
            Assert.Equal("s07e01", s7E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/0/95/0959d895d4a6b97625ff10c655940302.jpg",
                s7E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2000, 10, 13, 0, 0, 0, DateTimeKind.Utc)), s7E1.AirDate);

            #endregion

            #region Episode 12

            var s7E12 = season7.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1119, s7E12.Id);

            Assert.Equal("The One Where They're Up All Night", s7E12.Title, true);
            Assert.Equal("s07e12", s7E12.ShortName, true);

            Assert.Empty(s7E12.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2001, 1, 12, 1, 0, 0, DateTimeKind.Utc)), s7E12.AirDate);

            #endregion

            #region Episode 24

            var s7E24 = season7.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(1131, s7E24.Id);

            Assert.Equal("The One with Monica and Chandler's Wedding (2)", s7E24.Title, true);
            Assert.Equal("s07e24", s7E24.ShortName, true);

            Assert.Empty(s7E24.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2001, 5, 18, 0, 0, 0, DateTimeKind.Utc)), s7E24.AirDate);

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);

            Assert.NotNull(season8.Episodes);
            Assert.Equal(24, season8.Episodes.Count);

            #region Episode 1

            var s8E1 = season8.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1132, s8E1.Id);

            Assert.Equal("The One After \"I Do\"", s8E1.Title, true);
            Assert.Equal("s08e01", s8E1.ShortName, true);

            Assert.Empty(s8E1.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2001, 9, 28, 0, 0, 0, DateTimeKind.Utc)), s8E1.AirDate);

            #endregion

            #region Episode 12

            var s8E12 = season8.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1143, s8E12.Id);

            Assert.Equal("The One Where Joey Dates Rachel", s8E12.Title, true);
            Assert.Equal("s08e12", s8E12.ShortName, true);

            Assert.Empty(s8E12.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2002, 1, 11, 1, 0, 0, DateTimeKind.Utc)), s8E12.AirDate);

            #endregion

            #region Episode 24

            var s8E24 = season8.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(1155, s8E24.Id);

            Assert.Equal("The One Where Rachel Has a Baby (2)", s8E24.Title, true);
            Assert.Equal("s08e24", s8E24.ShortName, true);

            Assert.Empty(s8E24.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2002, 5, 17, 0, 0, 0, DateTimeKind.Utc)), s8E24.AirDate);

            #endregion

            #endregion

            #region Season 9

            var season9 = model.Seasons.First(s => s.Index == 9);

            Assert.NotNull(season9.Episodes);
            Assert.Equal(24, season9.Episodes.Count);

            #region Episode 1

            var s9E1 = season9.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1156, s9E1.Id);

            Assert.Equal("The One Where No One Proposes", s9E1.Title, true);
            Assert.Equal("s09e01", s9E1.ShortName, true);

            Assert.Empty(s9E1.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2002, 9, 27, 0, 0, 0, DateTimeKind.Utc)), s9E1.AirDate);

            #endregion

            #region Episode 12

            var s9E12 = season9.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1167, s9E12.Id);

            Assert.Equal("The One With Phoebe's Rats", s9E12.Title, true);
            Assert.Equal("s09e12", s9E12.ShortName, true);

            Assert.Empty(s9E12.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2003, 1, 17, 1, 0, 0, DateTimeKind.Utc)), s9E12.AirDate);

            #endregion

            #region Episode 24

            var s9E24 = season9.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(1179, s9E24.Id);

            Assert.Equal("The One in Barbados (2)", s9E24.Title, true);
            Assert.Equal("s09e24", s9E24.ShortName, true);

            Assert.Empty(s9E24.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2003, 5, 16, 0, 0, 0, DateTimeKind.Utc)), s9E24.AirDate);

            #endregion

            #endregion

            #region Season 10

            var season10 = model.Seasons.First(s => s.Index == 10);

            Assert.NotNull(season10.Episodes);
            Assert.NotNull(season10.Specials);
            Assert.Equal(18, season10.Episodes.Count);
            Assert.Equal(3, season10.Specials.Count);
            
            #region Episode 1

            var s10E1 = season10.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1180, s10E1.Id);

            Assert.Equal("The One After Joey And Rachel Kiss", s10E1.Title, true);
            Assert.Equal("s10e01", s10E1.ShortName, true);

            Assert.Empty(s10E1.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2003, 9, 26, 0, 0, 0, DateTimeKind.Utc)), s10E1.AirDate);

            #endregion

            #region Episode 9

            var s10E9 = season10.Episodes.First(e => e.EpisodeNumber == 9);

            Assert.Equal(1188, s10E9.Id);

            Assert.Equal("The One With the Birth Mother", s10E9.Title, true);
            Assert.Equal("s10e09", s10E9.ShortName, true);

            Assert.Empty(s10E9.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2004, 1, 9, 1, 0, 0, DateTimeKind.Utc)), s10E9.AirDate);

            #endregion

            #region Episode 18

            var s10E18 = season10.Episodes.First(e => e.EpisodeNumber == 18);

            Assert.Equal(1197, s10E18.Id);

            Assert.Equal("The Last One (2)", s10E18.Title, true);
            Assert.Equal("s10e18", s10E18.ShortName, true);

            Assert.Empty(s10E18.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2004, 5, 7, 0, 0, 0, DateTimeKind.Utc)), s10E18.AirDate);

            #endregion

            #region Special 3 FRIENDS REUNION - Tribute To Director James Burrows

            var s10S3 = season10.Specials.First(e => e.ShortName == "s10 special-3");

            Assert.Equal(15668758, s10S3.Id);

            Assert.Equal("FRIENDS REUNION - Tribute To Director James Burrows", s10S3.Title, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/6/4b/64b85d4cc7efeb5fbd2ac6452b34b4cc.jpg",
                s10S3.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2016, 2, 22, 1, 0, 0, DateTimeKind.Utc)), s10S3.AirDate);

            #endregion

            #endregion
        }

        #endregion

        #region Extract Anger Managment TvShow Tests

        [Fact]
        public async Task Should_Extract_Anger_Managment_Id_English()
        {
            var model = await _englishService.GetTvShowByIdAsync(23992);
            ValidateAngerManagment(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_Id_Russian()
        {
            var model = await _russianService.GetTvShowByIdAsync(23992);
            ValidateAngerManagment(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_Uri()
        {
            var model = await _englishService.GetTvShowByUriAsync("https://myshows.me/view/23992/");
            ValidateAngerManagment(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_English_Title()
        {
            string fullTitle = GetAngerManagmentTitle(Language.English);
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(23992, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Anger_Managment_By_Russian_Title()
        {
            string fullTitle = GetAngerManagmentTitle(Language.Russian);
            var models = await _russianService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

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

        private void ValidateAngerManagment(MyShowsTvShowModel model, Language language)
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

            Assert.Equal(GetAngerManagmentTitle(language), model.Title, true);
            Assert.Equal(GetAngerManagmentTitle(Language.English), model.TitleOriginal, true);

            Assert.Equal(GetDescription().ClearString(), model.Description.ClearString(), true);

            Assert.Equal("US", model.Country, true);

            Assert.Equal("https://media.myshows.me/shows/normal/3/3b/3b8013ec507437324e069d8c998f8c9c.jpg", model.Image,
                true);

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

            Assert.Equal(TvShowStatus.CanceledOrEnded, model.Status);

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

            Assert.Equal(GetGenres(), model.Genres);

            Assert.NotNull(model.Seasons);
            Assert.Equal(2, model.TotalSeasons);
            Assert.Equal(2, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(10, season1.Episodes.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1411218, s1E1.Id);

            Assert.Equal("Charlie Goes Back to Therapy", s1E1.Title, true);
            Assert.Equal("s01e01", s1E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/a/9f/a9f9079688eb61352666e3e38157da14.jpg",
                s1E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2012, 6, 29, 2, 0, 0, DateTimeKind.Utc)), s1E1.AirDate);

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.EpisodeNumber == 5);

            Assert.Equal(1515544, s1E5.Id);

            Assert.Equal("Charlie Tries to Prove Therapy is Legit", s1E5.Title, true);
            Assert.Equal("s01e05", s1E5.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/6/44/644fbc577091c796fb428a1b274c169b.jpg",
                s1E5.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2012, 7, 20, 2, 0, 0, DateTimeKind.Utc)), s1E5.AirDate);

            #endregion

            #region Episode 10

            var s1E10 = season1.Episodes.First(e => e.EpisodeNumber == 10);

            Assert.Equal(1515549, s1E10.Id);

            Assert.Equal("Charlie Gets Romantic", s1E10.Title, true);
            Assert.Equal("s01e10", s1E10.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/8/aa/8aa9a6917e6bb7fa8389ccb720881bba.jpg",
                s1E10.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2012, 8, 24, 2, 0, 0, DateTimeKind.Utc)), s1E10.AirDate);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(90, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1579536, s2E1.Id);

            Assert.Equal("Charlie Loses It at a Baby Shower", s2E1.Title, true);
            Assert.Equal("s02e01", s2E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/b/4e/b4ebede2195719823f18395ba56ed645.jpg",
                s2E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 1, 18, 3, 0, 0, DateTimeKind.Utc)), s2E1.AirDate);

            #endregion

            #region Episode 45

            var s2E45 = season2.Episodes.First(e => e.EpisodeNumber == 45);

            Assert.Equal(1979110, s2E45.Id);

            Assert.Equal("Charlie and Lacey Shack Up", s2E45.Title, true);
            Assert.Equal("s02e45", s2E45.ShortName, true);

            Assert.Empty(s2E45.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 12, 13, 2, 30, 0, DateTimeKind.Utc)), s2E45.AirDate);

            #endregion

            #region Episode 90

            var s2E90 = season2.Episodes.First(e => e.EpisodeNumber == 90);

            Assert.Equal(2459243, s2E90.Id);

            Assert.Equal("Charlie and the 100th Episode", s2E90.Title, true);
            Assert.Equal("s02e90", s2E90.ShortName, true);

            Assert.Empty(s2E90.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2014, 12, 23, 3, 0, 0, DateTimeKind.Utc)), s2E90.AirDate);

            #endregion

            #endregion
        }

        #endregion
        
        #region Extract Steins;Gate Anime Show Tests

        [Fact]
        public async Task Should_Extract_Steins_Gate_Id_English()
        {
            var model = await _englishService.GetTvShowByIdAsync(15897);
            ValidateSteinsGate(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_Id_Russian()
        {
            var model = await _russianService.GetTvShowByIdAsync(15897);
            ValidateSteinsGate(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_Uri()
        {
            var model = await _englishService.GetTvShowByUriAsync("https://myshows.me/view/15897/");
            ValidateSteinsGate(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_English_Title()
        {
            string fullTitle = GetSteinsGateTitle(Language.English);
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(15897, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Steins_Gate_By_Russian_Title()
        {
            string fullTitle = GetSteinsGateTitle(Language.Russian);
            var models = await _russianService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

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

        private void ValidateSteinsGate(MyShowsTvShowModel model, Language language)
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

            Assert.Equal(GetSteinsGateTitle(language), model.Title, true);
            Assert.Equal(GetSteinsGateTitle(Language.English), model.TitleOriginal, true);

            Assert.Equal(GetDescription().ClearString(), model.Description.ClearString(), true);

            Assert.Equal("JP", model.Country, true);

            Assert.Equal("https://media.myshows.me/shows/normal/1/1b/1ba31529a26258159a5f2b6de7351a65.jpg", model.Image,
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
                        return new List<string> {"Drama","Sci-Fi", "Fantasy", "Anime"};
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

            Assert.NotNull(season1.Episodes);
            Assert.NotNull(season1.Specials);
            Assert.Equal(24, season1.Episodes.Count);
            Assert.Equal(7, season1.Specials.Count);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.EpisodeNumber == 1);

            Assert.Equal(1021681, s1E1.Id);

            Assert.Equal("Prologue of the Beginning and the End", s1E1.Title, true);
            Assert.Equal("s01e01", s1E1.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/9/79/979790b845c14f22cbf65676b35def02.jpg",
                s1E1.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2011, 4, 6, 3, 0, 0, DateTimeKind.Utc)), s1E1.AirDate);

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.EpisodeNumber == 12);

            Assert.Equal(1100177, s1E12.Id);

            Assert.Equal("Dogma of Static Limit", s1E12.Title, true);
            Assert.Equal("s01e12", s1E12.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/4/7c/47cc636b76ca10d8507418f4557034d7.jpg",
                s1E12.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2011, 6, 22, 3, 0, 0, DateTimeKind.Utc)), s1E12.AirDate);

            #endregion

            #region Episode 24

            var s1E24 = season1.Episodes.First(e => e.EpisodeNumber == 24);

            Assert.Equal(1197405, s1E24.Id);

            Assert.Equal("The Prologue Begins With the End", s1E24.Title, true);
            Assert.Equal("s01e24", s1E24.ShortName, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/7/e3/7e33df48114661a3cd9f22388e23c104.jpg",
                s1E24.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2011, 9, 14, 3, 0, 0, DateTimeKind.Utc)), s1E24.AirDate);

            #endregion

            #region Special 4
          
            var s1S4 = season1.Specials.First(e => e.ShortName == "s01 special-4");

            Assert.Equal(2608194, s1S4.Id);

            Assert.Equal("Soumei Eichi no Cognitive Computing Episode 4: Meeting Chapter", s1S4.Title, true);

            Assert.Equal("https://media.myshows.me/episodes/normal/3/6b/36bec630c22324fe32c735d9a6f9c2ea.jpg",
                s1S4.Image, true);

            Assert.Equal(new DateTimeOffset(new DateTime(2014, 10, 22, 4, 0, 0, DateTimeKind.Utc)), s1S4.AirDate);

            #endregion
            
            #endregion
        }

        #endregion
    }
}