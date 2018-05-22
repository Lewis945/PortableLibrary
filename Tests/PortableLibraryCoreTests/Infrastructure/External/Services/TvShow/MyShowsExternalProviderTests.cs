using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
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
            const string fullTitle = "Grimm";
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Russian_Title()
        {
            const string fullTitle = "Гримм";
            var models = await _russianService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        private void ValidateGrimm(MyShowsTvShowModel model, Language language)
        {
            #region Tv Show

            string GetTitle()
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

            Assert.Equal(GetTitle(), model.Title, true);
            Assert.Equal("Grimm", model.TitleOriginal, true);

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
            Assert.Equal(23, season2.Episodes.Count);

            #region Episode 0

            var s2E0 = season2.Episodes.First(e => e.EpisodeNumber == 0);

            Assert.Equal(2097143, s2E0.Id);

            Assert.Equal("Bad Hair Day", s2E0.Title, true);
            Assert.Equal("s02 special-1", s2E0.ShortName, true);

            Assert.Empty(s2E0.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 1, 17, 2, 0, 0, DateTimeKind.Utc)), s2E0.AirDate);

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
            Assert.Equal(24, season3.Episodes.Count);

            #region Episode 0 Special 1

            var s3E01 = season3.Episodes.First(e => e.EpisodeNumber == 0 && e.ShortName == "s03 special-1");

            Assert.Equal(2097144, s3E01.Id);

            Assert.Equal("Meltdown", s3E01.Title, true);
            Assert.Equal("s03 special-1", s3E01.ShortName, true);

            Assert.Empty(s3E01.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2013, 10, 5, 2, 0, 0, DateTimeKind.Utc)), s3E01.AirDate);

            #endregion

            #region Episode 0 Special 2

            var s3E02 = season3.Episodes.First(e => e.EpisodeNumber == 0 && e.ShortName == "s03 special-2");

            Assert.Equal(2097145, s3E02.Id);

            Assert.Equal("Love is In the Air", s3E02.Title, true);
            Assert.Equal("s03 special-2", s3E02.ShortName, true);

            Assert.Empty(s3E02.Image);

            Assert.Equal(new DateTimeOffset(new DateTime(2014, 1, 31, 14, 0, 0, DateTimeKind.Utc)), s3E02.AirDate);

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
    }
}