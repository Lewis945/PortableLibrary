using AutoMapper;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.MyShows
{
    public abstract class MyShowsTestsBase : TvShowExternalProviderTestsBase
    {
        #region Properties

        protected readonly MyShowsExternalProvider EnglishService;
        protected readonly MyShowsExternalProvider RussianService;

        #endregion

        #region .ctor

        public MyShowsTestsBase()
        {
            IHttpService httpService = new HttpService();
            IRetryService retryService = new RetryService();
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<MyShowsMappingProfile>(); });
            IMapper mapper = new Mapper(config);

            EnglishService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.English);
            RussianService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.Russian);
        }

        #endregion
    }
}
