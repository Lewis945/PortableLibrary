﻿using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.Book.Litres;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.Book
{
    public class LitresExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public LitresExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        #region Extract Book Tests

        [Fact]
        public async Task Should_Extract_Book_Alex_Kosh_Soyuz_Proklyatuh()
        {
            var service = new LitresExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.litres.ru/aleks-kosh/souz-proklyatyh/");

            Assert.Equal("https://cv4.litres.ru/sbc/25539349_cover-elektronnaya-kniga-aleks-kosh-souz-proklyatyh.jpg",
                model.ImageUri, true);

            Assert.Equal("https://www.litres.ru/serii-knig/odinochka-441373/", model.TrackingUri, true);

            Assert.Equal("Союз проклятых", model.Title, true);

            Assert.Equal("Алекс Кош", model.Author, true);

            Assert.Equal("Одиночка", model.AuthorSeries, true);

            Assert.Equal(2, model.Index);

            Assert.Collection(model.PublishersSeries, item => Assert.Equal("Время SUPERгероев", item, true));

            Assert.Collection(model.Genres,
                item => Assert.Equal("боевая фантастика", item, true),
                item => Assert.Equal("героическая фантастика", item, true),
                item => Assert.Equal("стимпанк", item, true)
            );

            Assert.Equal("После того как клан «Стальных Крыс» обманом завладел Костяным Мечом и продал его другому " +
                         "клану, Фальк вновь возвращается в Арктанию, чтобы попытаться вернуть квестовый предмет. " +
                         "Теперь на кону не только выполнение задания виртуальной богини, но и вполне реальная " +
                         "человеческая жизнь. Но все не так просто – в игре его на каждом шагу терроризируют " +
                         "«крысы», пытаясь заставить выложить всю информацию о местонахождении остальных эпических " +
                         "артефактов, представляющих не только игровую, но и немалую финансовую ценность. " +
                         "Да и новый владелец Костяного Меча едва ли захочет расстаться со столь ценным " +
                         "приобретением, и Фальку решительно нечего предложить ему в обмен. Но выход найдется " +
                         "всегда, пусть ради этого и придется нарушить закон…",
                model.Description, true);

            Assert.Equal(340, model.PagesCount);

            Assert.Equal(2016, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Book_Alex_Kosh_Igry_Masok()
        {
            var service = new LitresExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.litres.ru/aleks-kosh/igry-masok/");

            Assert.Equal("https://cv9.litres.ru/sbc/00827592_cover-elektronnaya-kniga-aleks-kosh-igry-masok.jpg",
                model.ImageUri, true);

            Assert.Null(model.TrackingUri);

            Assert.Equal("Игры Масок", model.Title, true);

            Assert.Equal("Алекс Кош", model.Author, true);

            Assert.Null(model.AuthorSeries);

            Assert.Null(model.Index);

            Assert.Null(model.PublishersSeries);

            Assert.Collection(model.Genres,
                item => Assert.Equal("боевая фантастика", item, true)
            );

            Assert.Equal(
                "Экстрим и стремление к самосовершенствованию у них в крови. Превосходя физическими способностями " +
                "обычных людей, идя по тонкой грани между жизнью и смертью, они получают нечто гораздо большее, " +
                "чем просто дозу адреналина. Лучшие из лучших заслуживают право прикоснуться к тайнам древних боевых " +
                "искусств и познать мир неизведанных возможностей человеческого организма. Чудовищная сила и " +
                "невероятная ловкость, управление внутренней энергией и повелевание древними духами… Кто-то называет " +
                "это магией, кто-то мистикой, на самом же деле все это результат долгих и изнурительных тренировок.",
                model.Description, true);

            Assert.Equal(520, model.PagesCount);

            Assert.Equal(2009, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Book_Gary_Garrison_Rozhdenie_Stalnoy_Krusu()
        {
            var service = new LitresExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.litres.ru/garri-garrison/rozhdenie-stalnoy-krysy/");

            Assert.Equal(
                "https://cv8.litres.ru/sbc/07036886_cover-elektronnaya-kniga-garri-garrison-rozhdenie-stalnoy-krysy.jpg",
                model.ImageUri, true);

            Assert.Equal("https://www.litres.ru/serii-knig/stalnaya-krysa/", model.TrackingUri, true);

            Assert.Equal("Рождение Стальной Крысы", model.Title, true);

            Assert.Equal("Гарри Гаррисон", model.Author, true);

            Assert.Equal("Стальная Крыса", model.AuthorSeries, true);

            Assert.Equal(1, model.Index);

            Assert.Null(model.PublishersSeries);

            Assert.Collection(model.Genres,
                item => Assert.Equal("героическая фантастика", item, true),
                item => Assert.Equal("зарубежная фантастика", item, true),
                item => Assert.Equal("юмористическая фантастика", item, true)
            );

            Assert.Equal(
                "Великолепный Джим ди Гриз – знаменитый межзвездный преступник – получил за свою изобретательность и решительность меткое прозвище Крыса из нержавеющей стали." +
                "Рожденный богатой творческой фантазией Гарри Гаррисона, отчаянный и симпатичный герой из далекого будущего приобрел необыкновенную любовь и популярность поклонников фантастики во всем мире, щедро поделившись славой со своим создателем.",
                model.Description, true);

            Assert.Equal(230, model.PagesCount);

            Assert.Equal(1985, model.ReleaseYear);
        }

        [Fact]
        public async Task Should_Extract_Book_Dem_Mihaylov_Grom_Nebesnuy()
        {
            var service = new LitresExternalProvider(_retryService);

            var model = await service.ExtractBook("https://www.litres.ru/dem-mihaylov/grom-nebesnyy/");

            Assert.Equal("https://cv3.litres.ru/sbc/12100038_cover-elektronnaya-kniga-dem-mihaylov-grom-nebesnyy.jpg",
                model.ImageUri, true);

            Assert.Equal("https://www.litres.ru/serii-knig/gospodstvo-klana-nespyaschih/", model.TrackingUri, true);

            Assert.Equal("Гром небесный", model.Title, true);

            Assert.Equal("Дем Михайлов", model.Author, true);

            Assert.Equal("Господство клана Неспящих", model.AuthorSeries, true);

            Assert.Equal(4, model.Index);

            Assert.Collection(model.PublishersSeries, item => Assert.Equal("LitRPG", item, true));

            Assert.Collection(model.Genres,
                item => Assert.Equal("боевое фэнтези", item, true),
                item => Assert.Equal("героическое фэнтези", item, true)
            );

            Assert.Equal("Онлайн-игра «Вальдира». Огромный загадочный мир с готовностью принимает в свои объятия " +
                         "каждого, гарантируя нескончаемые приключения, эпические битвы и сказочные сокровища. " +
                         "Бесчисленное количество кланов ожесточенно сражаются за земли, участвуют в войнах, плетут " +
                         "интриги и ведут шпионские игры. И где-то там, на бесконечных просторах Вальдиры, " +
                         "продолжается приключение Росгарда, волей судьбы и благодаря собственному упрямству " +
                         "ставшему Великим Навигатором, что в будущем возглавит корабельную армаду и направит ее " +
                         "прямо к древнему затерянному материку…",
                model.Description, true);

            Assert.Equal(310, model.PagesCount);

            Assert.Equal(2015, model.ReleaseYear);
        }

        #endregion

        #region Extract Books to Track Tests

        [Fact]
        public async Task Should_Extract_Books_to_Track_Alex_Kosh_Odinochka()
        {
            var service = new LitresExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://www.litres.ru/serii-knig/odinochka-441373/");

            Assert.NotNull(model);
            
            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Одиночка. Дорога мечей", item.Title, true);
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
        public async Task Should_Extract_Books_to_Track_Gary_Garrison_Stalnay_Krusa()
        {
            var service = new LitresExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://www.litres.ru/serii-knig/stalnaya-krysa/");

            Assert.NotNull(model);
            
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
                    Assert.Equal("Стальная Крыса", item.Title, true);
                    Assert.Equal(3, item.Index);
                },
                item =>
                {
                    Assert.Equal("Месть Стальной Крысы", item.Title, true);
                    Assert.Equal(4, item.Index);
                },
                item =>
                {
                    Assert.Equal("Стальная Крыса спасает мир", item.Title, true);
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
                    Assert.Equal(10, item.Index);
                },
                item =>
                {
                    Assert.Equal("Стальная Крыса на манеже", item.Title, true);
                    Assert.Equal(11, item.Index);
                }
            );
        }

        [Fact]
        public async Task Should_Extract_Books_to_Track_Dem_Mihaylov_Gospodstvo_Klana_Nespyashih()
        {
            var service = new LitresExternalProvider(_retryService);

            var model = await service.ExtractBooksToTrack(
                "https://www.litres.ru/serii-knig/gospodstvo-klana-nespyaschih/");

            Assert.NotNull(model);
            
            Assert.Collection(model,
                item =>
                {
                    Assert.Equal("Господство кланов", item.Title, true);
                    Assert.Equal(1, item.Index);
                },
                item =>
                {
                    Assert.Equal("Судьба клана", item.Title, true);
                    Assert.Equal(2, item.Index);
                },
                item =>
                {
                    Assert.Equal("Ярость богов", item.Title, true);
                    Assert.Equal(3, item.Index);
                },
                item =>
                {
                    Assert.Equal("Гром небесный", item.Title, true);
                    Assert.Equal(4, item.Index);
                },
                item =>
                {
                    Assert.Equal("Ультиматум", item.Title, true);
                    Assert.Equal(5, item.Index);
                },
                item =>
                {
                    Assert.Equal("Запределье", item.Title, true);
                    Assert.Equal(6, item.Index);
                },
                item =>
                {
                    Assert.Equal("Ярость Гуорры", item.Title, true);
                    Assert.Equal(7, item.Index);
                },
                item =>
                {
                    Assert.Equal("Великий Поход", item.Title, true);
                    Assert.Equal(8, item.Index);
                }
            );
        }
        
        #endregion
    }
}