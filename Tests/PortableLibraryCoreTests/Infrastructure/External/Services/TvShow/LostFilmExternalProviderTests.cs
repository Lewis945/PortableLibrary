using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.LostFilm;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
{
    public class LostFilmExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public LostFilmExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        #region Extract TvShow Tests

        [Fact]
        public async Task Should_Extract_Dirk_Gentlys_Holistic_Detective_Agency()
        {
            var service = new LostFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync(
                "https://www.lostfilm.tv/series/Dirk_Gentlys_Holistic_Detective_Agency/");

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

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

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

            Assert.Equal(testDescription, modelDescription, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(2, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);
            Assert.Equal(8, season1.TotalEpisodesCount);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(8, season1.Episodes.Count);

            #region Episode 1

            var s1e1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Горизонты", s1e1.Title, true);
            Assert.Equal("Horizons", s1e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 03), s1e1.DateReleased);
            Assert.Equal(new DateTime(2016, 10, 22), s1e1.DateOriginalReleased);

            #endregion

            #region Episode 2

            var s1e2 = season1.Episodes.First(e => e.Index == 2);

            Assert.Equal("Бюро находок", s1e2.Title, true);
            Assert.Equal("Lost and Found", s1e2.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 6), s1e2.DateReleased);
            Assert.Equal(new DateTime(2016, 10, 29), s1e2.DateOriginalReleased);

            #endregion

            #region Episode 3

            var s1e3 = season1.Episodes.First(e => e.Index == 3);

            Assert.Equal("Сумасшедшие фанаты стен", s1e3.Title, true);
            Assert.Equal("Rogue Wall Enthusiasts", s1e3.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 8), s1e3.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 5), s1e3.DateOriginalReleased);

            #endregion

            #region Episode 4

            var s1e4 = season1.Episodes.First(e => e.Index == 4);

            Assert.Equal("Уоткин", s1e4.Title, true);
            Assert.Equal("Watkin", s1e4.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 15), s1e4.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 12), s1e4.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s1e5 = season1.Episodes.First(e => e.Index == 5);

            Assert.Equal("Очень эректус", s1e5.Title, true);
            Assert.Equal("Very Erectus", s1e5.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 22), s1e5.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 19), s1e5.DateOriginalReleased);

            #endregion

            #region Episode 6

            var s1e6 = season1.Episodes.First(e => e.Index == 6);

            Assert.Equal("Мы всё исправим", s1e6.Title, true);
            Assert.Equal("Fix Everything", s1e6.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 29), s1e6.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 26), s1e6.DateOriginalReleased);

            #endregion

            #region Episode 7

            var s1e7 = season1.Episodes.First(e => e.Index == 7);

            Assert.Equal("Взрывоопасный дух", s1e7.Title, true);
            Assert.Equal("Weaponized Soul", s1e7.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 12, 7), s1e7.DateReleased);
            Assert.Equal(new DateTime(2016, 12, 3), s1e7.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s1e8 = season1.Episodes.First(e => e.Index == 8);

            Assert.Equal("Два вменяемых парня, занимающиеся нормальными вещами", s1e8.Title, true);
            Assert.Equal("Two Sane Guys Doing Normal Things", s1e8.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 12, 13), s1e8.DateReleased);
            Assert.Equal(new DateTime(2016, 12, 10), s1e8.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(10, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(10, season2.Episodes.Count);

            #region Episode 1

            var s2e1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Кролик из космоса", s2e1.Title, true);
            Assert.Equal("Space Rabbit", s2e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 10, 16), s2e1.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 14), s2e1.DateOriginalReleased);

            #endregion

            #region Episode 2

            var s2e2 = season2.Episodes.First(e => e.Index == 2);

            Assert.Equal("Фанаты мокрых кругов", s2e2.Title, true);
            Assert.Equal("Fans of Wet Circles", s2e2.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 10, 23), s2e2.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 21), s2e2.DateOriginalReleased);

            #endregion

            #region Episode 3

            var s2e3 = season2.Episodes.First(e => e.Index == 3);

            Assert.Equal("Два сломанных пальца", s2e3.Title, true);
            Assert.Equal("Two Broken Fingers", s2e3.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 10, 30), s2e3.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 28), s2e3.DateOriginalReleased);

            #endregion

            #region Episode 4

            var s2e4 = season2.Episodes.First(e => e.Index == 4);

            Assert.Equal("Дом внутри дома", s2e4.Title, true);
            Assert.Equal("The House Within the House", s2e4.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 6), s2e4.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 4), s2e4.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s2e5 = season2.Episodes.First(e => e.Index == 5);

            Assert.Equal("Цветные узоры", s2e5.Title, true);
            Assert.Equal("Shapes and Colors", s2e5.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 13), s2e5.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 11), s2e5.DateOriginalReleased);

            #endregion

            #region Episode 6

            var s2e6 = season2.Episodes.First(e => e.Index == 6);

            Assert.Equal("Высокая самооценка", s2e6.Title, true);
            Assert.Equal("Girl Power", s2e6.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 20), s2e6.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 18), s2e6.DateOriginalReleased);

            #endregion

            #region Episode 7

            var s2e7 = season2.Episodes.First(e => e.Index == 7);

            Assert.Equal("Это не Майами", s2e7.Title, true);
            Assert.Equal("That Is Not Miami", s2e7.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 27), s2e7.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 25), s2e7.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s2e8 = season2.Episodes.First(e => e.Index == 8);

            Assert.Equal("Мелкий чел, черные волосы", s2e8.Title, true);
            Assert.Equal("Little Guy, Black Hair", s2e8.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 12, 4), s2e8.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 2), s2e8.DateOriginalReleased);

            #endregion

            #region Episode 9

            var s2e9 = season2.Episodes.First(e => e.Index == 9);

            Assert.Equal("Проблемы — это плохо", s2e9.Title, true);
            Assert.Equal("Trouble is Bad", s2e9.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 12, 11), s2e9.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 9), s2e9.DateOriginalReleased);

            #endregion

            #region Episode 10

            var s2e10 = season2.Episodes.First(e => e.Index == 10);

            Assert.Equal("Классная куртка", s2e10.Title, true);
            Assert.Equal("Nice Jacket", s2e10.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 12, 19), s2e10.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 16), s2e10.DateOriginalReleased);

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Anger_Management()
        {
            var service = new LostFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Anger_Management");

            #region Tv Show

            Assert.Equal("static.lostfilm.tv/Images/172/Posters/poster.jpg",
                model.ImageUri, true);

            Assert.Equal("Управление гневом", model.Title, true);
            Assert.Equal("Anger Management", model.OriginalTitle, true);

            Assert.True(model.IsComplete);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Комедия", item, true)
            );

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            string testDescription = "Чарли, перед возвращением в стан своей бейсбольной команды прошел курс " +
                                     "управления гневом, прежде чем доказать себе и окружающим, что он настоящий лидер команды. " +
                                     "В результате он приводит команду к победе, после чего покидает ее. Выходит, пока Чарли " +
                                     "борется со своим гневом, в его жизни процветает хаос. Всё осложняется его отношениями с " +
                                     "собственным терапевтом и лучшим другом, бывшей женой, чьи позитивные взгляды на будущее, " +
                                     "но при этом плохой выбор мужчин, расстраивают Чарли и их 13-летнюю дочь, имеющую " +
                                     "психические расстройства.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription, modelDescription, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(2, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);
            Assert.Equal(10, season1.TotalEpisodesCount);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(10, season1.Episodes.Count);

            #region Episode 1

            var s1e1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Чарли снова проходит терапию", s1e1.Title, true);
            Assert.Equal("Charlie Goes Back to Therapy", s1e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 7, 2), s1e1.DateReleased);
            Assert.Equal(new DateTime(2012, 6, 28), s1e1.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s1e5 = season1.Episodes.First(e => e.Index == 5);

            Assert.Equal("Чарли доказывает, что терапия — штука честная", s1e5.Title, true);
            Assert.Equal("Charlie Tries to Prove Therapy Is Legit", s1e5.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 7, 22), s1e5.DateReleased);
            Assert.Equal(new DateTime(2012, 7, 19), s1e5.DateOriginalReleased);

            #endregion

            #region Episode 10

            var s1e10 = season1.Episodes.First(e => e.Index == 10);

            Assert.Equal("Чарли потянуло на романтику", s1e10.Title, true);
            Assert.Equal("Charlie Gets Romantic", s1e10.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 8, 25), s1e10.DateReleased);
            Assert.Equal(new DateTime(2012, 8, 23), s1e10.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(90, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(90, season2.Episodes.Count);

            #region Episode 1

            var s2e1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Как Чарли психанул на предрожденчике", s2e1.Title, true);
            Assert.Equal("Charlie Loses it at a Baby Shower", s2e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2013, 1, 21), s2e1.DateReleased);
            Assert.Equal(new DateTime(2013, 1, 17), s2e1.DateOriginalReleased);

            #endregion

            #region Episode 50

            var s2e50 = season2.Episodes.First(e => e.Index == 50);

            Assert.Equal("Чарли и Шон соревнуются из-за девушки", s2e50.Title, true);
            Assert.Equal("Charlie and Sean Fight Over a Girl", s2e50.OriginalTitle, true);

            Assert.Equal(new DateTime(2014, 3, 4), s2e50.DateReleased);
            Assert.Equal(new DateTime(2014, 2, 27), s2e50.DateOriginalReleased);

            #endregion

            #region Episode 100

            var s2e90 = season2.Episodes.First(e => e.Index == 90);

            Assert.Equal("Чарли и сотая серия", s2e90.Title, true);
            Assert.Equal("Charlie & the 100th Episode", s2e90.OriginalTitle, true);

            Assert.Equal(new DateTime(2015, 1, 19), s2e90.DateReleased);
            Assert.Equal(new DateTime(2014, 12, 22), s2e90.DateOriginalReleased);

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Friends()
        {
            var service = new LostFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Friends");

            #region Tv Show

            Assert.Equal("static.lostfilm.tv/Images/72/Posters/poster.jpg",
                model.ImageUri, true);

            Assert.Equal("Друзья", model.Title, true);
            Assert.Equal("Friends", model.OriginalTitle, true);

            Assert.True(model.IsComplete);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Комедия", item, true),
                item => Assert.Equal("Семейный", item, true)
            );

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

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

            Assert.Equal(testDescription, modelDescription, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(10, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);
            Assert.Equal(24, season1.TotalEpisodesCount);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(24, season1.Episodes.Count);

            #region Episode 1

            var s1e1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия, где Моника берет новую соседку", s1e1.Title, true);
            Assert.Equal("The One Where Monica Gets A Roommate", s1e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 14), s1e1.DateReleased);
            Assert.Equal(new DateTime(1994, 9, 22), s1e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s1e12 = season1.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с дюжиной лазаний", s1e12.Title, true);
            Assert.Equal("The One With The Dozen Lasagnas", s1e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 14), s1e12.DateReleased);
            Assert.Equal(new DateTime(1995, 1, 12), s1e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s1e24 = season1.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия, где Рейчел понимает", s1e24.Title, true);
            Assert.Equal("The One Where Rachel Finds Out", s1e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 14), s1e24.DateReleased);
            Assert.Equal(new DateTime(1995, 5, 18), s1e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(24, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(24, season2.Episodes.Count);

            #region Episode 1

            var s2e1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с новой подругой Росса", s2e1.Title, true);
            Assert.Equal("The One With Ross's New Girlfriend", s2e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 15), s2e1.DateReleased);
            Assert.Equal(new DateTime(1995, 9, 21), s2e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s2e12 = season2.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия после Суперкубка. Часть 1", s2e12.Title, true);
            Assert.Equal("The One After The Super Bowl (1)", s2e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 15), s2e12.DateReleased);
            Assert.Equal(new DateTime(1996, 1, 28), s2e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s2e24 = season2.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия со свадьбой Барри и Минди", s2e24.Title, true);
            Assert.Equal("The One With Barry And Mindy's Wedding", s2e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 15), s2e24.DateReleased);
            Assert.Equal(new DateTime(1996, 5, 16), s2e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);
            Assert.Equal(25, season3.TotalEpisodesCount);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(25, season3.Episodes.Count);

            #region Episode 1

            var s3e1 = season3.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с фантазией о принцессе Лейе", s3e1.Title, true);
            Assert.Equal("The One With The Princess Leia Fantasy", s3e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 18), s3e1.DateReleased);
            Assert.Equal(new DateTime(1996, 9, 16), s3e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s3e12 = season3.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с ревностью всех", s3e12.Title, true);
            Assert.Equal("The One With All The Jealousy", s3e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 18), s3e12.DateReleased);
            Assert.Equal(new DateTime(1997, 1, 16), s3e12.DateOriginalReleased);

            #endregion

            #region Episode 25

            var s3e25 = season3.Episodes.First(e => e.Index == 25);

            Assert.Equal("Серия на пляже", s3e25.Title, true);
            Assert.Equal("The One At The Beach", s3e25.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 18), s3e25.DateReleased);
            Assert.Equal(new DateTime(1997, 5, 15), s3e25.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);
            Assert.Equal(24, season4.TotalEpisodesCount);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(24, season4.Episodes.Count);

            #region Episode 1

            var s4e1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с медузой", s4e1.Title, true);
            Assert.Equal("The One With The Jellyfish", s4e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 19), s4e1.DateReleased);
            Assert.Equal(new DateTime(1997, 9, 25), s4e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s4e12 = season4.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с эмбрионами", s4e12.Title, true);
            Assert.Equal("The One With The Embryos", s4e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 19), s4e12.DateReleased);
            Assert.Equal(new DateTime(1998, 1, 15), s4e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s4e24 = season4.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия со свадьбой Росса. Часть 2", s4e24.Title, true);
            Assert.Equal("The One With Ross's Wedding (2)", s4e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 19), s4e24.DateReleased);
            Assert.Equal(new DateTime(1998, 5, 7), s4e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);
            Assert.Equal(24, season5.TotalEpisodesCount);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(24, season5.Episodes.Count);

            #region Episode 1

            var s5e1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после того, как Росс назвал имя Рэйчел", s5e1.Title, true);
            Assert.Equal("The One After Ross Says Rachel", s5e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 22), s5e1.DateReleased);
            Assert.Equal(new DateTime(1998, 9, 24), s5e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s5e12 = season5.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с рабочим смехом Чендлера", s5e12.Title, true);
            Assert.Equal("The One With Chandler's Work Laugh", s5e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 22), s5e12.DateReleased);
            Assert.Equal(new DateTime(1999, 1, 21), s5e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s5e24 = season5.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия в Вегасе. Часть 2", s5e24.Title, true);
            Assert.Equal("The One In Vegas (2)", s5e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 22), s5e24.DateReleased);
            Assert.Equal(new DateTime(1999, 5, 20), s5e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);
            Assert.Equal(25, season6.TotalEpisodesCount);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(25, season6.Episodes.Count);

            #region Episode 1

            var s6e1 = season6.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после Вегаса", s6e1.Title, true);
            Assert.Equal("The One After Vegas", s6e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 25), s6e1.DateReleased);
            Assert.Equal(new DateTime(1999, 9, 23), s6e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s6e12 = season6.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия c шуткой", s6e12.Title, true);
            Assert.Equal("The One With The Joke", s6e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 25), s6e12.DateReleased);
            Assert.Equal(new DateTime(2000, 1, 13), s6e12.DateOriginalReleased);

            #endregion

            #region Episode 25

            var s6e25 = season6.Episodes.First(e => e.Index == 25);

            Assert.Equal("Серия с предложением. Часть 2", s6e25.Title, true);
            Assert.Equal("The One With The Proposal (2)", s6e25.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 25), s6e25.DateReleased);
            Assert.Equal(new DateTime(2000, 5, 18), s6e25.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);
            Assert.Equal(24, season7.TotalEpisodesCount);

            Assert.NotNull(season7.Episodes);
            Assert.Equal(24, season7.Episodes.Count);

            #region Episode 1

            var s7e1 = season7.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с вечеринкой Моники", s7e1.Title, true);
            Assert.Equal("The One With Monica's Thunder", s7e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 27), s7e1.DateReleased);
            Assert.Equal(new DateTime(2000, 10, 12), s7e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s7e12 = season7.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия, в которой никто не спит", s7e12.Title, true);
            Assert.Equal("The One Where They're Up All Night", s7e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 27), s7e12.DateReleased);
            Assert.Equal(new DateTime(2001, 1, 11), s7e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s7e24 = season7.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия со свадьбой Моники и Чендлера. Часть 2", s7e24.Title, true);
            Assert.Equal("The One With Chandler And Monica's Wedding (2)", s7e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 27), s7e24.DateReleased);
            Assert.Equal(new DateTime(2001, 5, 17), s7e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);
            Assert.Equal(24, season8.TotalEpisodesCount);

            Assert.NotNull(season8.Episodes);
            Assert.Equal(24, season8.Episodes.Count);

            #region Episode 1

            var s8e1 = season8.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после слов «Я согласен»", s8e1.Title, true);
            Assert.Equal("The One After I Do", s8e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 3), s8e1.DateReleased);
            Assert.Equal(new DateTime(2001, 9, 27), s8e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s8e12 = season8.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия, в которой Джо идет на свидание с Рэйчел", s8e12.Title, true);
            Assert.Equal("The One Where Joey Dates Rachel", s8e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 3), s8e12.DateReleased);
            Assert.Equal(new DateTime(2002, 1, 10), s8e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s8e24 = season8.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия, в которой рождается ребёнок. Часть 2", s8e24.Title, true);
            Assert.Equal("The One Where Rachel Has A Baby (2)", s8e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 3), s8e24.DateReleased);
            Assert.Equal(new DateTime(2002, 5, 16), s8e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 9

            var season9 = model.Seasons.First(s => s.Index == 9);
            Assert.Equal(24, season9.TotalEpisodesCount);

            Assert.NotNull(season9.Episodes);
            Assert.Equal(24, season9.Episodes.Count);

            #region Episode 1

            var s9e1 = season9.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия, в которой никто не делает предложения", s9e1.Title, true);
            Assert.Equal("The One Where No One Proposes", s9e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 8), s9e1.DateReleased);
            Assert.Equal(new DateTime(2002, 9, 26), s9e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s9e12 = season9.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с крысами Фиби", s9e12.Title, true);
            Assert.Equal("The One With Phoebe's Rats", s9e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 8), s9e12.DateReleased);
            Assert.Equal(new DateTime(2003, 1, 16), s9e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s9e24 = season9.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия на Барбадосе. Часть 2", s9e24.Title, true);
            Assert.Equal("The One In Barbados (2)", s9e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 8), s9e24.DateReleased);
            Assert.Equal(new DateTime(2003, 5, 15), s9e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 10

            var season10 = model.Seasons.First(s => s.Index == 10);
            Assert.Equal(18, season10.TotalEpisodesCount);

            Assert.NotNull(season10.Episodes);
            Assert.Equal(18, season10.Episodes.Count);

            #region Episode 1

            var s10e1 = season10.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после поцелуя Джо и Рейчел", s10e1.Title, true);
            Assert.Equal("The One After Joey And Rachel Kiss", s10e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 10), s10e1.DateReleased);
            Assert.Equal(new DateTime(2003, 9, 25), s10e1.DateOriginalReleased);

            #endregion

            #region Episode 9

            var s9e9 = season10.Episodes.First(e => e.Index == 9);

            Assert.Equal("Серия с суррогатной матерью", s9e9.Title, true);
            Assert.Equal("The One With The Birth Mother", s9e9.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 10), s9e9.DateReleased);
            Assert.Equal(new DateTime(2004, 1, 8), s9e9.DateOriginalReleased);

            #endregion

            #region Episode 18

            var s9e18 = season10.Episodes.First(e => e.Index == 18);

            Assert.Equal("Последняя серия. Часть 2", s9e18.Title, true);
            Assert.Equal("The Last One: Part 2", s9e18.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 10), s9e18.DateReleased);
            Assert.Equal(new DateTime(2004, 5, 5), s9e18.DateOriginalReleased);

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Desperate_Housewives()
        {
            var service = new LostFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Desperate_Housewives");

            #region Tv Show

            Assert.Equal("static.lostfilm.tv/Images/64/Posters/poster.jpg",
                model.ImageUri, true);

            Assert.Equal("Отчаянные домохозяйки", model.Title, true);
            Assert.Equal("Desperate Housewives", model.OriginalTitle, true);

            Assert.True(model.IsComplete);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Драма", item, true),
                item => Assert.Equal("Комедия", item, true)
            );

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

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

            Assert.Equal(testDescription, modelDescription, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(8, model.Seasons.Count);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);
            Assert.Equal(23, season1.TotalEpisodesCount);

            Assert.NotNull(season1.Episodes);
            Assert.Equal(23, season1.Episodes.Count);

            #region Episode 1

            var s1e1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Пилотная", s1e1.Title, true);
            Assert.Equal("Pilot", s1e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 2, 13), s1e1.DateReleased);
            Assert.Equal(new DateTime(2004, 10, 3), s1e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s1e12 = season1.Episodes.First(e => e.Index == 12);

            Assert.Equal("Каждый день немного смерти", s1e12.Title, true);
            Assert.Equal("Every Day a Little Death", s1e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 3, 2), s1e12.DateReleased);
            Assert.Equal(new DateTime(2005, 1, 16), s1e12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s1e23 = season1.Episodes.First(e => e.Index == 23);

            Assert.Equal("В один прекрасный день", s1e23.Title, true);
            Assert.Equal("One Wonderful Day", s1e23.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 4, 1), s1e23.DateReleased);
            Assert.Equal(new DateTime(2005, 5, 22), s1e23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(24, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(24, season2.Episodes.Count);

            #region Episode 1

            var s2e1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Следующий", s2e1.Title, true);
            Assert.Equal("Next", s2e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 7, 24), s2e1.DateReleased);
            Assert.Equal(new DateTime(2005, 9, 25), s2e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s2e12 = season2.Episodes.First(e => e.Index == 12);

            Assert.Equal("У нас все будет хорошо", s2e12.Title, true);
            Assert.Equal("We're Gonna Be All Right", s2e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 7, 29), s2e12.DateReleased);
            Assert.Equal(new DateTime(2006, 1, 15), s2e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s2e24 = season2.Episodes.First(e => e.Index == 24);

            Assert.Equal("Помни: часть 2", s2e24.Title, true);
            Assert.Equal("Remember: Part 2", s2e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 7, 31), s2e24.DateReleased);
            Assert.Equal(new DateTime(2006, 5, 21), s2e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);
            Assert.Equal(23, season3.TotalEpisodesCount);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(23, season3.Episodes.Count);

            #region Episode 1

            var s3e1 = season3.Episodes.First(e => e.Index == 1);

            Assert.Equal("Слышишь, дождь стучит по крыше?", s3e1.Title, true);
            Assert.Equal("Listen to the Rain on the Roof", s3e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 6, 7), s3e1.DateReleased);
            Assert.Equal(new DateTime(2006, 9, 24), s3e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s3e12 = season3.Episodes.First(e => e.Index == 12);

            Assert.Equal("Неприятное соседство", s3e12.Title, true);
            Assert.Equal("Not While I'm Around", s3e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 8, 7), s3e12.DateReleased);
            Assert.Equal(new DateTime(2007, 1, 14), s3e12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s3e23 = season3.Episodes.First(e => e.Index == 23);

            Assert.Equal("Свадебная суета", s3e23.Title, true);
            Assert.Equal("Getting Married Today", s3e23.OriginalTitle, true);

            Assert.Equal(new DateTime(2008, 7, 19), s3e23.DateReleased);
            Assert.Equal(new DateTime(2007, 5, 20), s3e23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);
            Assert.Equal(17, season4.TotalEpisodesCount);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(17, season4.Episodes.Count);

            #region Episode 1

            var s4e1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Теперь ты знаешь", s4e1.Title, true);
            Assert.Equal("Now You Know", s4e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s4e1.DateReleased);
            Assert.Equal(new DateTime(2007, 9, 30), s4e1.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s4e8 = season4.Episodes.First(e => e.Index == 8);

            Assert.Equal("Далекое прошлое", s4e8.Title, true);
            Assert.Equal("Distant Past", s4e8.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s4e8.DateReleased);
            Assert.Equal(new DateTime(2007, 11, 25), s4e8.DateOriginalReleased);

            #endregion

            #region Episode 17

            var s4e17 = season4.Episodes.First(e => e.Index == 17);

            Assert.Equal("Свобода", s4e17.Title, true);
            Assert.Equal("Free", s4e17.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s4e17.DateReleased);
            Assert.Equal(new DateTime(2008, 5, 18), s4e17.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);
            Assert.Equal(24, season5.TotalEpisodesCount);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(24, season5.Episodes.Count);

            #region Episode 1

            var s5e1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Завтра будет хорошо", s5e1.Title, true);
            Assert.Equal("You're Gonna Love Tomorrow", s5e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s5e1.DateReleased);
            Assert.Equal(new DateTime(2008, 9, 28), s5e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s5e12 = season5.Episodes.First(e => e.Index == 12);

            Assert.Equal("Прием! Прием!", s5e12.Title, true);
            Assert.Equal("Connect! Connect!", s5e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s5e12.DateReleased);
            Assert.Equal(new DateTime(2009, 1, 11), s5e12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s5e24 = season5.Episodes.First(e => e.Index == 24);

            Assert.Equal("В плену иллюзий", s5e24.Title, true);
            Assert.Equal("If It's Only in Your Head", s5e24.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s5e24.DateReleased);
            Assert.Equal(new DateTime(2009, 5, 17), s5e24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);
            Assert.Equal(23, season6.TotalEpisodesCount);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(23, season6.Episodes.Count);

            #region Episode 1

            var s6e1 = season6.Episodes.First(e => e.Index == 1);

            Assert.Equal("Лучшее — враг хорошего!", s6e1.Title, true);
            Assert.Equal("Nice Is Different Than Good", s6e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 27), s6e1.DateReleased);
            Assert.Equal(new DateTime(2009, 9, 27), s6e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s6e12 = season6.Episodes.First(e => e.Index == 12);

            Assert.Equal("Придется пойти на хитрость", s6e12.Title, true);
            Assert.Equal("You Gotta Get a Gimmick", s6e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2010, 1, 16), s6e12.DateReleased);
            Assert.Equal(new DateTime(2010, 1, 10), s6e12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s6e23 = season6.Episodes.First(e => e.Index == 23);

            Assert.Equal("Видимо, это прощание", s6e23.Title, true);
            Assert.Equal("I Guess This Is Goodbye", s6e23.OriginalTitle, true);

            Assert.Equal(new DateTime(2010, 6, 3), s6e23.DateReleased);
            Assert.Equal(new DateTime(2010, 5, 16), s6e23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);
            Assert.Equal(23, season7.TotalEpisodesCount);

            Assert.NotNull(season7.Episodes);
            Assert.Equal(23, season7.Episodes.Count);

            #region Episode 1

            var s7e1 = season7.Episodes.First(e => e.Index == 1);

            Assert.Equal("Помнишь Пола?", s7e1.Title, true);
            Assert.Equal("Remember Paul?", s7e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2010, 10, 2), s7e1.DateReleased);
            Assert.Equal(new DateTime(2010, 9, 26), s7e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s7e12 = season7.Episodes.First(e => e.Index == 12);

            Assert.Equal("Где мое место?", s7e12.Title, true);
            Assert.Equal("Where Do I Belong?", s7e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2011, 1, 17), s7e12.DateReleased);
            Assert.Equal(new DateTime(2011, 1, 9), s7e12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s7e23 = season7.Episodes.First(e => e.Index == 23);

            Assert.Equal("Приходите на ужин", s7e23.Title, true);
            Assert.Equal("Come on Over for Dinner", s7e23.OriginalTitle, true);

            Assert.Equal(new DateTime(2011, 6, 14), s7e23.DateReleased);
            Assert.Equal(new DateTime(2011, 5, 15), s7e23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);
            Assert.Equal(23, season8.TotalEpisodesCount);

            Assert.NotNull(season8.Episodes);
            Assert.Equal(23, season8.Episodes.Count);

            #region Episode 1

            var s8e1 = season8.Episodes.First(e => e.Index == 1);

            Assert.Equal("Тайны, которые я не хочу знать", s8e1.Title, true);
            Assert.Equal("Secrets That I Never Want to Know", s8e1.OriginalTitle, true);

            Assert.Equal(new DateTime(2011, 10, 19), s8e1.DateReleased);
            Assert.Equal(new DateTime(2011, 9, 25), s8e1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s8e12 = season8.Episodes.First(e => e.Index == 12);

            Assert.Equal("Что хорошего в том, чтобы быть хорошей", s8e12.Title, true);
            Assert.Equal("What's the Good of Being Good", s8e12.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 1, 25), s8e12.DateReleased);
            Assert.Equal(new DateTime(2012, 1, 22), s8e12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s8e23 = season8.Episodes.First(e => e.Index == 23);

            Assert.Equal("Последний штрих", s8e23.Title, true);
            Assert.Equal("Finishing the Hat", s8e23.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 5, 17), s8e23.DateReleased);
            Assert.Equal(new DateTime(2012, 5, 13), s8e23.DateOriginalReleased);

            #endregion

            #endregion
        }

        #endregion
    }
}