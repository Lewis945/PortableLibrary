using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow
{
    public class OnlineAnidubExternalProviderTests
    {
        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public OnlineAnidubExternalProviderTests()
        {
            _retryService = new RetryService();
        }

        #endregion

        //https://online.anidub.com/anime/full/7710-vrata-shtaynera-steinsgate-01-25-iz-25bdrip720p2011.html
        //https://tr.anidub.com/anime_tv/full/7710-vrata-shtaynera-steinsgate-01-25-iz-25bdrip720p2011.html
        #region Extract Anime Shows
        

        #endregion
    }
}