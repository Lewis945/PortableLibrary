using PortableLibrary.Core.Database;
using PortableLibrary.Core.SimpleServices;

namespace PortableLibrary.Core.Infrastructure.SimpleServices
{
    public class TvShowService : ITvShowService
    {
        #region Fields

        private PortableLibraryDataContext _context;

        #endregion

        #region .ctor

        public TvShowService(PortableLibraryDataContext context)
        {
            _context = context;
        }

        #endregion

        #region ITvShowService

        

        #endregion

        #region Private Methods

        

        #endregion
    }
}
