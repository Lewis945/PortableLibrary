using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.MyShows
{
    public class FriendsTests : MyShowsTestsBase
    {
        #region Extract Friends TvShow Tests

        [Fact]
        public async Task Should_Extract_Friends_By_Id_English()
        {
            var model = await EnglishService.ExtractTvShowAsync(20);
            ValidateFriends(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Id_Russian()
        {
            var model = await RussianService.ExtractTvShowAsync(20);
            ValidateFriends(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Uri()
        {
            var model = await EnglishService.ExtractTvShowAsync("https://myshows.me/view/20/");
            ValidateFriends(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_English_Title()
        {
            string fullTitle = GetFriendsTitle(Language.English);
            var models = await EnglishService.FindTvShowAsync(fullTitle);
            var model = models.FirstOrDefault(m => (m.Titles?.Contains(fullTitle) ?? false) && m.Year == 1994);

            Assert.NotNull(model);
            Assert.Equal(20, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Friends_By_Russian_Title()
        {
            string fullTitle = GetFriendsTitle(Language.Russian);
            var models = await RussianService.FindTvShowAsync(fullTitle);
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
    }
}
