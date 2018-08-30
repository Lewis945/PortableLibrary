using AutoMapper;
using Newtonsoft.Json;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Models;
using PortableLibrary.Core.Utilities;
using System.IO;

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

            #region Configuration

            var fileContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "..",
                "Configuration", "Secret", "MyShowsConfiguration.json"));
            var authConfig = JsonConvert.DeserializeObject<AuthTokenModel>(fileContent);

            #endregion

            EnglishService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.English, authConfig);
            RussianService = new MyShowsExternalProvider(httpService, retryService, mapper, Language.Russian, authConfig);
        }

        #endregion
    }
}
