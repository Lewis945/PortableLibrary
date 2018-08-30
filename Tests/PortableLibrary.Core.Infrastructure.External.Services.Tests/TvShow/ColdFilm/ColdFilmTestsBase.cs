using PortableLibrary.Core.Infrastructure.External.Services.TvShow.ColdFilm;
using PortableLibrary.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.ColdFilm
{
    public abstract class ColdFilmTestsBase : TvShowExternalProviderTestsBase
    {
        #region Properties

        public ColdFilmExternalProvider Service { get; }

        #endregion

        #region .ctor

        public ColdFilmTestsBase()
        {
            var retryService = new RetryService();
            Service = new ColdFilmExternalProvider(retryService);
        }

        #endregion
    }
}
