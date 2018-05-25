using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.LostFilm;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow
{
    public class LostFilmExternalProviderTests : TvShowExternalProviderTestsBase
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

        [Fact]
        public async Task Should_Extract_Anger_Management()
        {
            var service = new LostFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Anger_Management");

            #region Tv Show

            var genres = new List<string>
            {
                "Комедия",
            };

            string testDescription = "Чарли, перед возвращением в стан своей бейсбольной команды прошел курс " +
                                     "управления гневом, прежде чем доказать себе и окружающим, что он настоящий лидер команды. " +
                                     "В результате он приводит команду к победе, после чего покидает ее. Выходит, пока Чарли " +
                                     "борется со своим гневом, в его жизни процветает хаос. Всё осложняется его отношениями с " +
                                     "собственным терапевтом и лучшим другом, бывшей женой, чьи позитивные взгляды на будущее, " +
                                     "но при этом плохой выбор мужчин, расстраивают Чарли и их 13-летнюю дочь, имеющую " +
                                     "психические расстройства.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            ValidateTvShow(model, title: "Управление гневом",
                originalTitle: "Anger Management",
                imageUri: "static.lostfilm.tv/Images/172/Posters/poster.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: genres, description: testDescription, seasonsCount: 2);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 10);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, title: "Чарли снова проходит терапию", originalTitle: "Charlie Goes Back to Therapy",
                dateReleased: new DateTime(2012, 7, 2, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 6, 28, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.Index == 5);

            ValidateEpisode(s1E5, title: "Чарли доказывает, что терапия — штука честная",
                originalTitle: "Charlie Tries to Prove Therapy Is Legit",
                dateReleased: new DateTime(2012, 7, 22, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 7, 19, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 10

            var s1E10 = season1.Episodes.First(e => e.Index == 10);

            ValidateEpisode(s1E10, title: "Чарли потянуло на романтику", originalTitle: "Charlie Gets Romantic",
                dateReleased: new DateTime(2012, 8, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 8, 23, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 90);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, title: "Как Чарли психанул на предрожденчике",
                originalTitle: "Charlie Loses it at a Baby Shower",
                dateReleased: new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2013, 1, 17, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 50

            var s2E50 = season2.Episodes.First(e => e.Index == 50);

            ValidateEpisode(s2E50, title: "Чарли и Шон соревнуются из-за девушки",
                originalTitle: "Charlie and Sean Fight Over a Girl",
                dateReleased: new DateTime(2014, 3, 4, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2014, 2, 27, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 100

            var s2E90 = season2.Episodes.First(e => e.Index == 90);

            ValidateEpisode(s2E90, title: "Чарли и сотая серия", originalTitle: "Charlie & the 100th Episode",
                dateReleased: new DateTime(2015, 1, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2014, 12, 22, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }

        [Fact]
        public async Task Should_Extract_Friends()
        {
            var service = new LostFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Friends");

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

        [Fact]
        public async Task Should_Extract_Desperate_Housewives()
        {
            var service = new LostFilmExternalProvider(_retryService);

            var model = await service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Desperate_Housewives");

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

        #endregion
    }
}