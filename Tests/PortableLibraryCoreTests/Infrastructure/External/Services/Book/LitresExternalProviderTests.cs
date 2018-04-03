using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Services.Book;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.Book
{
    public class LitresExternalProviderTests
    {
        [Fact]
        public async Task Should_Extract_Alex_Kosh_Soyuz_Proklyatuh()
        {
            var service = new LitresExternalProvider();

            var model = await service.Extract("https://www.litres.ru/aleks-kosh/souz-proklyatyh/");

            Assert.Equal("https://cv4.litres.ru/sbc/25539349_cover-elektronnaya-kniga-aleks-kosh-souz-proklyatyh.jpg",
                model.ImageUri);
            Assert.Equal(216724, model.ImageByteArray.Length);

            Assert.Equal("Союз проклятых", model.Title);

            Assert.Equal("Алекс Кош", model.Author);

            Assert.Equal("Одиночка", model.AuthorSeries);

            Assert.Equal(2, model.Index);

            Assert.Collection(model.PublishersSeries, item => Assert.Equal("Время SUPERгероев", item));

            Assert.Collection(model.Genres,
                item => Assert.Equal("боевая фантастика", item),
                item => Assert.Equal("героическая фантастика", item),
                item => Assert.Equal("стимпанк", item)
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
                model.Description);
//
//            Assert.Equal(340, model.PagesCount);
//
//            Assert.Equal(new DateTime(2016, 1, 1), model.DatePublished);
        }
    }
}