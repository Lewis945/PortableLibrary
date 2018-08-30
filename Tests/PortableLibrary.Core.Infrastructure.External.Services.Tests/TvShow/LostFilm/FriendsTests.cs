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
    public class FriendsTests: LostFilmTestsBase
    {
        [Fact]
        public async Task Should_Extract_Friends()
        {
            var model = await Service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Friends");

            #region Tv Show

            var genres = new List<string>
            {
                "Комедия",
                "Семейный"
            };

            string testDescription = "«Друзья» — это ситком, который с середины 90-х и на долгие годы задал " +
                                     "тон всем комедийным телешоу о том, как в одной компании могут уживаться, дружить, любить, " +
                                     "строить карьеры, бездельничать и бесконечно шутить самые разные по характеру молодые люди. " +
                                     "Начиная с первого эпизода и на протяжении всех десяти лет трансляции качество юмора в этом " +
                                     "шоу приносило ему стабильно высокие рейтинги. Сплоченная команда сценаристов в поте лица " +
                                     "трудилась над тем, чтобы шутка была почти в каждой реплике. Уморительные, абсурдные, " +
                                     "нарочито несмешные, искрометные, добродушные, язвительные — все виды шуток, существующие " +
                                     "в природе, вы найдете в «Друзьях». Но не только в этом секрет бешеной популярности, " +
                                     "культового статуса сериала и причисление его к классике жанра. «Друзья» показывают нам " +
                                     "нас такими, какими мы хотим себя видеть, — молодыми, беззаботными, неунывающими, полными " +
                                     "надежд и еще не успевшими разочароваться в жизни. Также стоит отметить, что ситком послужил " +
                                     "толчком для карьер всех главных актеров, которых долгое время поклонники называли " +
                                     "исключительно по именам их персонажей. За десять лет успешных показов «Друзья» заработали " +
                                     "почти семь десятков наград, включая шесть премий «Эмми»." +
                                     "СЮЖЕТ" +
                                     "Их шестеро.Они молоды, красивы, " +
                                     "живут в большом городе, ходят на работу, вместе отмечают праздники и переживают потери, а " +
                                     "свободное время любят проводить в одном и том же кафе за обсуждением последних новостей. " +
                                     "Росс(Дэвид Швиммер) — палеонтолог, которого бросила жена, и он очень из-за этого страдает. " +
                                     "Его младшая сестра Моника(Кортни Кокс) собирается на свидание с очаровательным кавалером и " +
                                     "смущается от подколов друзей на тему того, что он снова окажется не «тем самым». " +
                                     "Чендлер(Мэттью Перри) пришел в кафе, чтобы рассказать всем о том, что ему приснился «голый» сон." +
                                     "Джоуи(Мэтт ЛеБлан) советует бедолаге Россу избавиться от душевных мук посредством похода на " +
                                     "стриптиз, а Фиби(Лиза Кудроу) с готовностью принялась чистить расстроенному другу карму, " +
                                     "совершая пассы руками над его головой. Внезапно в кафе врывается Рэйчел(Дженнифер Энистон) в " +
                                     "свадебном платье — прямо во время свадебной церемонии девушка поняла, что голова ее жениха " +
                                     "слишком похожа на брюкву, и вообще она этого мужчину ни капельки не любит, поэтому подхватила " +
                                     "подол белоснежного наряда и сбежала... к друзьям в кафе. Чуть позже, уже в квартире у Моники, " +
                                     "Рэйчел поругается по телефону с отцом и решит немного пожить у подруги.Тем временем Росс, " +
                                     "влюбленный в Рэйчел с детства, наберется храбрости и пригласит ее как-нибудь куда - нибудь " +
                                     "сходить.С этого и начнется череда забавных, но жизненных приключений шестерых героев. Монике, " +
                                     "Россу, Рэйчел, Джоуи, Чендлеру и Фиби предстоит пережить лучшие и худшие моменты, неоднократно " +
                                     "подставлять друг другу плечо, делиться советами, едой и деньгами, дарить любовь и спорить." +
                                     "И все это они будут делать исключительно вместе, ведь они — настоящие друзья.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            ValidateTvShow(model, title: "Друзья",
                originalTitle: "Friends",
                imageUri: "static.lostfilm.tv/Images/72/Posters/poster.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: genres, description: testDescription, seasonsCount: 10);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 24);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, title: "Серия, где Моника берет новую соседку",
                originalTitle: "The One Where Monica Gets A Roommate",
                dateReleased: new DateTime(2006, 12, 14, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1994, 9, 22, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s1E12, title: "Серия с дюжиной лазаний", originalTitle: "The One With The Dozen Lasagnas",
                dateReleased: new DateTime(2006, 12, 14, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1995, 1, 12, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s1E24 = season1.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s1E24, title: "Серия, где Рейчел понимает", originalTitle: "The One Where Rachel Finds Out",
                dateReleased: new DateTime(2006, 12, 14, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1995, 5, 18, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 24);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, title: "Серия с новой подругой Росса",
                originalTitle: "The One With Ross's New Girlfriend",
                dateReleased: new DateTime(2006, 12, 15, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1995, 9, 21, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s2E12 = season2.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s2E12, title: "Серия после Суперкубка. Часть 1",
                originalTitle: "The One After The Super Bowl (1)",
                dateReleased: new DateTime(2006, 12, 15, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1996, 1, 28, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s2E24 = season2.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s2E24, title: "Серия со свадьбой Барри и Минди",
                originalTitle: "The One With Barry And Mindy's Wedding",
                dateReleased: new DateTime(2006, 12, 15, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1996, 5, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);

            ValidateSeason(season3, 25);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s3E1, title: "Серия с фантазией о принцессе Лейе",
                originalTitle: "The One With The Princess Leia Fantasy",
                dateReleased: new DateTime(2006, 12, 18, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1996, 9, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s3E12 = season3.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s3E12, title: "Серия с ревностью всех", originalTitle: "The One With All The Jealousy",
                dateReleased: new DateTime(2006, 12, 18, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1997, 1, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 25

            var s3E25 = season3.Episodes.First(e => e.Index == 25);

            ValidateEpisode(s3E25, title: "Серия на пляже", originalTitle: "The One At The Beach",
                dateReleased: new DateTime(2006, 12, 18, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1997, 5, 15, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);

            ValidateSeason(season4, 24);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s4E1, title: "Серия с медузой", originalTitle: "The One With The Jellyfish",
                dateReleased: new DateTime(2006, 12, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1997, 9, 25, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s4E12 = season4.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s4E12, title: "Серия с эмбрионами", originalTitle: "The One With The Embryos",
                dateReleased: new DateTime(2006, 12, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1998, 1, 15, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s4E24 = season4.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s4E24, title: "Серия со свадьбой Росса. Часть 2",
                originalTitle: "The One With Ross's Wedding (2)",
                dateReleased: new DateTime(2006, 12, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1998, 5, 7, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);

            ValidateSeason(season5, 24);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s5E1, title: "Серия после того, как Росс назвал имя Рэйчел",
                originalTitle: "The One After Ross Says Rachel",
                dateReleased: new DateTime(2006, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1998, 9, 24, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s5E12 = season5.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s5E12, title: "Серия с рабочим смехом Чендлера",
                originalTitle: "The One With Chandler's Work Laugh",
                dateReleased: new DateTime(2006, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1999, 1, 21, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s5E24 = season5.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s5E24, title: "Серия в Вегасе. Часть 2", originalTitle: "The One In Vegas (2)",
                dateReleased: new DateTime(2006, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1999, 5, 20, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);

            ValidateSeason(season6, 25);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s6E1, title: "Серия после Вегаса", originalTitle: "The One After Vegas",
                dateReleased: new DateTime(2006, 12, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(1999, 9, 23, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s6E12 = season6.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s6E12, title: "Серия c шуткой", originalTitle: "The One With The Joke",
                dateReleased: new DateTime(2006, 12, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2000, 1, 13, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 25

            var s6E25 = season6.Episodes.First(e => e.Index == 25);

            ValidateEpisode(s6E25, title: "Серия с предложением. Часть 2",
                originalTitle: "The One With The Proposal (2)",
                dateReleased: new DateTime(2006, 12, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2000, 5, 18, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);

            ValidateSeason(season7, 24);

            #region Episode 1

            var s7E1 = season7.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s7E1, title: "Серия с вечеринкой Моники", originalTitle: "The One With Monica's Thunder",
                dateReleased: new DateTime(2006, 12, 27, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2000, 10, 12, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s7E12 = season7.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s7E12, title: "Серия, в которой никто не спит",
                originalTitle: "The One Where They're Up All Night",
                dateReleased: new DateTime(2006, 12, 27, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2001, 1, 11, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s7E24 = season7.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s7E24, title: "Серия со свадьбой Моники и Чендлера. Часть 2",
                originalTitle: "The One With Chandler And Monica's Wedding (2)",
                dateReleased: new DateTime(2006, 12, 27, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2001, 5, 17, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);

            ValidateSeason(season8, 24);

            #region Episode 1

            var s8E1 = season8.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s8E1, title: "Серия после слов «Я согласен»", originalTitle: "The One After I Do",
                dateReleased: new DateTime(2007, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2001, 9, 27, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s8E12 = season8.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s8E12, title: "Серия, в которой Джо идет на свидание с Рэйчел",
                originalTitle: "The One Where Joey Dates Rachel",
                dateReleased: new DateTime(2007, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2002, 1, 10, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s8E24 = season8.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s8E24, title: "Серия, в которой рождается ребёнок. Часть 2",
                originalTitle: "The One Where Rachel Has A Baby (2)",
                dateReleased: new DateTime(2007, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2002, 5, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 9

            var season9 = model.Seasons.First(s => s.Index == 9);

            ValidateSeason(season9, 24);

            #region Episode 1

            var s9E1 = season9.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s9E1, title: "Серия, в которой никто не делает предложения",
                originalTitle: "The One Where No One Proposes",
                dateReleased: new DateTime(2007, 1, 8, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2002, 9, 26, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 12

            var s9E12 = season9.Episodes.First(e => e.Index == 12);

            ValidateEpisode(s9E12, title: "Серия с крысами Фиби", originalTitle: "The One With Phoebe's Rats",
                dateReleased: new DateTime(2007, 1, 8, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2003, 1, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 24

            var s9E24 = season9.Episodes.First(e => e.Index == 24);

            ValidateEpisode(s9E24, title: "Серия на Барбадосе. Часть 2", originalTitle: "The One In Barbados (2)",
                dateReleased: new DateTime(2007, 1, 8, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2003, 5, 15, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 10

            var season10 = model.Seasons.First(s => s.Index == 10);

            ValidateSeason(season10, 18);

            #region Episode 1

            var s10E1 = season10.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s10E1, title: "Серия после поцелуя Джо и Рейчел",
                originalTitle: "The One After Joey And Rachel Kiss",
                dateReleased: new DateTime(2007, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2003, 9, 25, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 9

            var s9E9 = season10.Episodes.First(e => e.Index == 9);

            ValidateEpisode(s9E9, title: "Серия с суррогатной матерью", originalTitle: "The One With The Birth Mother",
                dateReleased: new DateTime(2007, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2004, 1, 8, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 18

            var s9E18 = season10.Episodes.First(e => e.Index == 18);

            ValidateEpisode(s9E18, title: "Последняя серия. Часть 2", originalTitle: "The Last One: Part 2",
                dateReleased: new DateTime(2007, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2004, 5, 5, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }
    }
}
