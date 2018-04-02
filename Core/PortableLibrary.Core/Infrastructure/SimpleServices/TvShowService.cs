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

                var now = DateTime.Now;

                libraryTvShow = new LibraryTvShow
                {
                    Name = tvShowName,
                    DateCreated = now,
                    DateModified = now,
                    Alias = await GenerateLibraryTvShowAliasAsync(tvShowName),
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

        public async Task<bool> RemoveLibraryTvShowAsync(string name)
        {
            try
            {
                var tvShow = await _context.LibrariesTvShows.FirstOrDefaultAsync(show => show.Name == name);

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

        public async Task<string> GenerateLibraryTvShowAliasAsync(string name)
        {
            var alias = name.FormatAlias();
            var tvShow = await _context.LibrariesTvShows.FirstOrDefaultAsync(show => show.Alias == alias);
            if (tvShow != null)
                throw new Exception($"Tv show with the given alias ({alias}) already exists.");
            return alias;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
