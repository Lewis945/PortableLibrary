using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Extensions;
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

        public async Task<bool> AddLibraryTvShowAsync(string tvShowName, string userId, string libraryName)
        {
            try
            {
                string libraryAlias = libraryName.FormatAlias();
                var library =
                    await _context.TvShowsLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.AppUserId == userId && l.Alias == libraryAlias);

                if (library == null)
                    return false;

                string tvShowAlias = tvShowName.FormatAlias();
                var libraryTvShow = library.TvShows.FirstOrDefault(show => show.Alias == tvShowAlias);

                if (libraryTvShow != null)
                    return false;

                var now = DateTime.Now;

                libraryTvShow = new LibraryTvShow
                {
                    Name = tvShowName,
                    DateCreated = now,
                    DateModified = now,
                    Alias = tvShowAlias,
                    IsPublished = true
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

        public async Task<bool> RemoveLibraryTvShowAsync(string name, string userId, string libraryName)
        {
            try
            {
                string libraryAlias = libraryName.FormatAlias();
                string tvShowAlias = name.FormatAlias();
                var tvShow = await _context.LibrariesTvShows.FirstOrDefaultAsync(show => show.TvShowsLibrary.Alias == libraryAlias && show.TvShowsLibrary.AppUserId == userId &&
                !show.TvShowsLibrary.IsDeleted && show.Alias == tvShowAlias);

                if (tvShow == null)
                    return false;

                tvShow.IsDeleted = true;

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
