using PortableLibrary.Core.Infrastructure.External.Services.TvShow.Baibako;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.Baibako
{
    public abstract class BaibakoTestsBase : TvShowExternalProviderTestsBase
    {
        #region Properties

        protected BaibakoExternalProvider Service { get; }

        #endregion

        #region .ctor

        public BaibakoTestsBase()
        {
            var retryService = new RetryService();
            Service = new BaibakoExternalProvider(retryService);
        }

        #endregion
    }
}
