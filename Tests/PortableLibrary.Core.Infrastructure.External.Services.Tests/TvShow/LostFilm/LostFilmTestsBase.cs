using PortableLibrary.Core.Infrastructure.External.Services.TvShow.LostFilm;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.LostFilm
{
    public abstract class LostFilmTestsBase : TvShowExternalProviderTestsBase
    {
        #region Properties

        protected LostFilmExternalProvider Service { get; set; }

        #endregion

        #region .ctor

        public LostFilmTestsBase()
        {
            var retryService = new RetryService();
            Service = new LostFilmExternalProvider(retryService);
        }

        #endregion
    }
}
