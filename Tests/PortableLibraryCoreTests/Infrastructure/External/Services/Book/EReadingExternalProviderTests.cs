﻿using PortableLibrary.Core.Infrastructure.External.Services.Book;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.Book
{
    public class EReadingExternalProviderTests
    {
        #region Tests

        [Fact]
        public async Task Should_Extract_Vasiliy_Mahanenko_Barleona()
        {
            var service = new EReadingExternalProvider();

            var model = await service.Extract("https://www.e-reading.club/book.php?book=1016974");

            Assert.Equal("https://www.e-reading.club/cover/1016/1016974.png",
                model.ImageUri, true);

            Assert.Equal("Барлиона", model.Title, true);

            Assert.Equal("Маханенко Василий", model.Author, true);

            Assert.Equal("Мир Барлионы", model.Serie, true);

            //Assert.Equal(2, model.Index);

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

            Assert.Null(model.Year);

            Assert.Null(model.DownloadLinks);
            Assert.Null(model.DownloadBooks);
        }

        [Fact]
        public async Task Should_Extract_Zukov_Vitaliy_Konklav_Besmertnih_Proba_Sil()
        {
            var service = new EReadingExternalProvider();

            var model = await service.Extract("https://www.e-reading.club/book.php?book=86632");

            Assert.Equal("https://www.e-reading.club/cover/86/86632.jpg",
                model.ImageUri, true);

            Assert.Equal("Конклав Бессмертных. Проба сил", model.Title, true);

            Assert.Equal("Зыков Виталий", model.Author, true);

            Assert.Equal("Война за выживание", model.Serie, true);

            //Assert.Equal(2, model.Index);

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

            Assert.Equal(2008, model.Year);

            Assert.Collection(model.DownloadLinks,
               item => Assert.True(item.Type.Equals("fb2") &&
                                   item.Uri.Equals("https://www.e-reading.club/download.php?book=86632")),
               item => Assert.True(item.Type.Equals("lrf") &&
                                   item.Uri.Equals("https://www.e-reading.club/lrf.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.lrf")),
               item => Assert.True(item.Type.Equals("epub") &&
                                   item.Uri.Equals("https://www.e-reading.club/epub.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.epub")),
               item => Assert.True(item.Type.Equals("mobi") &&
                                   item.Uri.Equals("https://www.e-reading.club/mobi.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.mobi")),
                item => Assert.True(item.Type.Equals("txt") &&
                                   item.Uri.Equals("https://www.e-reading.club/txt.php/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.txt")),
                 item => Assert.True(item.Type.Equals("html") &&
                                   item.Uri.Equals("https://www.e-reading.club/bookreader.php/save/86632/Зыков_-_Конклав_Бессмертных._Проба_сил.html"))
           );
        }

        [Fact]
        public async Task Should_Extract_Gary_Garrison_Bill_Galactic_Hero()
        {
            var service = new EReadingExternalProvider();

            var model = await service.Extract("https://www.e-reading.club/book.php?book=13794");

            Assert.Equal("https://www.e-reading.club/cover/13/13794.jpg",
                model.ImageUri, true);

            Assert.Equal("Билл - Герой Галактики", model.Title, true);

            Assert.Equal("Гаррисон Гарри", model.Author, true);

            Assert.Equal("Билл - герой Галактики", model.Serie, true);

            //Assert.Equal(2, model.Index);

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

            Assert.Equal(1965, model.Year);

            Assert.Null(model.DownloadLinks);
            Assert.Null(model.DownloadBooks);
        }

        [Fact]
        public async Task Should_Extract_Gary_Garrison_Bill_Galactic_Hero_2()
        {
            var service = new EReadingExternalProvider();

            var model = await service.Extract("https://www.e-reading.club/book.php?book=13798");

            Assert.Equal("https://www.e-reading.club/cover/13/13798.jpg",
                model.ImageUri, true);

            Assert.Equal("Билл, герой Галактики, на планете роботов-рабов", model.Title, true);

            Assert.Equal("Гаррисон Гарри", model.Author, true);

            Assert.Equal("Билл - герой Галактики", model.Serie, true);

            //Assert.Equal(2, model.Index);

            Assert.Null(model.Genres);

            Assert.Equal("В недавнем прошлом простой деревенский парень Билл, пройдя через горнило " +
                "космических битв, становится закаленным в боях межзвездным воином. Не раз и не два " +
                "заглядывал он смерти в глаза. Но ни жестокие удары судьбы, ни страшные ранения не " +
                "сумели сломить Билла. Его решительность и природный ум, чувство юмора и изобретательность " +
                "качества, благодаря которым он снискал себе славу Героя Галактики,љ никогда не изменяют " +
                "ему. И вот уже новые, захватывающие дух приключения ожидают Билла на планете роботов-рабов " +
                "и на планете закупоренных мозгов.",
                model.Description, true);

            Assert.Equal(1989, model.Year);

            Assert.Null(model.DownloadLinks);
            Assert.Null(model.DownloadBooks);
        }

        [Fact]
        public async Task Should_Extract_Alex_Kosh_Ogneniy_Fakultet()
        {
            var service = new EReadingExternalProvider();

            var model = await service.Extract("https://www.e-reading.club/book.php?book=30001");

            Assert.Equal("https://www.e-reading.club/cover/30/30001.jpg",
                model.ImageUri, true);

            Assert.Equal("Огненный факультет", model.Title, true);

            Assert.Equal("Кош Алекс", model.Author, true);

            Assert.Equal("Ремесло", model.Serie, true);

            //Assert.Equal(2, model.Index);

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

            Assert.Equal(2005, model.Year);

            Assert.Collection(model.DownloadLinks,
               item => Assert.True(item.Type.Equals("fb2") &&
                                   item.Uri.Equals("https://www.e-reading.club/download.php?book=30001")),
               item => Assert.True(item.Type.Equals("lrf") &&
                                   item.Uri.Equals("https://www.e-reading.club/lrf.php/30001/Кош_-_Огненный_факультет.lrf")),
               item => Assert.True(item.Type.Equals("epub") &&
                                   item.Uri.Equals("https://www.e-reading.club/epub.php/30001/Кош_-_Огненный_факультет.epub")),
               item => Assert.True(item.Type.Equals("mobi") &&
                                   item.Uri.Equals("https://www.e-reading.club/mobi.php/30001/Кош_-_Огненный_факультет.mobi")),
                item => Assert.True(item.Type.Equals("txt") &&
                                   item.Uri.Equals("https://www.e-reading.club/txt.php/30001/Кош_-_Огненный_факультет.txt")),
                 item => Assert.True(item.Type.Equals("html") &&
                                   item.Uri.Equals("https://www.e-reading.club/bookreader.php/save/30001/Кош_-_Огненный_факультет.html"))
           );
        }

        [Fact]
        public async Task Should_Extract_Alex_Kosh_Ogneniy_Patrul()
        {
            var service = new EReadingExternalProvider();

            var model = await service.Extract("https://www.e-reading.club/book.php?book=30000");

            Assert.Equal("https://www.e-reading.club/cover/30/30000.jpg",
                model.ImageUri, true);

            Assert.Equal("Огненный патруль", model.Title, true);

            Assert.Equal("Кош Алекс", model.Author, true);

            Assert.Equal("Ремесло", model.Serie, true);

            //Assert.Equal(2, model.Index);

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

            Assert.Equal(2006, model.Year);

            Assert.Collection(model.DownloadLinks,
               item => Assert.True(item.Type.Equals("fb2") &&
                                   item.Uri.Equals("https://www.e-reading.club/download.php?book=30000")),
               item => Assert.True(item.Type.Equals("lrf") &&
                                   item.Uri.Equals("https://www.e-reading.club/lrf.php/30000/Кош_-_Огненный_патруль.lrf")),
               item => Assert.True(item.Type.Equals("epub") &&
                                   item.Uri.Equals("https://www.e-reading.club/epub.php/30000/Кош_-_Огненный_патруль.epub")),
               item => Assert.True(item.Type.Equals("mobi") &&
                                   item.Uri.Equals("https://www.e-reading.club/mobi.php/30000/Кош_-_Огненный_патруль.mobi")),
                item => Assert.True(item.Type.Equals("txt") &&
                                   item.Uri.Equals("https://www.e-reading.club/txt.php/30000/Кош_-_Огненный_патруль.txt")),
                 item => Assert.True(item.Type.Equals("html") &&
                                   item.Uri.Equals("https://www.e-reading.club/bookreader.php/save/30000/Кош_-_Огненный_патруль.html"))
           );
        }

        #endregion
    }
}
