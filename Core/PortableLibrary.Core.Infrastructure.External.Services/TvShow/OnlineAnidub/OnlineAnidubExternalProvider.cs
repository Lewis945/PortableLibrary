using System;
using System.Threading.Tasks;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.OnlineAnidub
{
    /// <summary>
    /// https://online.anidub.com/
    /// </summary>
    public class OnlineAnidubExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "https://online.anidub.com/";
        public override string ServiceName => "AnidubOnline";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public OnlineAnidubExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion
        
        #region Public Methods

        public Task<OnlineAnidubTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
