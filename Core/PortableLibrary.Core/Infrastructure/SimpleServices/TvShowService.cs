using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.TvShow;
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

        public async Task<bool> AddLibraryTvShowAsync(string tvShowName, string libraryName)
        {
            try
            {
                var library =
                    await _context.TvShowsLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.Name == libraryName);

                if (library == null)
                    return false;

                var libraryTvShow = library.TvShows.FirstOrDefault(show => show.Name == tvShowName);

                if (libraryTvShow != null)
                    return false;

                libraryTvShow = new LibraryTvShow
                {
                    Name = tvShowName
                };

                library.TvShows.Add(libraryTvShow);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Private Methods

        

        #endregion
    }
}
