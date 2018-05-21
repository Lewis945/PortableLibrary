using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper;
using PortableLibrary.Core.Utilities;
using Xunit;

namespace PortableLibraryCoreTests.Infrastructure.External.Services.TvShow
{
    public class MyShowsExternalProviderTests
    {
        #region Fields

        private readonly MyShowsExternalProvider _englishService;
        private readonly MyShowsExternalProvider _russianService;

        #endregion

        #region .ctor

        public MyShowsExternalProviderTests()
        {
            IHttpService httpService = new HttpService();
            IRetryService retryService = new RetryService();
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<MyShowsMappingProfile>(); });
            IMapper mapper = new Mapper(config);

            _englishService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.English);
            _russianService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.Russian);
        }

        #endregion

        #region Extract Grimm TvShow Tests

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_English()
        {
            var model = await _englishService.ExtractTvShowByIdAsync(17186);
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Id_Russian()
        {
            var model = await _russianService.ExtractTvShowByIdAsync(17186);
            ValidateGrimm(model, Language.Russian);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Uri()
        {
            var model = await _englishService.ExtractTvShowByUriAsync("https://myshows.me/view/17186/");
            ValidateGrimm(model, Language.English);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_English_Name()
        {
            const string fullTitle = "Grimm";
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        [Fact]
        public async Task Should_Extract_Grimm_By_Russian_Name()
        {
            const string fullTitle = "Гримм";
            var models = await _englishService.GetTvShowsByTitleAsync(fullTitle);
            var model = models.FirstOrDefault(m => m.Title == fullTitle);

            Assert.NotNull(model);
            Assert.Equal(17186, model.Id);
        }

        private void ValidateGrimm(MyShowsTvShowModel model, Language language)
        {
            #region Tv Show

            string GetTitle()
            {
                switch (language)
                {
                    case Language.English:
                        return "Grimm";
                    case Language.Russian:
                        return "Гримм";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(language), language, null);
                }
            }

            Assert.Equal(GetTitle(), model.Title, true);
            Assert.Equal("Grimm", model.TitleOriginal, true);

            Assert.NotNull(model.Seasons);
            Assert.Equal(6, model.TotalSeasons);
            Assert.Equal(6, model.Seasons.Count);

            #endregion
        }

        #endregion
    }
}