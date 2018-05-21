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

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Горизонты", s1E1.Title, true);
            Assert.Equal("Horizons", s1E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 03), s1E1.DateReleased);
            Assert.Equal(new DateTime(2016, 10, 22), s1E1.DateOriginalReleased);

            #endregion

            #region Episode 2

            var s1E2 = season1.Episodes.First(e => e.Index == 2);

            Assert.Equal("Бюро находок", s1E2.Title, true);
            Assert.Equal("Lost and Found", s1E2.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 6), s1E2.DateReleased);
            Assert.Equal(new DateTime(2016, 10, 29), s1E2.DateOriginalReleased);

            #endregion

            #region Episode 3

            var s1E3 = season1.Episodes.First(e => e.Index == 3);

            Assert.Equal("Сумасшедшие фанаты стен", s1E3.Title, true);
            Assert.Equal("Rogue Wall Enthusiasts", s1E3.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 8), s1E3.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 5), s1E3.DateOriginalReleased);

            #endregion

            #region Episode 4

            var s1E4 = season1.Episodes.First(e => e.Index == 4);

            Assert.Equal("Уоткин", s1E4.Title, true);
            Assert.Equal("Watkin", s1E4.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 15), s1E4.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 12), s1E4.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.Index == 5);

            Assert.Equal("Очень эректус", s1E5.Title, true);
            Assert.Equal("Very Erectus", s1E5.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 22), s1E5.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 19), s1E5.DateOriginalReleased);

            #endregion

            #region Episode 6

            var s1E6 = season1.Episodes.First(e => e.Index == 6);

            Assert.Equal("Мы всё исправим", s1E6.Title, true);
            Assert.Equal("Fix Everything", s1E6.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 11, 29), s1E6.DateReleased);
            Assert.Equal(new DateTime(2016, 11, 26), s1E6.DateOriginalReleased);

            #endregion

            #region Episode 7

            var s1E7 = season1.Episodes.First(e => e.Index == 7);

            Assert.Equal("Взрывоопасный дух", s1E7.Title, true);
            Assert.Equal("Weaponized Soul", s1E7.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 12, 7), s1E7.DateReleased);
            Assert.Equal(new DateTime(2016, 12, 3), s1E7.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s1E8 = season1.Episodes.First(e => e.Index == 8);

            Assert.Equal("Два вменяемых парня, занимающиеся нормальными вещами", s1E8.Title, true);
            Assert.Equal("Two Sane Guys Doing Normal Things", s1E8.OriginalTitle, true);

            Assert.Equal(new DateTime(2016, 12, 13), s1E8.DateReleased);
            Assert.Equal(new DateTime(2016, 12, 10), s1E8.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(10, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(10, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Кролик из космоса", s2E1.Title, true);
            Assert.Equal("Space Rabbit", s2E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 10, 16), s2E1.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 14), s2E1.DateOriginalReleased);

            #endregion

            #region Episode 2

            var s2E2 = season2.Episodes.First(e => e.Index == 2);

            Assert.Equal("Фанаты мокрых кругов", s2E2.Title, true);
            Assert.Equal("Fans of Wet Circles", s2E2.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 10, 23), s2E2.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 21), s2E2.DateOriginalReleased);

            #endregion

            #region Episode 3

            var s2E3 = season2.Episodes.First(e => e.Index == 3);

            Assert.Equal("Два сломанных пальца", s2E3.Title, true);
            Assert.Equal("Two Broken Fingers", s2E3.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 10, 30), s2E3.DateReleased);
            Assert.Equal(new DateTime(2017, 10, 28), s2E3.DateOriginalReleased);

            #endregion

            #region Episode 4

            var s2E4 = season2.Episodes.First(e => e.Index == 4);

            Assert.Equal("Дом внутри дома", s2E4.Title, true);
            Assert.Equal("The House Within the House", s2E4.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 6), s2E4.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 4), s2E4.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s2E5 = season2.Episodes.First(e => e.Index == 5);

            Assert.Equal("Цветные узоры", s2E5.Title, true);
            Assert.Equal("Shapes and Colors", s2E5.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 13), s2E5.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 11), s2E5.DateOriginalReleased);

            #endregion

            #region Episode 6

            var s2E6 = season2.Episodes.First(e => e.Index == 6);

            Assert.Equal("Высокая самооценка", s2E6.Title, true);
            Assert.Equal("Girl Power", s2E6.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 20), s2E6.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 18), s2E6.DateOriginalReleased);

            #endregion

            #region Episode 7

            var s2E7 = season2.Episodes.First(e => e.Index == 7);

            Assert.Equal("Это не Майами", s2E7.Title, true);
            Assert.Equal("That Is Not Miami", s2E7.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 11, 27), s2E7.DateReleased);
            Assert.Equal(new DateTime(2017, 11, 25), s2E7.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s2E8 = season2.Episodes.First(e => e.Index == 8);

            Assert.Equal("Мелкий чел, черные волосы", s2E8.Title, true);
            Assert.Equal("Little Guy, Black Hair", s2E8.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 12, 4), s2E8.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 2), s2E8.DateOriginalReleased);

            #endregion

            #region Episode 9

            var s2E9 = season2.Episodes.First(e => e.Index == 9);

            Assert.Equal("Проблемы — это плохо", s2E9.Title, true);
            Assert.Equal("Trouble is Bad", s2E9.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 12, 11), s2E9.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 9), s2E9.DateOriginalReleased);

            #endregion

            #region Episode 10

            var s2E10 = season2.Episodes.First(e => e.Index == 10);

            Assert.Equal("Классная куртка", s2E10.Title, true);
            Assert.Equal("Nice Jacket", s2E10.OriginalTitle, true);

            Assert.Equal(new DateTime(2017, 12, 19), s2E10.DateReleased);
            Assert.Equal(new DateTime(2017, 12, 16), s2E10.DateOriginalReleased);

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

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Чарли снова проходит терапию", s1E1.Title, true);
            Assert.Equal("Charlie Goes Back to Therapy", s1E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 7, 2), s1E1.DateReleased);
            Assert.Equal(new DateTime(2012, 6, 28), s1E1.DateOriginalReleased);

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.Index == 5);

            Assert.Equal("Чарли доказывает, что терапия — штука честная", s1E5.Title, true);
            Assert.Equal("Charlie Tries to Prove Therapy Is Legit", s1E5.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 7, 22), s1E5.DateReleased);
            Assert.Equal(new DateTime(2012, 7, 19), s1E5.DateOriginalReleased);

            #endregion

            #region Episode 10

            var s1E10 = season1.Episodes.First(e => e.Index == 10);

            Assert.Equal("Чарли потянуло на романтику", s1E10.Title, true);
            Assert.Equal("Charlie Gets Romantic", s1E10.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 8, 25), s1E10.DateReleased);
            Assert.Equal(new DateTime(2012, 8, 23), s1E10.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(90, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(90, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Как Чарли психанул на предрожденчике", s2E1.Title, true);
            Assert.Equal("Charlie Loses it at a Baby Shower", s2E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2013, 1, 21), s2E1.DateReleased);
            Assert.Equal(new DateTime(2013, 1, 17), s2E1.DateOriginalReleased);

            #endregion

            #region Episode 50

            var s2E50 = season2.Episodes.First(e => e.Index == 50);

            Assert.Equal("Чарли и Шон соревнуются из-за девушки", s2E50.Title, true);
            Assert.Equal("Charlie and Sean Fight Over a Girl", s2E50.OriginalTitle, true);

            Assert.Equal(new DateTime(2014, 3, 4), s2E50.DateReleased);
            Assert.Equal(new DateTime(2014, 2, 27), s2E50.DateOriginalReleased);

            #endregion

            #region Episode 100

            var s2E90 = season2.Episodes.First(e => e.Index == 90);

            Assert.Equal("Чарли и сотая серия", s2E90.Title, true);
            Assert.Equal("Charlie & the 100th Episode", s2E90.OriginalTitle, true);

            Assert.Equal(new DateTime(2015, 1, 19), s2E90.DateReleased);
            Assert.Equal(new DateTime(2014, 12, 22), s2E90.DateOriginalReleased);

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

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия, где Моника берет новую соседку", s1E1.Title, true);
            Assert.Equal("The One Where Monica Gets A Roommate", s1E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 14), s1E1.DateReleased);
            Assert.Equal(new DateTime(1994, 9, 22), s1E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с дюжиной лазаний", s1E12.Title, true);
            Assert.Equal("The One With The Dozen Lasagnas", s1E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 14), s1E12.DateReleased);
            Assert.Equal(new DateTime(1995, 1, 12), s1E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s1E24 = season1.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия, где Рейчел понимает", s1E24.Title, true);
            Assert.Equal("The One Where Rachel Finds Out", s1E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 14), s1E24.DateReleased);
            Assert.Equal(new DateTime(1995, 5, 18), s1E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(24, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(24, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с новой подругой Росса", s2E1.Title, true);
            Assert.Equal("The One With Ross's New Girlfriend", s2E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 15), s2E1.DateReleased);
            Assert.Equal(new DateTime(1995, 9, 21), s2E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s2E12 = season2.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия после Суперкубка. Часть 1", s2E12.Title, true);
            Assert.Equal("The One After The Super Bowl (1)", s2E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 15), s2E12.DateReleased);
            Assert.Equal(new DateTime(1996, 1, 28), s2E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s2E24 = season2.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия со свадьбой Барри и Минди", s2E24.Title, true);
            Assert.Equal("The One With Barry And Mindy's Wedding", s2E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 15), s2E24.DateReleased);
            Assert.Equal(new DateTime(1996, 5, 16), s2E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);
            Assert.Equal(25, season3.TotalEpisodesCount);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(25, season3.Episodes.Count);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с фантазией о принцессе Лейе", s3E1.Title, true);
            Assert.Equal("The One With The Princess Leia Fantasy", s3E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 18), s3E1.DateReleased);
            Assert.Equal(new DateTime(1996, 9, 16), s3E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s3E12 = season3.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с ревностью всех", s3E12.Title, true);
            Assert.Equal("The One With All The Jealousy", s3E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 18), s3E12.DateReleased);
            Assert.Equal(new DateTime(1997, 1, 16), s3E12.DateOriginalReleased);

            #endregion

            #region Episode 25

            var s3E25 = season3.Episodes.First(e => e.Index == 25);

            Assert.Equal("Серия на пляже", s3E25.Title, true);
            Assert.Equal("The One At The Beach", s3E25.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 18), s3E25.DateReleased);
            Assert.Equal(new DateTime(1997, 5, 15), s3E25.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);
            Assert.Equal(24, season4.TotalEpisodesCount);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(24, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с медузой", s4E1.Title, true);
            Assert.Equal("The One With The Jellyfish", s4E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 19), s4E1.DateReleased);
            Assert.Equal(new DateTime(1997, 9, 25), s4E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s4E12 = season4.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с эмбрионами", s4E12.Title, true);
            Assert.Equal("The One With The Embryos", s4E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 19), s4E12.DateReleased);
            Assert.Equal(new DateTime(1998, 1, 15), s4E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s4E24 = season4.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия со свадьбой Росса. Часть 2", s4E24.Title, true);
            Assert.Equal("The One With Ross's Wedding (2)", s4E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 19), s4E24.DateReleased);
            Assert.Equal(new DateTime(1998, 5, 7), s4E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);
            Assert.Equal(24, season5.TotalEpisodesCount);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(24, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после того, как Росс назвал имя Рэйчел", s5E1.Title, true);
            Assert.Equal("The One After Ross Says Rachel", s5E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 22), s5E1.DateReleased);
            Assert.Equal(new DateTime(1998, 9, 24), s5E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s5E12 = season5.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с рабочим смехом Чендлера", s5E12.Title, true);
            Assert.Equal("The One With Chandler's Work Laugh", s5E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 22), s5E12.DateReleased);
            Assert.Equal(new DateTime(1999, 1, 21), s5E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s5E24 = season5.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия в Вегасе. Часть 2", s5E24.Title, true);
            Assert.Equal("The One In Vegas (2)", s5E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 22), s5E24.DateReleased);
            Assert.Equal(new DateTime(1999, 5, 20), s5E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);
            Assert.Equal(25, season6.TotalEpisodesCount);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(25, season6.Episodes.Count);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после Вегаса", s6E1.Title, true);
            Assert.Equal("The One After Vegas", s6E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 25), s6E1.DateReleased);
            Assert.Equal(new DateTime(1999, 9, 23), s6E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s6E12 = season6.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия c шуткой", s6E12.Title, true);
            Assert.Equal("The One With The Joke", s6E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 25), s6E12.DateReleased);
            Assert.Equal(new DateTime(2000, 1, 13), s6E12.DateOriginalReleased);

            #endregion

            #region Episode 25

            var s6E25 = season6.Episodes.First(e => e.Index == 25);

            Assert.Equal("Серия с предложением. Часть 2", s6E25.Title, true);
            Assert.Equal("The One With The Proposal (2)", s6E25.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 25), s6E25.DateReleased);
            Assert.Equal(new DateTime(2000, 5, 18), s6E25.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);
            Assert.Equal(24, season7.TotalEpisodesCount);

            Assert.NotNull(season7.Episodes);
            Assert.Equal(24, season7.Episodes.Count);

            #region Episode 1

            var s7E1 = season7.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия с вечеринкой Моники", s7E1.Title, true);
            Assert.Equal("The One With Monica's Thunder", s7E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 27), s7E1.DateReleased);
            Assert.Equal(new DateTime(2000, 10, 12), s7E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s7E12 = season7.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия, в которой никто не спит", s7E12.Title, true);
            Assert.Equal("The One Where They're Up All Night", s7E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 27), s7E12.DateReleased);
            Assert.Equal(new DateTime(2001, 1, 11), s7E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s7E24 = season7.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия со свадьбой Моники и Чендлера. Часть 2", s7E24.Title, true);
            Assert.Equal("The One With Chandler And Monica's Wedding (2)", s7E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2006, 12, 27), s7E24.DateReleased);
            Assert.Equal(new DateTime(2001, 5, 17), s7E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);
            Assert.Equal(24, season8.TotalEpisodesCount);

            Assert.NotNull(season8.Episodes);
            Assert.Equal(24, season8.Episodes.Count);

            #region Episode 1

            var s8E1 = season8.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после слов «Я согласен»", s8E1.Title, true);
            Assert.Equal("The One After I Do", s8E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 3), s8E1.DateReleased);
            Assert.Equal(new DateTime(2001, 9, 27), s8E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s8E12 = season8.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия, в которой Джо идет на свидание с Рэйчел", s8E12.Title, true);
            Assert.Equal("The One Where Joey Dates Rachel", s8E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 3), s8E12.DateReleased);
            Assert.Equal(new DateTime(2002, 1, 10), s8E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s8E24 = season8.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия, в которой рождается ребёнок. Часть 2", s8E24.Title, true);
            Assert.Equal("The One Where Rachel Has A Baby (2)", s8E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 3), s8E24.DateReleased);
            Assert.Equal(new DateTime(2002, 5, 16), s8E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 9

            var season9 = model.Seasons.First(s => s.Index == 9);
            Assert.Equal(24, season9.TotalEpisodesCount);

            Assert.NotNull(season9.Episodes);
            Assert.Equal(24, season9.Episodes.Count);

            #region Episode 1

            var s9E1 = season9.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия, в которой никто не делает предложения", s9E1.Title, true);
            Assert.Equal("The One Where No One Proposes", s9E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 8), s9E1.DateReleased);
            Assert.Equal(new DateTime(2002, 9, 26), s9E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s9E12 = season9.Episodes.First(e => e.Index == 12);

            Assert.Equal("Серия с крысами Фиби", s9E12.Title, true);
            Assert.Equal("The One With Phoebe's Rats", s9E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 8), s9E12.DateReleased);
            Assert.Equal(new DateTime(2003, 1, 16), s9E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s9E24 = season9.Episodes.First(e => e.Index == 24);

            Assert.Equal("Серия на Барбадосе. Часть 2", s9E24.Title, true);
            Assert.Equal("The One In Barbados (2)", s9E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 8), s9E24.DateReleased);
            Assert.Equal(new DateTime(2003, 5, 15), s9E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 10

            var season10 = model.Seasons.First(s => s.Index == 10);
            Assert.Equal(18, season10.TotalEpisodesCount);

            Assert.NotNull(season10.Episodes);
            Assert.Equal(18, season10.Episodes.Count);

            #region Episode 1

            var s10E1 = season10.Episodes.First(e => e.Index == 1);

            Assert.Equal("Серия после поцелуя Джо и Рейчел", s10E1.Title, true);
            Assert.Equal("The One After Joey And Rachel Kiss", s10E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 10), s10E1.DateReleased);
            Assert.Equal(new DateTime(2003, 9, 25), s10E1.DateOriginalReleased);

            #endregion

            #region Episode 9

            var s9E9 = season10.Episodes.First(e => e.Index == 9);

            Assert.Equal("Серия с суррогатной матерью", s9E9.Title, true);
            Assert.Equal("The One With The Birth Mother", s9E9.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 10), s9E9.DateReleased);
            Assert.Equal(new DateTime(2004, 1, 8), s9E9.DateOriginalReleased);

            #endregion

            #region Episode 18

            var s9E18 = season10.Episodes.First(e => e.Index == 18);

            Assert.Equal("Последняя серия. Часть 2", s9E18.Title, true);
            Assert.Equal("The Last One: Part 2", s9E18.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 1, 10), s9E18.DateReleased);
            Assert.Equal(new DateTime(2004, 5, 5), s9E18.DateOriginalReleased);

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

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            Assert.Equal("Пилотная", s1E1.Title, true);
            Assert.Equal("Pilot", s1E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 2, 13), s1E1.DateReleased);
            Assert.Equal(new DateTime(2004, 10, 3), s1E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s1E12 = season1.Episodes.First(e => e.Index == 12);

            Assert.Equal("Каждый день немного смерти", s1E12.Title, true);
            Assert.Equal("Every Day a Little Death", s1E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 3, 2), s1E12.DateReleased);
            Assert.Equal(new DateTime(2005, 1, 16), s1E12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s1E23 = season1.Episodes.First(e => e.Index == 23);

            Assert.Equal("В один прекрасный день", s1E23.Title, true);
            Assert.Equal("One Wonderful Day", s1E23.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 4, 1), s1E23.DateReleased);
            Assert.Equal(new DateTime(2005, 5, 22), s1E23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);
            Assert.Equal(24, season2.TotalEpisodesCount);

            Assert.NotNull(season2.Episodes);
            Assert.Equal(24, season2.Episodes.Count);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            Assert.Equal("Следующий", s2E1.Title, true);
            Assert.Equal("Next", s2E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 7, 24), s2E1.DateReleased);
            Assert.Equal(new DateTime(2005, 9, 25), s2E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s2E12 = season2.Episodes.First(e => e.Index == 12);

            Assert.Equal("У нас все будет хорошо", s2E12.Title, true);
            Assert.Equal("We're Gonna Be All Right", s2E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 7, 29), s2E12.DateReleased);
            Assert.Equal(new DateTime(2006, 1, 15), s2E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s2E24 = season2.Episodes.First(e => e.Index == 24);

            Assert.Equal("Помни: часть 2", s2E24.Title, true);
            Assert.Equal("Remember: Part 2", s2E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 7, 31), s2E24.DateReleased);
            Assert.Equal(new DateTime(2006, 5, 21), s2E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 3

            var season3 = model.Seasons.First(s => s.Index == 3);
            Assert.Equal(23, season3.TotalEpisodesCount);

            Assert.NotNull(season3.Episodes);
            Assert.Equal(23, season3.Episodes.Count);

            #region Episode 1

            var s3E1 = season3.Episodes.First(e => e.Index == 1);

            Assert.Equal("Слышишь, дождь стучит по крыше?", s3E1.Title, true);
            Assert.Equal("Listen to the Rain on the Roof", s3E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 6, 7), s3E1.DateReleased);
            Assert.Equal(new DateTime(2006, 9, 24), s3E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s3E12 = season3.Episodes.First(e => e.Index == 12);

            Assert.Equal("Неприятное соседство", s3E12.Title, true);
            Assert.Equal("Not While I'm Around", s3E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2007, 8, 7), s3E12.DateReleased);
            Assert.Equal(new DateTime(2007, 1, 14), s3E12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s3E23 = season3.Episodes.First(e => e.Index == 23);

            Assert.Equal("Свадебная суета", s3E23.Title, true);
            Assert.Equal("Getting Married Today", s3E23.OriginalTitle, true);

            Assert.Equal(new DateTime(2008, 7, 19), s3E23.DateReleased);
            Assert.Equal(new DateTime(2007, 5, 20), s3E23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 4

            var season4 = model.Seasons.First(s => s.Index == 4);
            Assert.Equal(17, season4.TotalEpisodesCount);

            Assert.NotNull(season4.Episodes);
            Assert.Equal(17, season4.Episodes.Count);

            #region Episode 1

            var s4E1 = season4.Episodes.First(e => e.Index == 1);

            Assert.Equal("Теперь ты знаешь", s4E1.Title, true);
            Assert.Equal("Now You Know", s4E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s4E1.DateReleased);
            Assert.Equal(new DateTime(2007, 9, 30), s4E1.DateOriginalReleased);

            #endregion

            #region Episode 8

            var s4E8 = season4.Episodes.First(e => e.Index == 8);

            Assert.Equal("Далекое прошлое", s4E8.Title, true);
            Assert.Equal("Distant Past", s4E8.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s4E8.DateReleased);
            Assert.Equal(new DateTime(2007, 11, 25), s4E8.DateOriginalReleased);

            #endregion

            #region Episode 17

            var s4E17 = season4.Episodes.First(e => e.Index == 17);

            Assert.Equal("Свобода", s4E17.Title, true);
            Assert.Equal("Free", s4E17.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s4E17.DateReleased);
            Assert.Equal(new DateTime(2008, 5, 18), s4E17.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 5

            var season5 = model.Seasons.First(s => s.Index == 5);
            Assert.Equal(24, season5.TotalEpisodesCount);

            Assert.NotNull(season5.Episodes);
            Assert.Equal(24, season5.Episodes.Count);

            #region Episode 1

            var s5E1 = season5.Episodes.First(e => e.Index == 1);

            Assert.Equal("Завтра будет хорошо", s5E1.Title, true);
            Assert.Equal("You're Gonna Love Tomorrow", s5E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s5E1.DateReleased);
            Assert.Equal(new DateTime(2008, 9, 28), s5E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s5E12 = season5.Episodes.First(e => e.Index == 12);

            Assert.Equal("Прием! Прием!", s5E12.Title, true);
            Assert.Equal("Connect! Connect!", s5E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s5E12.DateReleased);
            Assert.Equal(new DateTime(2009, 1, 11), s5E12.DateOriginalReleased);

            #endregion

            #region Episode 24

            var s5E24 = season5.Episodes.First(e => e.Index == 24);

            Assert.Equal("В плену иллюзий", s5E24.Title, true);
            Assert.Equal("If It's Only in Your Head", s5E24.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 25), s5E24.DateReleased);
            Assert.Equal(new DateTime(2009, 5, 17), s5E24.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 6

            var season6 = model.Seasons.First(s => s.Index == 6);
            Assert.Equal(23, season6.TotalEpisodesCount);

            Assert.NotNull(season6.Episodes);
            Assert.Equal(23, season6.Episodes.Count);

            #region Episode 1

            var s6E1 = season6.Episodes.First(e => e.Index == 1);

            Assert.Equal("Лучшее — враг хорошего!", s6E1.Title, true);
            Assert.Equal("Nice Is Different Than Good", s6E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2009, 11, 27), s6E1.DateReleased);
            Assert.Equal(new DateTime(2009, 9, 27), s6E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s6E12 = season6.Episodes.First(e => e.Index == 12);

            Assert.Equal("Придется пойти на хитрость", s6E12.Title, true);
            Assert.Equal("You Gotta Get a Gimmick", s6E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2010, 1, 16), s6E12.DateReleased);
            Assert.Equal(new DateTime(2010, 1, 10), s6E12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s6E23 = season6.Episodes.First(e => e.Index == 23);

            Assert.Equal("Видимо, это прощание", s6E23.Title, true);
            Assert.Equal("I Guess This Is Goodbye", s6E23.OriginalTitle, true);

            Assert.Equal(new DateTime(2010, 6, 3), s6E23.DateReleased);
            Assert.Equal(new DateTime(2010, 5, 16), s6E23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 7

            var season7 = model.Seasons.First(s => s.Index == 7);
            Assert.Equal(23, season7.TotalEpisodesCount);

            Assert.NotNull(season7.Episodes);
            Assert.Equal(23, season7.Episodes.Count);

            #region Episode 1

            var s7E1 = season7.Episodes.First(e => e.Index == 1);

            Assert.Equal("Помнишь Пола?", s7E1.Title, true);
            Assert.Equal("Remember Paul?", s7E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2010, 10, 2), s7E1.DateReleased);
            Assert.Equal(new DateTime(2010, 9, 26), s7E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s7E12 = season7.Episodes.First(e => e.Index == 12);

            Assert.Equal("Где мое место?", s7E12.Title, true);
            Assert.Equal("Where Do I Belong?", s7E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2011, 1, 17), s7E12.DateReleased);
            Assert.Equal(new DateTime(2011, 1, 9), s7E12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s7E23 = season7.Episodes.First(e => e.Index == 23);

            Assert.Equal("Приходите на ужин", s7E23.Title, true);
            Assert.Equal("Come on Over for Dinner", s7E23.OriginalTitle, true);

            Assert.Equal(new DateTime(2011, 6, 14), s7E23.DateReleased);
            Assert.Equal(new DateTime(2011, 5, 15), s7E23.DateOriginalReleased);

            #endregion

            #endregion

            #region Season 8

            var season8 = model.Seasons.First(s => s.Index == 8);
            Assert.Equal(23, season8.TotalEpisodesCount);

            Assert.NotNull(season8.Episodes);
            Assert.Equal(23, season8.Episodes.Count);

            #region Episode 1

            var s8E1 = season8.Episodes.First(e => e.Index == 1);

            Assert.Equal("Тайны, которые я не хочу знать", s8E1.Title, true);
            Assert.Equal("Secrets That I Never Want to Know", s8E1.OriginalTitle, true);

            Assert.Equal(new DateTime(2011, 10, 19), s8E1.DateReleased);
            Assert.Equal(new DateTime(2011, 9, 25), s8E1.DateOriginalReleased);

            #endregion

            #region Episode 12

            var s8E12 = season8.Episodes.First(e => e.Index == 12);

            Assert.Equal("Что хорошего в том, чтобы быть хорошей", s8E12.Title, true);
            Assert.Equal("What's the Good of Being Good", s8E12.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 1, 25), s8E12.DateReleased);
            Assert.Equal(new DateTime(2012, 1, 22), s8E12.DateOriginalReleased);

            #endregion

            #region Episode 23

            var s8E23 = season8.Episodes.First(e => e.Index == 23);

            Assert.Equal("Последний штрих", s8E23.Title, true);
            Assert.Equal("Finishing the Hat", s8E23.OriginalTitle, true);

            Assert.Equal(new DateTime(2012, 5, 17), s8E23.DateReleased);
            Assert.Equal(new DateTime(2012, 5, 13), s8E23.DateOriginalReleased);

            #endregion

            #endregion
        }

        #endregion
    }
}