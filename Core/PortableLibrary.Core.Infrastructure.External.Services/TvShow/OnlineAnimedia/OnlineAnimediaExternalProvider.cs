using System;
using System.Threading.Tasks;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.OnlineAnimedia
{
    /// <summary>
    /// http://online.animedia.tv
    /// </summary>
    public class OnlineAnimediaExternalProvider : BaseExternalProvider
    {
        #region Properties
        
        public override string ServiceUri => "http://online.animedia.tv";
        public override string ServiceName => "AnimediaOnline";
        
        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public OnlineAnimediaExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion
        
        #region Public Methods

        public Task<OnlineAnimediaTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
