using PortableLibrary.Core.External.Services.TvShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.LostFilm
{
    public class DesperateHousewivesTests: LostFilmTestsBase
    {
        [Fact]
        public async Task Should_Extract_Desperate_Housewives()
        {
            var model = await Service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Desperate_Housewives");

            #region Tv Show

            var genres = new List<string>
            {
                "Драма",
                "Комедия"
            };

            string testDescription = "В центре событий — четыре современные домохозяйки, которые живут в тихом " +
                                     "пригороде и отчаянно ищут личного счастья. Сьюзан осталась одна после того, " +
                                     "как муж променял ее на секретаршу. Бывшая фотомодель, а ныне неверная жена, " +
                                     "Габриэль вышла замуж по расчету, а потом поняла, что нуждается не в деньгах, а " +
                                     "в настоящей любви, и завела роман с юным садовником. Бри пытается сохранить " +
                                     "разваливающийся брак и найти общий язык с сыном-наркоманом и слишком рано " +
                                     "повзрослевшей дочерью. Линнет поставила крест на блестящей карьере в крупной " +
                                     "компании, чтобы посвятить все свое время воспитанию троих детей. Внезапное " +
                                     "самоубийство их подружки Мэри Элис Янг оставляет всех в недоумении и заставляет " +
                                     "искать разгадку ее смерти. Мэри расстается с жизнью, но обретает способность " +
                                     "проникать во все секреты, спрятанные за наглухо закрытыми дверями этого " +
                                     "благополучного американского пригорода. Наблюдая за жизнью подружек с высоты " +
                                     "своего нового положения, она пытается помочь им обрести личное счастье и с " +
                                     "помощью записок дает им советы…";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            ValidateTvShow(model, title: "Отчаянные домохозяйки",
                originalTitle: "Desperate Housewives",
                imageUri: "static.lostfilm.tv/Images/64/Posters/poster.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: genres, description: testDescription, seasonsCount: 8);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 23);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, title: "Пилотная", originalTitle: "Pilot",
                dateReleased: new DateTime(2007, 2, 13, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2004, 10, 3, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s1E12, title: "Каждый день немного смерти", originalTitle: "Every Day a Little Death",
                dateReleased: new DateTime(2007, 3, 2, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2005, 1, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 23

            var s1E23 = season1.Episodes.First(e => e.Index == 23);

            ValidateEpisode(s1E23, title: "В один прекрасный день", originalTitle: "One Wonderful Day",
                dateReleased: new DateTime(2007, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2005, 5, 22, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 24);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, title: "Следующий", originalTitle: "Next",
                dateReleased: new DateTime(2007, 7, 24, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2005, 9, 25, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s2E12 = season2.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s2E12, title: "У нас все будет хорошо", originalTitle: "We're Gonna Be All Right",
                dateReleased: new DateTime(2007, 7, 29, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2006, 1, 15, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s2E24 = season2.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s2E24, title: "Помни: часть 2", originalTitle: "Remember: Part 2",
                dateReleased: new DateTime(2007, 7, 31, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2006, 5, 21, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            ValidateSeason(season3, 23);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s3E1, title: "Слышишь, дождь стучит по крыше?",
                originalTitle: "Listen to the Rain on the Roof",
                dateReleased: new DateTime(2007, 6, 7, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2006, 9, 24, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s3E12 = season3.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s3E12, title: "Неприятное соседство", originalTitle: "Not While I'm Around",
                dateReleased: new DateTime(2007, 8, 7, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2007, 1, 14, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 23

            var s3E23 = season3.Episodes.First(e => e.Index == 23);

            ValidateEpisode(s3E23, title: "Свадебная суета", originalTitle: "Getting Married Today",
                dateReleased: new DateTime(2008, 7, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2007, 5, 20, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            ValidateSeason(season4, 17);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s4E1, title: "Теперь ты знаешь", originalTitle: "Now You Know",
                dateReleased: new DateTime(2009, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2007, 9, 30, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 8

            var s4E8 = season4.Episodes.First(e => e.Index == 8);

            ValidateEpisode(s4E8, title: "Далекое прошлое", originalTitle: "Distant Past",
                dateReleased: new DateTime(2009, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2007, 11, 25, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 17

            var s4E17 = season4.Episodes.First(e => e.Index == 17);

            ValidateEpisode(s4E17, title: "Свобода", originalTitle: "Free",
                dateReleased: new DateTime(2009, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2008, 5, 18, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            ValidateSeason(season5, 24);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s5E1, title: "Завтра будет хорошо", originalTitle: "You're Gonna Love Tomorrow",
                dateReleased: new DateTime(2009, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2008, 9, 28, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s5E12 = season5.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s5E12, title: "Прием! Прием!", originalTitle: "Connect! Connect!",
                dateReleased: new DateTime(2009, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2009, 1, 11, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s5E24 = season5.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s5E24, title: "В плену иллюзий", originalTitle: "If It's Only in Your Head",
                dateReleased: new DateTime(2009, 11, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2009, 5, 17, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            ValidateSeason(season6, 23);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s6E1, title: "Лучшее — враг хорошего!", originalTitle: "Nice Is Different Than Good",
                dateReleased: new DateTime(2009, 11, 27, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2009, 9, 27, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s6E12 = season6.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s6E12, title: "Придется пойти на хитрость", originalTitle: "You Gotta Get a Gimmick",
                dateReleased: new DateTime(2010, 1, 16, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2010, 1, 10, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 23

            var s6E23 = season6.Episodes.First(e => e.Index == 23);

            ValidateEpisode(s6E23, title: "Видимо, это прощание", originalTitle: "I Guess This Is Goodbye",
                dateReleased: new DateTime(2010, 6, 3, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2010, 5, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);

            ValidateSeason(season7, 23);

            #region Episode 1

            var s7E1 = season7.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s7E1, title: "Помнишь Пола?", originalTitle: "Remember Paul?",
                dateReleased: new DateTime(2010, 10, 2, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2010, 9, 26, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s7E12 = season7.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s7E12, title: "Где мое место?", originalTitle: "Where Do I Belong?",
                dateReleased: new DateTime(2011, 1, 17, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2011, 1, 9, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 23

            var s7E23 = season7.Episodes.First(e => e.Index == 23);

            ValidateEpisode(s7E23, title: "Приходите на ужин", originalTitle: "Come on Over for Dinner",
                dateReleased: new DateTime(2011, 6, 14, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2011, 5, 15, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);

            ValidateSeason(season8, 23);

            #region Episode 1

            var s8E1 = season8.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s8E1, title: "Тайны, которые я не хочу знать",
                originalTitle: "Secrets That I Never Want to Know",
                dateReleased: new DateTime(2011, 10, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2011, 9, 25, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s8E12 = season8.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s8E12, title: "Что хорошего в том, чтобы быть хорошей",
                originalTitle: "What's the Good of Being Good",
                dateReleased: new DateTime(2012, 1, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 1, 22, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 23

            var s8E23 = season8.Episodes.First(e => e.Index == 23);

            ValidateEpisode(s8E23, title: "Последний штрих", originalTitle: "Finishing the Hat",
                dateReleased: new DateTime(2012, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 5, 13, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }
    }
}
