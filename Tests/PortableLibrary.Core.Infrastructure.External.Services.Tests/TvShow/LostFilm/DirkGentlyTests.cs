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
    public class DirkGentlyTests: LostFilmTestsBase
    {
        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency()
        {
            var model = await Service.ExtractTvShowAsync(
                "https://www.lostfilm.tv/series/Dirk_Gentlys_Holistic_Detective_Agency/");

            #region Tv Show

            var genres = new List<string>
            {
                "Комедия",
                "Мистика",
                "Фантастика",
                "Детектив"
            };

            string testDescription =
                "Жестоко растерзанные трупы, устрашающего вида бугаи-наемники, спятившая девица с мачете, " +
                "пустоголовые спецагенты, симпатичный вельш-корги и пугливый черный котенок, а также " +
                "эксцентричный детектив в желтой куртке — все оказывается связанным в возмутительно " +
                "искрометном сериале «Холистическое детективное агентство Дирка Джентли». Изголодавшимся " +
                "по острым впечатлениям зрителям не стоит проходить мимо этой фантастической черной комедии " +
                "с элементами детектива, абсурдистского юмора и мистики. Адаптация одноименного романа " +
                "культового британского фантаста Дугласа Адамса впитала в себя авантюрный дух, динамичный " +
                "темп повествования и сочную стилистику оригинала. К тому же по атмосфере шоу близко к таким " +
                "телевизионным шедеврам, как «Утопия», «Шерлок», «Уилфред» и «Фарго». Если вы давно не " +
                "подбирали с пола челюсть, вопрошая себя: «Что за чертовщина здесь творится?», — «Дирк Джентли»" +
                " напомнит вам об этом забытом ощущении шока и восторга. А запутанный, полный головоломок и " +
                "интриг сюжет, завораживающая цветовая гамма, колоритные гипертрофированные персонажи и " +
                "вызывающая параноидальные настроения музыка только укрепят чувство несуразности происходящего." +
                "Сюжет" +
                "Очередной унылый день отельного коридорного Тодда Бротцмана (Элайджа Вуд) " +
                "начинается с крика разъяренного домовладельца, крушащего его машину за неуплату аренды. " +
                "Больная редким неврологическим заболеванием сестра просит денег на лечение, а на работе " +
                "происходит ужасное кровавое убийство богатого постояльца пентхауса. Главным подозреваемым, " +
                "несмотря на совсем уж фантастические обстоятельства, конечно же, становится неудачник Тодд. " +
                "Однако настоящее безумие в его жизни начинается, когда он приходит домой и обнаруживает в " +
                "своей квартире загадочного гостя. Вломившийся через окно беспардонный и весьма эксцентричный " +
                "детектив Дирк Джентли (Сэмюэл Барнетт) тут же навязывает озадаченному Бротцману дружбу и роль " +
                "своего помощника. Тодд не успевает опомниться, как оказывается втянут в расследование " +
                "убийства, которое страдает отсутствием логики или вообще каких-либо действий розыскного " +
                "характера. Холистический сыщик Джентли не тратит времени на такую ерунду, как отпечатки " +
                "пальцев, поиск улик и допросы подозреваемых. Он убежден, что вселенная сама подскажет ему " +
                "ответы. Вот только делать это она не спешит, взамен подбрасывая странному дуэту испытания " +
                "одно другого опаснее. Разгадать безумную тайну, в которой явно замешаны сверхъестественные " +
                "силы, и не поплатиться жизнью за дружбу с экстравагантным чудаком — задача не из легких. " +
                "Особенно когда на хвосте у тебя сидит с десяток подозрительных типов, включая психованную " +
                "холистическую убийцу...";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            ValidateTvShow(model, title: "Холистическое детективное агентство Дирка Джентли",
                originalTitle: "Dirk Gently's Holistic Detective Agency",
                imageUri: "static.lostfilm.tv/Images/293/Posters/poster.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: genres, description: testDescription, seasonsCount: 2);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 8);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, title: "Горизонты", originalTitle: "Horizons",
                dateReleased: new DateTime(2016, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 10, 22, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 2

            var s1E2 = season1.Episodes.First(e => e.Index == 2);

            ValidateEpisode(s1E2, title: "Бюро находок", originalTitle: "Lost and Found",
                dateReleased: new DateTime(2016, 11, 6, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 10, 29, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 3

            var s1E3 = season1.Episodes.First(e => e.Index == 3);

            ValidateEpisode(s1E3, title: "Сумасшедшие фанаты стен", originalTitle: "Rogue Wall Enthusiasts",
                dateReleased: new DateTime(2016, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 11, 5, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 4

            var s1E4 = season1.Episodes.First(e => e.Index == 4);

            ValidateEpisode(s1E4, title: "Уоткин", originalTitle: "Watkin",
                dateReleased: new DateTime(2016, 11, 15, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 11, 12, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.Index == 5);

            ValidateEpisode(s1E5, title: "Очень эректус", originalTitle: "Very Erectus",
                dateReleased: new DateTime(2016, 11, 22, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 11, 19, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 6

            var s1E6 = season1.Episodes.First(e => e.Index == 6);

            ValidateEpisode(s1E6, title: "Мы всё исправим", originalTitle: "Fix Everything",
                dateReleased: new DateTime(2016, 11, 29, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 11, 26, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 7

            var s1E7 = season1.Episodes.First(e => e.Index == 7);

            ValidateEpisode(s1E7, title: "Взрывоопасный дух", originalTitle: "Weaponized Soul",
                dateReleased: new DateTime(2016, 12, 7, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 12, 3, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 8

            var s1E8 = season1.Episodes.First(e => e.Index == 8);

            ValidateEpisode(s1E8, title: "Два вменяемых парня, занимающиеся нормальными вещами",
                originalTitle: "Two Sane Guys Doing Normal Things",
                dateReleased: new DateTime(2016, 12, 13, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2016, 12, 10, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 10);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, title: "Кролик из космоса", originalTitle: "Space Rabbit",
                dateReleased: new DateTime(2017, 10, 16, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 10, 14, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 2

            var s2E2 = season2.Episodes.First(e => e.Index == 2);

            ValidateEpisode(s2E2, title: "Фанаты мокрых кругов", originalTitle: "Fans of Wet Circles",
                dateReleased: new DateTime(2017, 10, 23, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 10, 21, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 3

            var s2E3 = season2.Episodes.First(e => e.Index == 3);

            ValidateEpisode(s2E3, title: "Два сломанных пальца", originalTitle: "Two Broken Fingers",
                dateReleased: new DateTime(2017, 10, 30, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 10, 28, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 4

            var s2E4 = season2.Episodes.First(e => e.Index == 4);

            ValidateEpisode(s2E4, title: "Дом внутри дома", originalTitle: "The House Within the House",
                dateReleased: new DateTime(2017, 11, 6, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 11, 4, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 5

            var s2E5 = season2.Episodes.First(e => e.Index == 5);

            ValidateEpisode(s2E5, title: "Цветные узоры", originalTitle: "Shapes and Colors",
                dateReleased: new DateTime(2017, 11, 13, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 11, 11, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 6

            var s2E6 = season2.Episodes.First(e => e.Index == 6);

            ValidateEpisode(s2E6, title: "Высокая самооценка", originalTitle: "Girl Power",
                dateReleased: new DateTime(2017, 11, 20, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 11, 18, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 7

            var s2E7 = season2.Episodes.First(e => e.Index == 7);

            ValidateEpisode(s2E7, title: "Это не Майами", originalTitle: "That Is Not Miami",
                dateReleased: new DateTime(2017, 11, 27, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 11, 25, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 8

            var s2E8 = season2.Episodes.First(e => e.Index == 8);

            ValidateEpisode(s2E8, title: "Мелкий чел, черные волосы", originalTitle: "Little Guy, Black Hair",
                dateReleased: new DateTime(2017, 12, 4, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 12, 2, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 9

            var s2E9 = season2.Episodes.First(e => e.Index == 9);

            ValidateEpisode(s2E9, title: "Проблемы — это плохо", originalTitle: "Trouble is Bad",
                dateReleased: new DateTime(2017, 12, 11, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 12, 9, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 10

            var s2E10 = season2.Episodes.First(e => e.Index == 10);

            ValidateEpisode(s2E10, title: "Классная куртка", originalTitle: "Nice Jacket",
                dateReleased: new DateTime(2017, 12, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2017, 12, 16, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }
    }
}
