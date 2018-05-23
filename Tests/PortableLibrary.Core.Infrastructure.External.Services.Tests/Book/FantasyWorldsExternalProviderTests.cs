using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.Book.FantasyWorlds;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.Book
{
    public class FantasyWorldsExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public FantasyWorldsExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        #region Extract Book Tests

        [Fact]
        public async Task Should_Extract_Book_Gary_Garrison_The_Stainless_Steel_Rat_Goes_to_Hell()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.Extract("https://fantasy-worlds.org/lib/id3422/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/34/3422.jpg",
                model.ImageUri, true);

            Assert.Equal("https://fantasy-worlds.org/series/id655/", model.TrackingUri, true);

            Assert.Equal("Стальная Крыса отправляется в ад", model.Title, true);
            Assert.Equal("The Stainless Steel Rat Goes to Hell", model.OriginalTitle, true);

            Assert.Collection(model.OtherTitles,
                item => Assert.Equal("Стальная крыса в гостях у дьявола", item, true)
            );

            Assert.Equal("Гарри Гаррисон", model.Author, true);

            Assert.Collection(model.Series,
                item => Assert.Equal("Крыса из нержавеющей стали", item, true)
            );

            Assert.Equal(9, model.Index);

            Assert.Equal("Великолепный Джим ди Гриз давным-давно выбрал свой путь. И пусть кому-то такая жизнь может " +
                         "показаться странной и даже преступной, тем не менее, верная служба в Специальном Корпусе не " +
                         "помешала Скользкому Джиму наполнить свой банковский сейф, опустошив множество чужих. " +
                         "С таким жизненным и денежным багажом не грех бы отправиться в какой-нибудь райский уголок, " +
                         "чтобы провести там остаток дней. Но, черт возьми! Обстоятельства вынуждают бесстрашного " +
                         "ди Гриза броситься прямо в пекло ада на поиски своей любимой Анжелины.",
                model.Description, true);

            Assert.Equal(1996, model.ReleaseYear);

            Assert.Null(model.DownloadLinks);
        }

        [Fact]
        public async Task Should_Extract_Book_Alex_Kosh_Ogneniy_Orden()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.Extract("https://fantasy-worlds.org/lib/id14932/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/149/14932.jpg",
                model.ImageUri, true);

            Assert.Equal("https://fantasy-worlds.org/series/id872/", model.TrackingUri, true);

            Assert.Equal("Огненный орден", model.Title, true);
            Assert.Equal("Огненный орден", model.OriginalTitle, true);

            Assert.Null(model.OtherTitles);

            Assert.Equal("Алекс Кош", model.Author, true);

            Assert.Collection(model.Series,
                item => Assert.Equal("Ремесло", item, true)
            );

            Assert.Equal(3, model.Index);

            string testDescription =
                "Справиться с бандой низших вампиров, промышляющих разбоем прямо под носом местной " +
                "стражи? Для магов-первокурсников это не может быть серьезной проблемой. " +
                "Вот только как быть с тем, что один из них потерял способности к магии, " +
                "а другой вот-вот сам превратится в вампира? Да и справиться с бандой гораздо " +
                "сложнее, чем может показаться на первый взгляд. Закери Никерс не привык жаловаться " +
                "на судьбу, но в этот раз она явно перестаралась: разборки с тайными обществами, прогулки " +
                "по Коридору Судьбы и иным мирам, магические дуэли со старшекурсниками, путешествие в земли " +
                "вампиров и знакомство с женой самого кровавого вампира тысячелетия! " +
                "Ну что ж, практика магов-первокурсников продолжается!";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription,
                modelDescription, true);

            Assert.Equal(2011, model.ReleaseYear);

            Assert.Collection(model.DownloadLinks,
                item => Assert.True(item.Type.Equals("fb2") &&
                                    item.Uri.Equals("https://fantasy-worlds.org/lib/id14932/download/")),
                item => Assert.True(item.Type.Equals("epub") &&
                                    item.Uri.Equals("https://fantasy-worlds.org/lib/id14932/download/epub/")),
                item => Assert.True(item.Type.Equals("mobi") &&
                                    item.Uri.Equals("https://fantasy-worlds.org/lib/id14932/download/mobi/")),
                item => Assert.True(item.Type.Equals("txt") &&
                                    item.Uri.Equals("https://fantasy-worlds.org/lib/id14932/download/txt/"))
            );
        }

        [Fact]
        public async Task Should_Extract_Book_Alex_Kosh_Soyuz_Proklyatuh()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.Extract("https://fantasy-worlds.org/lib/id25365/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/253/25365.jpg",
                model.ImageUri, true);

            Assert.Equal("https://fantasy-worlds.org/series/id4111/", model.TrackingUri, true);

            Assert.Equal("Союз проклятых", model.Title, true);
            Assert.Equal("Союз проклятых", model.OriginalTitle, true);

            Assert.Null(model.OtherTitles);

            Assert.Equal("Алекс Кош", model.Author, true);

            Assert.Collection(model.Series,
                item => Assert.Equal("WarGames", item, true),
                item => Assert.Equal("Одиночка", item, true)
            );

            Assert.Equal(2, model.Index);

            string testDescription = "После того как клан «Стальных Крыс» обманом завладел Костяным Мечом и продал " +
                                     "его другому клану, Фальк вновь возвращается в Арктанию, чтобы попытаться вернуть " +
                                     "квестовый предмет. Теперь на кону не только выполнение задания виртуальной богини," +
                                     " но и вполне реальная человеческая жизнь. Но все не так просто – в игре его на " +
                                     "каждом шагу терроризируют «крысы», пытаясь заставить выложить всю информацию о " +
                                     "местонахождении остальных эпических артефактов, представляющих не только игровую, " +
                                     "но и немалую финансовую ценность. Да и новый владелец Костяного Меча едва ли " +
                                     "захочет расстаться со столь ценным приобретением, и Фальку решительно нечего " +
                                     "предложить ему в обмен. Но выход найдется всегда, пусть ради этого и придется " +
                                     "нарушить закон…";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription, modelDescription, true);

            Assert.Equal(2016, model.ReleaseYear);

            Assert.Null(model.DownloadLinks);
        }

        [Fact]
        public async Task Should_Extract_Book_John_R_R_Tolkien_The_Fellowship_of_the_Ring()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.Extract("https://fantasy-worlds.org/lib/id25800/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/258/25800.jpg",
                model.ImageUri, true);

            Assert.Equal("https://fantasy-worlds.org/series/id1740/", model.TrackingUri, true);

            Assert.Equal("Хранители Кольца", model.Title, true);
            Assert.Equal("The Fellowship of the Ring", model.OriginalTitle, true);

            Assert.Collection(model.OtherTitles,
                item => Assert.Equal("Братство Кольца", item, true),
                item => Assert.Equal("Хранители", item, true),
                item => Assert.Equal("Дружество кольца", item, true),
                item => Assert.Equal("Содружество кольца", item, true),
                item => Assert.Equal("Товарищество кольца", item, true)
            );

            Assert.Equal("Дж. Р. Р. Толкин", model.Author, true);

            Assert.Collection(model.Series,
                item => Assert.Equal("Легендариум Средиземья", item, true),
                item => Assert.Equal("Властелин колец", item, true)
            );

            Assert.Equal(1, model.Index);

            string testDescription =
                "Трилогия «Властелин Колец» бесспорно возглавляет список «культовых» книг ХХ века." +
                " Ее автор, Дж. Р.Р. Толкин, профессор Оксфордского университета, специалист по " +
                "древнему и средневековому английскому языку, создал удивительный мир – Средиземье," +
                " который вот уже без малого пятьдесят лет неодолимо влечет к себе миллионы " +
                "читателей. Великолепная кинотрилогия, снятая Питером Джексоном, в десятки раз " +
                "увеличила ряды поклонников как Толкина, так и самого жанра героического фэнтези.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription, modelDescription, true);

            Assert.Equal(1954, model.ReleaseYear);

            Assert.Null(model.DownloadLinks);
        }

        [Fact]
        public async Task Should_Extract_Book_Ray_Bradbury_Fahrenheit_451()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.Extract("https://fantasy-worlds.org/lib/id13732/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/137/13732.jpg",
                model.ImageUri, true);

            Assert.Null(model.TrackingUri);

            Assert.Equal("451° по Фаренгейту", model.Title, true);
            Assert.Equal("Fahrenheit 451", model.OriginalTitle, true);

            Assert.Collection(model.OtherTitles,
                item => Assert.Equal("451 градус по Фаренгейту", item, true)
            );

            Assert.Equal("Рэй Брэдбери", model.Author, true);

            Assert.Null(model.Series);

            Assert.Null(model.Index);

            string testDescription = "«451° по Фаренгейту» — роман, принесший писателю мировую известность. 451° " +
                                     "по Фаренгейту — температура, при которой воспламеняется и горит бумага. " +
                                     "Философская антиутопия Рэя Брэдбери рисует беспросветную картину развития " +
                                     "постиндустриального общества; это мир будущего, в котором все письменные " +
                                     "издания безжалостно уничтожаются специальным отрядом пожарных, а хранение " +
                                     "книг преследуется по закону, интерактивное телевидение успешно служит всеобщему " +
                                     "оболваниванию, карательная психиатрия решительно разбирается с редкими " +
                                     "инакомыслящими, а на охоту за неисправимыми диссидентами выходит электрический пес…";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription, modelDescription, true);

            Assert.Equal(1953, model.ReleaseYear);

            Assert.Null(model.DownloadLinks);
        }

        #endregion

        #region Extract Books to Track Tests

        [Fact]
        public async Task Should_Extract_Books_to_Track_Gary_Garrison_Krusa_iz_Nerzhveyushey_Stali()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://fantasy-worlds.org/series/id655/");

            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Рождение Стальной Крысы", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Стальная Крыса идет в армию", item.Title, true);
                    Assert.Equal(2, item.Index);
                },
                item =>
                {
                    Assert.Equal("Крыса из нержавеющей стали", item.Title, true);
                    Assert.Equal(3, item.Index);
                },
                item =>
                {
                    Assert.Equal("Месть крысы из нержавеющей стали", item.Title, true);
                    Assert.Equal(4, item.Index);
                },
                item =>
                {
                    Assert.Equal("Крыса из нержавеющей стали спасает мир", item.Title, true);
                    Assert.Equal(5, item.Index);
                },
                item =>
                {
                    Assert.Equal("Ты нужен Стальной Крысе", item.Title, true);
                    Assert.Equal(6, item.Index);
                },
                item =>
                {
                    Assert.Equal("Стальную Крысу – в президенты!", item.Title, true);
                    Assert.Equal(7, item.Index);
                },
                item =>
                {
                    Assert.Equal("Стальная Крыса поет блюз", item.Title, true);
                    Assert.Equal(8, item.Index);
                },
                item =>
                {
                    Assert.Equal("Золотые годы Стальной Крысы", item.Title, true);
                    Assert.Equal(9, item.Index);
                },
                item =>
                {
                    Assert.Equal("Стальная Крыса отправляется в ад", item.Title, true);
                    Assert.Equal(9, item.Index);
                },
                item =>
                {
                    Assert.Equal("Стальная Крыса на манеже", item.Title, true);
                    Assert.Equal(10, item.Index);
                },
                item =>
                {
                    Assert.Equal("Новые приключения Стальной Крысы", item.Title, true);
                    Assert.Equal(12, item.Index);
                }
            );
        }

        [Fact]
        public async Task Should_Extract_Books_to_Track_Alex_Kosh_Remeslo()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://fantasy-worlds.org/series/id872/");

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

        [Fact]
        public async Task Should_Extract_Books_to_Track_Alex_Kosh_Odinochka()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://fantasy-worlds.org/series/id4111/");

            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Дорога мечей", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Союз проклятых", item.Title, true);
                    Assert.Equal(2, item.Index);
                }
            );
        }

        [Fact]
        public async Task Should_Extract_Books_to_Track_John_R_R_Tolkien_The_Fellowship_of_the_Ring()
        {
            var service = new FantasyWorldsExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://fantasy-worlds.org/series/id1740/");

            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Властелин колец", item.Title, true);
                    Assert.Null(item.Index);
                },
                item =>
                {
                    Assert.Equal("Хранители Кольца", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Две твердыни", item.Title, true);
                    Assert.Equal(2, item.Index);
                },
                item =>
                {
                    Assert.Equal("Возвращение короля", item.Title, true);
                    Assert.Equal(3, item.Index);
                }
            );
        }

        #endregion
    }
}