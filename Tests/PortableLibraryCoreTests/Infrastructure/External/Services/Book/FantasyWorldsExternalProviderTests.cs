using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.Book;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.Book
{
    public class FantasyWorldsExternalProviderTests
    {
        [Fact]
        public async Task Should_Extract_Gary_Garrison_The_Stainless_Steel_Rat_Goes_to_Hell()
        {
            var service = new FantasyWorldsExternalProvider();

            var model = await service.Extract("https://fantasy-worlds.org/lib/id3422/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/34/3422.jpg",
                model.ImageUri, true);

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
        public async Task Should_Extract_Alex_Kosh_Ogneniy_Orden()
        {
            var service = new FantasyWorldsExternalProvider();

            var model = await service.Extract("https://fantasy-worlds.org/lib/id14932/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/149/14932.jpg",
                model.ImageUri, true);

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
        public async Task Should_Extract_Alex_Kosh_Soyuz_Proklyatuh()
        {
            var service = new FantasyWorldsExternalProvider();

            var model = await service.Extract("https://fantasy-worlds.org/lib/id25365/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/253/25365.jpg",
                model.ImageUri, true);

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
        public async Task Should_Extract_John_R_R_Tolkien_The_Fellowship_of_the_Ring()
        {
            var service = new FantasyWorldsExternalProvider();

            var model = await service.Extract("https://fantasy-worlds.org/lib/id25800/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/258/25800.jpg",
                model.ImageUri, true);

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
        public async Task Should_Extract_Ray_Bradbury_Fahrenheit_451()
        {
            var service = new FantasyWorldsExternalProvider();

            var model = await service.Extract("https://fantasy-worlds.org/lib/id13732/");

            Assert.Equal("https://fantasy-worlds.org/img/preview/137/13732.jpg",
                model.ImageUri, true);

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
    }
}