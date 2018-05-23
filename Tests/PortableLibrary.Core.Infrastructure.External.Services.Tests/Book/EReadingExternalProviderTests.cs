using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.Book.EReading;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.Book
{
    public class EReadingExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public EReadingExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        #region Extract Book Tests

        [Fact]
        public async Task Should_Extract_Book_Vasiliy_Mahanenko_Barleona()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.e-reading.club/book.php?book=1016974");

            Assert.Equal("https://www.e-reading.club/cover/1016/1016974.png",
                model.ImageUri, true);

            Assert.Equal("Барлиона", model.Title, true);

            Assert.Equal("Маханенко Василий", model.Author, true);

            Assert.Equal("Мир Барлионы", model.Serie, true);

            Assert.Equal("https://www.e-reading.club/series.aspx/1002172/Mir_Barliony.html", model.TrackingUri, true);

            Assert.Equal(1, model.Index);

            Assert.Collection(model.Genres,
                item => Assert.Equal("боевик", item, true),
                item => Assert.Equal("фэнтези", item, true)
            );

            Assert.Equal("Барлиона. Виртуальный мир, наполненный приключениями, битвами, " +
                         "монстрами и конечно же игроками. Для многих Барлиона заменила собой реальность, " +
                         "ведь в ней сбываются любые желания человека: магия, эльфы, драконы. В мире " +
                         "существует только одно правило — игрок не чувствует боли. Но у каждого правила " +
                         "есть исключения, и для части игроков Барлиона стала адом. Ведь они — преступники, " +
                         "играющие в самом тяжелом режиме с включенными ощущениями.",
                model.Description, true);

            Assert.Null(model.ReleaseYear);

            Assert.Null(model.DownloadLinks);
        }

        [Fact]
        public async Task Should_Extract_Book_Zukov_Vitaliy_Konklav_Besmertnih_Proba_Sil()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.e-reading.club/book.php?book=86632");

            Assert.Equal("https://www.e-reading.club/cover/86/86632.jpg",
                model.ImageUri, true);

            Assert.Equal("Конклав Бессмертных. Проба сил", model.Title, true);

            Assert.Equal("Зыков Виталий", model.Author, true);

            Assert.Equal("Война за выживание", model.Serie, true);

            Assert.Equal("https://www.e-reading.club/series.aspx/3334/Voyna_za_vyzhivanie.html", model.TrackingUri);

            Assert.Equal(2, model.Index);

            Assert.Collection(model.Genres,
                item => Assert.Equal("фэнтези", item, true)
            );

            Assert.Equal("Трудно уцелеть в мире победившей Тьмы. На улицах Сосновска льется " +
                         "кровь и творится злая волшба, любой может стать жертвой монстра. Однако среди " +
                         "горожан по-прежнему нет единства. Кто-то борется за право оставаться человеком, " +
                         "а кто-то готов на все ради власти. Но если нет героев в белых одеждах, рыцарей " +
                         "без страха и упрека, вместо них приходят обычные люди. Те, кому надоело трястись " +
                         "от страха, кого не испугал лабиринт древних загадок и тайн. И теперь им пришла " +
                         "пора сделать первый шаг и попробовать силы в схватке с врагом.",
                model.Description, true);

            Assert.Equal(2008, model.ReleaseYear);

            Assert.Collection(model.DownloadLinks,
                item => Assert.True(item.Type.Equals("fb2") &&
                                    item.Uri.Equals("https://www.e-reading.club/download.php?book=86632")),
                item => Assert.True(item.Type.Equals("lrf") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/lrf.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.lrf")),
                item => Assert.True(item.Type.Equals("epub") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/epub.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.epub")),
                item => Assert.True(item.Type.Equals("mobi") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/mobi.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.mobi")),
                item => Assert.True(item.Type.Equals("txt") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/txt.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.txt")),
                item => Assert.True(item.Type.Equals("html") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/bookreader.php/save/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.html"))
            );
        }

        [Fact]
        public async Task Should_Extract_Book_Gary_Garrison_Bill_Galactic_Hero()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.e-reading.club/book.php?book=13794");

            Assert.Equal("https://www.e-reading.club/cover/13/13794.jpg",
                model.ImageUri, true);

            Assert.Equal("Билл - Герой Галактики", model.Title, true);

            Assert.Equal("Гаррисон Гарри", model.Author, true);

            Assert.Equal("Билл - герой Галактики", model.Serie, true);

            Assert.Equal("https://www.e-reading.club/series.aspx/763/Bill_-_geroy_Galaktiki.html", model.TrackingUri);

            Assert.Equal(1, model.Index);

            Assert.Null(model.Genres);

            Assert.Equal("Кто знает, как бы сложилась жизнь простого парня Билла, если бы не случай, " +
                         "который сыграл с ним злую шутку и привел его в ряды имперской космической пехоты. " +
                         "Вот тут-то он и окунается с головой в мир невероятных приключений. Обстоятельства " +
                         "вынуждают командирование космического флота отправить ещё не обстрелянного, плохо " +
                         "обученного рекрута вместе с такими же зелеными новобранцами воевать с разумными " +
                         "обитателями далеких планет. Не раз и не два придется Биллу смотреть в глаза, но " +
                         "природная смекалка, изобретательность, а где-то и везение позволяют ему не только " +
                         "выжить, но и стать тем, кого весь обитаемый космос знает как Билла - героя Галактики.",
                model.Description, true);

            Assert.Equal(1965, model.ReleaseYear);

            Assert.Null(model.DownloadLinks);
        }

        [Fact]
        public async Task Should_Extract_Book_Gary_Garrison_Bill_Galactic_Hero_2()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.e-reading.club/book.php?book=13798");

            Assert.Equal("https://www.e-reading.club/cover/13/13798.jpg",
                model.ImageUri, true);

            Assert.Equal("Билл, герой Галактики, на планете роботов-рабов", model.Title, true);

            Assert.Equal("Гаррисон Гарри", model.Author, true);

            Assert.Equal("Билл - герой Галактики", model.Serie, true);

            Assert.Equal("https://www.e-reading.club/series.aspx/763/Bill_-_geroy_Galaktiki.html", model.TrackingUri);

            Assert.Equal(2, model.Index);

            Assert.Null(model.Genres);

            Assert.Equal("В недавнем прошлом простой деревенский парень Билл, пройдя через горнило " +
                         "космических битв, становится закаленным в боях межзвездным воином. Не раз и не два " +
                         "заглядывал он смерти в глаза. Но ни жестокие удары судьбы, ни страшные ранения не " +
                         "сумели сломить Билла. Его решительность и природный ум, чувство юмора и изобретательность " +
                         "качества, благодаря которым он снискал себе славу Героя Галактики,љ никогда не изменяют " +
                         "ему. И вот уже новые, захватывающие дух приключения ожидают Билла на планете роботов-рабов " +
                         "и на планете закупоренных мозгов.",
                model.Description, true);

            Assert.Equal(1989, model.ReleaseYear);

            Assert.Null(model.DownloadLinks);
        }

        [Fact]
        public async Task Should_Extract_Book_Alex_Kosh_Ogneniy_Fakultet()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.e-reading.club/book.php?book=30001");

            Assert.Equal("https://www.e-reading.club/cover/30/30001.jpg",
                model.ImageUri, true);

            Assert.Equal("Огненный факультет", model.Title, true);

            Assert.Equal("Кош Алекс", model.Author, true);

            Assert.Equal("Ремесло", model.Serie, true);

            Assert.Equal("https://www.e-reading.club/series.aspx/1607/Remeslo.html", model.TrackingUri);

            Assert.Equal(1, model.Index);

            Assert.Collection(model.Genres,
                item => Assert.Equal("фэнтези", item, true)
            );

            Assert.Equal("Академия Ремесла принимает новых учеников, и этот курс будет не таким, " +
                         "как все предыдущие. Хотя бы потому, что в Академию поступила вампирша, а еще " +
                         "каким-то чудом в нее поступил главный герой, у которого практически полностью " +
                         "отсутствуют магические способности. Впереди лекции по тактике и энергетике, " +
                         "многочасовые медитации, магические поединки, межфакультетские соревнования " +
                         "Множество проблем ляжет на плечи новичков, но они не сдадутся и не опозорят " +
                         "ОГНЕННЫЙ ФАКУЛЬТЕТ.",
                model.Description, true);

            Assert.Equal(2005, model.ReleaseYear);

            Assert.Collection(model.DownloadLinks,
                item => Assert.True(item.Type.Equals("fb2") &&
                                    item.Uri.Equals("https://www.e-reading.club/download.php?book=30001")),
                item => Assert.True(item.Type.Equals("lrf") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/lrf.php/30001/Кош_-_Огненный_факультет.lrf")),
                item => Assert.True(item.Type.Equals("epub") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/epub.php/30001/Кош_-_Огненный_факультет.epub")),
                item => Assert.True(item.Type.Equals("mobi") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/mobi.php/30001/Кош_-_Огненный_факультет.mobi")),
                item => Assert.True(item.Type.Equals("txt") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/txt.php/30001/Кош_-_Огненный_факультет.txt")),
                item => Assert.True(item.Type.Equals("html") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/bookreader.php/save/30001/Кош_-_Огненный_факультет.html"))
            );
        }

        [Fact]
        public async Task Should_Extract_Book_Alex_Kosh_Ogneniy_Patrul()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.e-reading.club/book.php?book=30000");

            Assert.Equal("https://www.e-reading.club/cover/30/30000.jpg",
                model.ImageUri, true);

            Assert.Equal("Огненный патруль", model.Title, true);

            Assert.Equal("Кош Алекс", model.Author, true);

            Assert.Equal("Ремесло", model.Serie, true);

            Assert.Equal("https://www.e-reading.club/series.aspx/1607/Remeslo.html", model.TrackingUri);

            Assert.Equal(2, model.Index);

            Assert.Collection(model.Genres,
                item => Assert.Equal("фэнтези", item, true)
            );

            string testDescription =
                "Легко ли вчерашним магам-первокурсникам переквалифицироваться в сыщиков " +
                "любителей? Запросто! Дайте им в руки по справочнику заклинаний, отправьте в " +
                "совершенно незнакомый город, поселите в дом с привидениями и посмотрите, " +
                "что из всего этого получится." +
                "В городе начались загадочные убийства? Вперед! Некромантия позволит получить " +
                "информацию об убийце от самих жертв, а поисковые заклинания с легкостью найдут " +
                "виновного, вот только справиться с ним будет не так-то просто. Водоворот странных " +
                "событий затягивает учеников Огненного Факультета все глубже и глубже: тайные " +
                "общества, вампиры, шпионы, ходячие мертвецы... Но жители города могут быть спокойны, " +
                "потому что отныне за порядком следит ОГНЕННЫЙ ПАТРУЛЬ.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription, modelDescription, true);

            Assert.Equal(2006, model.ReleaseYear);

            Assert.Collection(model.DownloadLinks,
                item => Assert.True(item.Type.Equals("fb2") &&
                                    item.Uri.Equals("https://www.e-reading.club/download.php?book=30000")),
                item => Assert.True(item.Type.Equals("lrf") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/lrf.php/30000/Кош_-_Огненный_патруль.lrf")),
                item => Assert.True(item.Type.Equals("epub") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/epub.php/30000/Кош_-_Огненный_патруль.epub")),
                item => Assert.True(item.Type.Equals("mobi") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/mobi.php/30000/Кош_-_Огненный_патруль.mobi")),
                item => Assert.True(item.Type.Equals("txt") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/txt.php/30000/Кош_-_Огненный_патруль.txt")),
                item => Assert.True(item.Type.Equals("html") &&
                                    item.Uri.Equals(
                                        "https://www.e-reading.club/bookreader.php/save/30000/Кош_-_Огненный_патруль.html"))
            );
        }

        #endregion

        #region Extract Books to Track Tests

        [Fact]
        public async Task Should_Extract_Books_to_Track_Vasiliy_Mahanenko_Mir_Barleonu()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://www.e-reading.club/series.aspx/1002172/Mir_Barliony.html");

            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Барлиона", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Путь Шамана. Гамбит Картоса", item.Title, true);
                    Assert.Equal(2, item.Index);
                },
                item =>
                {
                    Assert.Equal("Путь Шамана. Тайна Темного леса", item.Title, true);
                    Assert.Equal(3, item.Index);
                },
                item =>
                {
                    Assert.Equal("Путь Шамана. Шаг 4: Доспехи Светозарного", item.Title, true);
                    Assert.Equal(4, item.Index);
                },
                item =>
                {
                    Assert.Equal("Призрачный замок", item.Title, true);
                    Assert.Equal(5, item.Index);
                },
                item =>
                {
                    Assert.Equal("Путь Шамана. Шаг 5: Шахматы Кармадонта", item.Title, true);
                    Assert.Equal(6, item.Index);
                },
                item =>
                {
                    Assert.Equal("Путь шамана. Все только начинается", item.Title, true);
                    Assert.Equal(7, item.Index);
                },
                item =>
                {
                    Assert.Equal("Путь Шамана. Шаг 7: Поиск Создателя", item.Title, true);
                    Assert.Equal(8, item.Index);
                },
                item =>
                {
                    Assert.Equal("Мир Барлионы. Книги 1-7", item.Title, true);
                    Assert.Equal(9, item.Index);
                }
            );
        }

        [Fact]
        public async Task Should_Extract_Books_to_Track_Zukov_Vitaliy_Voyna_za_Vuzhuvanie()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://www.e-reading.club/series.aspx/3334/Voyna_za_vyzhivanie.html");

            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Конклав бессмертных. В краю далеком", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Конклав Бессмертных. Проба сил", item.Title, true);
                    Assert.Equal(2, item.Index);
                },
                item =>
                {
                    Assert.Equal("Во имя потерянных душ", item.Title, true);
                    Assert.Equal(3, item.Index);
                }
            );
        }

        [Fact]
        public async Task Should_Extract_Books_to_Track_Gary_Garrison_Bill_Galactic_Hero()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://www.e-reading.club/series.aspx/763/Bill_-_geroy_Galaktiki.html");


            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Билл - Герой Галактики", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Билл, герой Галактики, на планете роботов-рабов", item.Title, true);
                    Assert.Equal(2, item.Index);
                },
                item =>
                {
                    Assert.Equal("Билл - Герой Галактики на планете Бутылочных мозгов", item.Title, true);
                    Assert.Equal(3, item.Index);
                },
                item =>
                {
                    Assert.Equal("Билл — герой Галактики, на планете зомби-вампиров", item.Title, true);
                    Assert.Equal(4, item.Index);
                },
                item =>
                {
                    Assert.Equal("Билл, герой Галактики, на планете непознанных наслаждений", item.Title, true);
                    Assert.Equal(5, item.Index);
                },
                item =>
                {
                    Assert.Equal("Билл, герой Галактики, на планете десяти тысяч баров", item.Title, true);
                    Assert.Equal(6, item.Index);
                },
                item =>
                {
                    Assert.Equal("Билл, герой Галактики, отправляется в свой первый отпуск", item.Title, true);
                    Assert.Equal(7, item.Index);
                },
                item =>
                {
                    Assert.Equal("Билл, герой Галактики: Последнее злополучное приключение", item.Title, true);
                    Assert.Equal(8, item.Index);
                }
            );
        }

        [Fact]
        public async Task Should_Extract_Books_to_Track_Alex_Kosh_Remeslo()
        {
            var service = new EReadingExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack("https://www.e-reading.club/series.aspx/1607/Remeslo.html");

            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Огненный факультет", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Огненный патруль", item.Title, true);
                    Assert.Equal(2, item.Index);
                },
                item =>
                {
                    Assert.Equal("Огненный орден", item.Title, true);
                    Assert.Equal(3, item.Index);
                },
                item =>
                {
                    Assert.Equal("Огненный Легион", item.Title, true);
                    Assert.Equal(4, item.Index);
                }
            );
        }

        #endregion
    }
}