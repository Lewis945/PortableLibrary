using PortableLibrary.Core.Infrastructure.External.Services.TvShow.NewStudio;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.NewStudio
{
    public abstract class NewstudioTestsBase : TvShowExternalProviderTestsBase
    {
        #region Properties

        public NewStudioExternalProvider Service { get; }

        #endregion

        #region .ctor

        public NewstudioTestsBase()
        {
            var retryService = new RetryService();
            Service = new NewStudioExternalProvider(retryService);
        }

        #endregion
    }
}
