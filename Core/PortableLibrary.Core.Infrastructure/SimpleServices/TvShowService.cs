using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.SimpleServices;
using PortableLibrary.Core.SimpleServices.Models;

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

        public async Task<IEnumerable<LibraryTvShowListModel>> GetTvShows(string libraryAlias, string userId)
        {
            try
            {
                var library =
                    await _context.TvShowsLibraries.FirstOrDefaultAsync(lib => !lib.IsDeleted &&
                        lib.AppUserId == userId && lib.Alias == libraryAlias);

                if (library == null)
                    return null;

                return library.TvShows.Select(tvShow => new LibraryTvShowListModel
                {
                    Title = tvShow.Name
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddLibraryTvShowAsync(string tvShowtitle, string libraryTitle, string userId)
        {
            try
            {
                string libraryAlias = libraryTitle.FormatAlias();
                var library =
                    await _context.TvShowsLibraries.FirstOrDefaultAsync(l => !l.IsDeleted && l.AppUserId == userId && l.Alias == libraryAlias);

                if (library == null)
                    return false;

                string tvShowAlias = tvShowtitle.FormatAlias();
                var libraryTvShow = library.TvShows.FirstOrDefault(show => show.Alias == tvShowAlias);

                if (libraryTvShow != null)
                    return false;

                var now = DateTime.Now;

                libraryTvShow = new LibraryTvShow
                {
                    Name = tvShowtitle,
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

        public async Task<bool> RemoveLibraryTvShowAsync(string tvShowTitle, string libraryTitle, string userId)
        {
            try
            {
                string libraryAlias = libraryTitle.FormatAlias();

                var libraryExists = await _context.TvShowsLibraries.AnyAsync(lib => lib.Alias == libraryAlias && lib.AppUserId == userId);
                if (!libraryExists)
                    return false;

                string tvShowAlias = tvShowTitle.FormatAlias();
                var tvShow = await _context.LibrariesTvShows.FirstOrDefaultAsync(show => show.TvShowsLibrary.Alias == libraryAlias &&
                    show.TvShowsLibrary.AppUserId == userId && !show.TvShowsLibrary.IsDeleted && show.Alias == tvShowAlias);

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
