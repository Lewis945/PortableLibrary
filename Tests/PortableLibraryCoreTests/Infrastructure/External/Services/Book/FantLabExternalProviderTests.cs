using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.Book;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.Book
{
    public class FantLabExternalProviderTests
    {
        [Fact]
        public async Task Should_Extract_Alex_Kosh_Ogneniy_Facultet()
        {
            var service = new FantLabExternalProvider();

            var model = await service.Extract("https://fantlab.ru/work43493");

            Assert.Equal("https://data.fantlab.ru/images/editions/big/21842",
                model.ImageUri, true);

            Assert.Equal("Огненный Факультет", model.Title, true);
            Assert.Equal("Огненный Факультет", model.OriginalTitle, true);
            Assert.Null(model.OtherTitles);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Фэнтези", item, true),
                item => Assert.Equal("Героическое фэнтези", item, true),
                item => Assert.Equal("Science Fantasy", item, true)
            );

            Assert.Equal("Алекс Кош", model.Author, true);

            Assert.Equal("Ремесло", model.Series, true);

            Assert.Equal("https://fantlab.ru/work43498/", model.TrackingUri, true);

            Assert.Equal(1, model.Index);

            string testDescription =
                "Закери Никерс — парень, поступивший в Академию Ремесла на факультет магии Огня. " +
                "Вы не смотрите, что у него совсем нет способностей к магии, он полный разгильдяй, " +
                "и ему снятся... ну очень странные сны. Он еще многим здесь задаст жару! " +
                "А помогут ему в этом его верные друзья и девушка-вампирша.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription,
                modelDescription, true);

            Assert.Equal(2005, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Alex_Kosh_Ogneniy_Orden()
        {
            var service = new FantLabExternalProvider();

            var model = await service.Extract("https://fantlab.ru/work43496");

            Assert.Equal("https://data.fantlab.ru/images/editions/big/58726",
                model.ImageUri, true);

            Assert.Equal("Огненный Орден", model.Title, true);
            Assert.Equal("Огненный Орден", model.OriginalTitle, true);
            Assert.Null(model.OtherTitles);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Фэнтези", item, true),
                item => Assert.Equal("Героическое фэнтези", item, true),
                item => Assert.Equal("Science Fantasy", item, true)
            );

            Assert.Equal("Алекс Кош", model.Author, true);

            Assert.Equal("Ремесло", model.Series, true);

            Assert.Equal("https://fantlab.ru/work43498/", model.TrackingUri, true);

            Assert.Equal(3, model.Index);

            string testDescription =
                "В третьей книге из цикла «Ремесло» Зака и его друзей ожидают новые испытания: " +
                "борьба с группой низших вампиров, дуэли с старшекурсниками, путешествие в " +
                "земли упырей из клана Сеон. Сможет ли Зак и его товарищи решить все проблемы " +
                "и найти ответы на все вопросы? Справится ли Никерс со своими личными неурядицами? " +
                "Добьётся ли он взаимности от Алисы?";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription,
                modelDescription, true);

            Assert.Equal(2011, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Gary_Garrison_A_Stainless_Steel_Rat_is_Born()
        {
            var service = new FantLabExternalProvider();

            var model = await service.Extract("https://fantlab.ru/work2455");

            Assert.Equal("https://data.fantlab.ru/images/editions/big/7440",
                model.ImageUri, true);

            Assert.Equal("Рождение Стальной Крысы", model.Title, true);
            Assert.Equal("A Stainless Steel Rat is Born", model.OriginalTitle, true);

            Assert.Collection(model.OtherTitles,
                item => Assert.Equal("Крыса из нержавеющей стали появляется на свет", item, true)
            );

            Assert.Collection(model.Genres,
                item => Assert.Equal("Фантастика", item, true),
                item => Assert.Equal("Гуманитарная («мягкая») НФ", item, true)
            );

            Assert.Equal("Гарри Гаррисон", model.Author, true);

            Assert.Equal("Стальная Крыса", model.Series, true);

            Assert.Equal("https://fantlab.ru/work2454/", model.TrackingUri, true);

            Assert.Equal(1, model.Index);

            string testDescription =
                "Семнадцатилетний Джим ди Гриз, уроженец захолустной сельскохозяйственной " +
                "планеты Бит О'Хэвен, твердо решает посвятить свою жизнь преступной " +
                "деятельности. Но вот беда — ввести в криминальный мир и обучить самым " +
                "необходимым вещам его некому. Есть только одна надежда — легендарный " +
                "преступник Слон, о котором, правда, уже полтора десятка лет никто ничего " +
                "не слышал. И все же другого выбора нет, и Джим пойдет на все, " +
                "чтобы разыскать Слона и убедить его стать своим учителем.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription,
                modelDescription, true);

            Assert.Equal(1985, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Gary_Garrison_The_Stainless_Steel_Rat()
        {
            var service = new FantLabExternalProvider();

            var model = await service.Extract("https://fantlab.ru/work2458");

            Assert.Equal("https://data.fantlab.ru/images/editions/big/1306",
                model.ImageUri, true);

            Assert.Equal("Стальная Крыса", model.Title, true);
            Assert.Equal("The Stainless Steel Rat", model.OriginalTitle, true);

            Assert.Collection(model.OtherTitles,
                item => Assert.Equal("Крыса из нержавеющей стали", item, true),
                item => Assert.Equal("Приключения Джима Ди Гриза", item, true),
                item => Assert.Equal("Джим-стальная крыса", item, true)
            );

            Assert.Collection(model.Genres,
                item => Assert.Equal("Фантастика", item, true),
                item => Assert.Equal("Гуманитарная («мягкая») НФ", item, true)
            );

            Assert.Equal("Гарри Гаррисон", model.Author, true);

            Assert.Equal("Стальная Крыса", model.Series, true);

            Assert.Equal("https://fantlab.ru/work2454/", model.TrackingUri, true);

            Assert.Equal(4, model.Index);

            string testDescription =
                "Скользкий Джим ди Гриз, самый удачливый авантюрист и мошенник в Галактике, " +
                "все же попадает в ловушку, расставленную для него сотрудниками Специального " +
                "Корпуса. Но вместо суда ему предлагают службу в Корпусе, на что Джим, конечно, " +
                "соглашается. Вскоре появляется и первое серьезное задание — необходимо выяснить, " +
                "кто и с какой целью строит на мирной планете боевой космический корабль...";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription,
                modelDescription, true);

            Assert.Equal(1961, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Igor_Dravin_Chuzhak_Ohotnik()
        {
            var service = new FantLabExternalProvider();

            var model = await service.Extract("https://fantlab.ru/work229786");

            Assert.Equal("https://data.fantlab.ru/images/editions/big/52167",
                model.ImageUri, true);

            Assert.Equal("Чужак. Охотник", model.Title, true);
            Assert.Equal("Чужак. Охотник", model.OriginalTitle, true);
            Assert.Null(model.OtherTitles);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Фэнтези", item, true),
                item => Assert.Equal("Героическое фэнтези", item, true),
                item => Assert.Equal("Боевик", item, true)
            );

            Assert.Equal("Игорь Дравин", model.Author, true);

            Assert.Equal("Чужак", model.Series, true);

            Assert.Equal("https://fantlab.ru/work226082/", model.TrackingUri, true);

            Assert.Equal(3, model.Index);

            string testDescription =
                "Влад набирается навыков и опыта в погани и становится мастером охотником. " +
                "Получив некое задание от гильдии охотников, Влад решает покинуть " +
                "Белгор и отправится путешествовать по миру Арланд." +
                "Его путь поначалу лежит в Литию. Что ждет Влада в этом путешествии, " +
                "через какие приключения он должен пройти, и чем закончится его поход " +
                "предсказать невозможно.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription,
                modelDescription, true);

            Assert.Equal(2010, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Igor_Dravin_Chuzhak_Metr()
        {
            var service = new FantLabExternalProvider();

            var model = await service.Extract("https://fantlab.ru/work233376");

            Assert.Equal("https://data.fantlab.ru/images/editions/big/67871",
                model.ImageUri, true);

            Assert.Equal("Чужак. Мэтр", model.Title, true);
            Assert.Equal("Чужак. Мэтр", model.OriginalTitle, true);
            Assert.Null(model.OtherTitles);

            Assert.Collection(model.Genres,
                item => Assert.Equal("Фэнтези", item, true),
                item => Assert.Equal("Героическое фэнтези", item, true),
                item => Assert.Equal("Боевик", item, true)
            );

            Assert.Equal("Игорь Дравин", model.Author, true);

            Assert.Equal("Чужак", model.Series, true);

            Assert.Equal("https://fantlab.ru/work226082/", model.TrackingUri, true);

            Assert.Equal(7, model.Index);

            string testDescription =
                "Став графом эл Артуа Влад окунается в высокую политику Арланда. Но для того, " +
                "чтобы быть успешным в политике необходимо собирать больше информации. Для " +
                "этого Влад создает службу разведки и специальных операций. А как может быть " +
                "иначе, если он собирается бросить вызов серым, темным и эльфам. Количество " +
                "встающих перед ним задач все растет, а уровень все усложняется. Кроме того " +
                "Владу предстоит встреча с Кенарой, а также смена семейного положения. В " +
                "состоянии ли он будет осилить своих врагов, будет ли счастлив, и что ждет " +
                "его в конце пути? Главное не расслабляться и не терять над собой контроль.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            string modelDescription = Regex.Replace(model.Description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(testDescription,
                modelDescription, true);

            Assert.Equal(2011, model.ReleaseYear);
        }
    }
}