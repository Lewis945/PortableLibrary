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
    public class GrimmTests : MyShowsTestsBase
    {
        #region Extract Grimm TvShow Tests

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_English()
        {
            var model = await EnglishService.ExtractTvShowAsync(17186);
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_Russian()
        {
            var model = await RussianService.ExtractTvShowAsync(17186);
            ValidateGrimm(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Uri()
        {
            var model = await EnglishService.ExtractTvShowAsync("https://myshows.me/view/17186/");
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_English_Title()
        {
            string fullTitle = GetGrimmTitle(Language.English);
            var models = await EnglishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Titles?.Contains(fullTitle) ?? false);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Russian_Title()
        {
            string fullTitle = GetGrimmTitle(Language.Russian);
            var models = await RussianService.FindTvShowAsync(fullTitle);
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
                        return new List<string> { "Drama", "Crime", "Supernatural" };
                    case Language.Russian:
                        return new List<string> { "Драма", "Криминал", "Сверхъестественное" };
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
                dateReleasedOrigianl: new DateTime(2014, 1, 29, 6, 0, 0, DateTimeKind.Utc));

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
    }
}
