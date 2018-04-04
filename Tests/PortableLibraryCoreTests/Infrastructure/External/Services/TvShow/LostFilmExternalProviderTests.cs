using System;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
{
    public class LostFilmExternalProviderTests
    {
        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency()
        {
            var service = new LostFilmExternalProvider();

            var model = await service.Extract("https://www.lostfilm.tv/series/Dirk_Gentlys_Holistic_Detective_Agency/");

            #region Tv Show

            Assert.Equal("static.lostfilm.tv/Images/293/Posters/poster.jpg",
                model.ImageUri, true);

            Assert.Equal("Холистическое детективное агентство Дирка Джентли", model.Title, true);
            Assert.Equal("Dirk Gently's Holistic Detective Agency", model.OriginalTitle, true);

            Assert.True(model.IsComplete);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Комедия", item, true),
                item => Assert.Equal("Мистика", item, true),
                item => Assert.Equal("Фантастика", item, true),
                item => Assert.Equal("Детектив", item, true)
            );

            Assert.Equal("Жестоко растерзанные трупы, устрашающего вида бугаи-наемники, спятившая девица с мачете, " +
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
                         "холистическую убийцу...",
                model.Description, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(2, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First();
            Assert.Equal(8, season1.TotalEpisodesCount);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(8, season1.Episodes.Count);

            #region Episode 1

            var s1e1 = season1.Episodes[0];

            Assert.Equal("Горизонты", s1e1.Title, true);
            Assert.Equal("Horizons", s1e1.OriginalTitle, true);

            Assert.Equal(1, s1e1.Index);

            Assert.Equal(new DateTime(2016, 11, 03), s1e1.DateReleased);
            Assert.Equal(new DateTime(2016, 10, 22), s1e1.DateOriginalReleased);

            #endregion

            #region Episode 2

            var s1e2 = season1.Episodes[1];

            Assert.Equal("Бюро находок", s1e2.Title, true);
            Assert.Equal("Lost and Found", s1e2.OriginalTitle, true);

            Assert.Equal(2, s1e2.Index);

            Assert.Equal(new DateTime(2016, 11, 6), s1e2.DateReleased);
            Assert.Equal(new DateTime(2016, 10, 29), s1e2.DateOriginalReleased);

            #endregion

            #region Episode 3

            var s1e3 = season1.Episodes[2];

            Assert.Equal("Сумасшедшие фанаты стен", s1e3.Title, true);
            Assert.Equal("Rogue Wall Enthusiasts", s1e3.OriginalTitle, true);

            Assert.Equal(3, s1e3.Index);

            Assert.Equal(new DateTime(2016, 11, 8), s1e3.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 5), s1e3.DateOriginalReleased);

            #endregion

            #region Episode 4

            var s1e4 = season1.Episodes[3];

            Assert.Equal("Уоткин", s1e4.Title, true);
            Assert.Equal("Watkin", s1e4.OriginalTitle, true);

            Assert.Equal(4, s1e4.Index);

            Assert.Equal(new DateTime(2016, 11, 15), s1e4.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 12), s1e4.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s1e5 = season1.Episodes[4];

            Assert.Equal("Очень эректус", s1e5.Title, true);
            Assert.Equal("Very Erectus", s1e5.OriginalTitle, true);

            Assert.Equal(5, s1e5.Index);

            Assert.Equal(new DateTime(2016, 11, 22), s1e5.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 19), s1e5.DateOriginalReleased);

            #endregion

            #region Episode 6

            var s1e6 = season1.Episodes[5];

            Assert.Equal("Мы всё исправим", s1e6.Title, true);
            Assert.Equal("Fix Everything", s1e6.OriginalTitle, true);

            Assert.Equal(6, s1e6.Index);

            Assert.Equal(new DateTime(2016, 11, 29), s1e6.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 26), s1e6.DateOriginalReleased);

            #endregion

            #region Episode 7

            var s1e7 = season1.Episodes[6];

            Assert.Equal("Взрывоопасный дух", s1e7.Title, true);
            Assert.Equal("Weaponized Soul", s1e7.OriginalTitle, true);

            Assert.Equal(7, s1e7.Index);

            Assert.Equal(new DateTime(2016, 12, 7), s1e7.DateReleased);
            Assert.Equal(new DateTime(2016, 12, 3), s1e7.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s1e8 = season1.Episodes[7];

            Assert.Equal("Два вменяемых парня, занимающиеся нормальными вещами", s1e8.Title, true);
            Assert.Equal("Two Sane Guys Doing Normal Things", s1e8.OriginalTitle, true);

            Assert.Equal(8, s1e8.Index);

            Assert.Equal(new DateTime(2016, 12, 13), s1e8.DateReleased);
            Assert.Equal(new DateTime(2016, 12, 10), s1e8.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.Last();
            Assert.Equal(10, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(10, season2.Episodes.Count);

            #region Episode 1

            var s2e1 = season2.Episodes[0];

            Assert.Equal("Кролик из космоса", s2e1.Title, true);
            Assert.Equal("Space Rabbit", s2e1.OriginalTitle, true);

            Assert.Equal(1, s2e1.Index);

            Assert.Equal(new DateTime(2017, 10, 16), s2e1.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 14), s2e1.DateOriginalReleased);

            #endregion

            #region Episode 2

            var s2e2 = season2.Episodes[1];

            Assert.Equal("Фанаты мокрых кругов", s2e2.Title, true);
            Assert.Equal("Fans of Wet Circles", s2e2.OriginalTitle, true);

            Assert.Equal(2, s2e2.Index);

            Assert.Equal(new DateTime(2017, 10, 23), s2e2.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 21), s2e2.DateOriginalReleased);

            #endregion

            #region Episode 3

            var s2e3 = season2.Episodes[2];

            Assert.Equal("Два сломанных пальца", s2e3.Title, true);
            Assert.Equal("Two Broken Fingers", s2e3.OriginalTitle, true);

            Assert.Equal(3, s2e3.Index);

            Assert.Equal(new DateTime(2017, 10, 30), s2e3.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 28), s2e3.DateOriginalReleased);

            #endregion

            #region Episode 4

            var s2e4 = season2.Episodes[3];

            Assert.Equal("Дом внутри дома", s2e4.Title, true);
            Assert.Equal("The House Within the House", s2e4.OriginalTitle, true);

            Assert.Equal(4, s2e4.Index);

            Assert.Equal(new DateTime(2017, 11, 6), s2e4.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 4), s2e4.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s2e5 = season2.Episodes[4];

            Assert.Equal("Цветные узоры", s2e5.Title, true);
            Assert.Equal("Shapes and Colors", s2e5.OriginalTitle, true);

            Assert.Equal(5, s2e5.Index);

            Assert.Equal(new DateTime(2017, 11, 13), s2e5.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 11), s2e5.DateOriginalReleased);

            #endregion

            #region Episode 6

            var s2e6 = season2.Episodes[5];

            Assert.Equal("Высокая самооценка", s2e6.Title, true);
            Assert.Equal("Girl Power", s2e6.OriginalTitle, true);

            Assert.Equal(6, s2e6.Index);

            Assert.Equal(new DateTime(2017, 11, 20), s2e6.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 18), s2e6.DateOriginalReleased);

            #endregion

            #region Episode 7

            var s2e7 = season2.Episodes[6];

            Assert.Equal("Это не Майами", s2e7.Title, true);
            Assert.Equal("That Is Not Miami", s2e7.OriginalTitle, true);

            Assert.Equal(7, s2e7.Index);

            Assert.Equal(new DateTime(2017, 11, 27), s2e7.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 25), s2e7.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s2e8 = season2.Episodes[7];

            Assert.Equal("Мелкий чел, черные волосы", s2e8.Title, true);
            Assert.Equal("Little Guy, Black Hair", s2e8.OriginalTitle, true);

            Assert.Equal(8, s2e8.Index);

            Assert.Equal(new DateTime(2017, 12, 4), s2e8.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 2), s2e8.DateOriginalReleased);

            #endregion

            #region Episode 9

            var s2e9 = season2.Episodes[8];

            Assert.Equal("Проблемы это плохо", s2e9.Title, true);
            Assert.Equal("Trouble is Bad", s2e9.OriginalTitle, true);

            Assert.Equal(9, s2e9.Index);

            Assert.Equal(new DateTime(2017, 12, 11), s2e9.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 9), s2e9.DateOriginalReleased);

            #endregion

            #region Episode 10

            var s2e10 = season2.Episodes[9];

            Assert.Equal("Классная куртка", s2e10.Title, true);
            Assert.Equal("Nice Jacket", s2e10.OriginalTitle, true);

            Assert.Equal(10, s2e10.Index);

            Assert.Equal(new DateTime(2017, 12, 19), s2e10.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 16), s2e10.DateOriginalReleased);

            #endregion

            #endregion
        }
    }
}