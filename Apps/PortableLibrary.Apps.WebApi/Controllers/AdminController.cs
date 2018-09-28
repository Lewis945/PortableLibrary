using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Membership;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private PortableLibraryDataContext _context;

        public AdminController(PortableLibraryDataContext context)
        {
            _context = context;
        }

        [HttpPost("clenup/deleted")]
        public async Task<IActionResult> ClenupDeleted()
        {
            try
            {
                #region Libraries

                var bookLibrariesToBeDeletedForever = _context.BookLibraries.Where(l => l.IsDeleted).ToList();
                var tvShowLibrariesToBeDeletedForever = _context.TvShowsLibraries.Where(l => l.IsDeleted).ToList();

                _context.BookLibraries.RemoveRange(bookLibrariesToBeDeletedForever);
                _context.TvShowsLibraries.RemoveRange(tvShowLibrariesToBeDeletedForever);

                #endregion

                #region Libraries items

                var libraryBooksToBeDeletedForever = _context.LibrariesBooks.Where(l => l.IsDeleted).ToList();
                var librayTvShowsToBeDeletedForever = _context.LibrariesTvShows.Where(l => l.IsDeleted).ToList();

                _context.LibrariesBooks.RemoveRange(libraryBooksToBeDeletedForever);
                _context.LibrariesTvShows.RemoveRange(librayTvShowsToBeDeletedForever);

                #endregion

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    user = User.GetUserName(),
                    userid = User.GetUserId(),
                    libraries = bookLibrariesToBeDeletedForever.Select(l => l.Name)
                        .Concat(tvShowLibrariesToBeDeletedForever.Select(l => l.Name)),
                    books = libraryBooksToBeDeletedForever.Select(b => b.Name),
                    tvshows = librayTvShowsToBeDeletedForever.Select(s => s.Name)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { user = User.GetUserName(), userid = User.GetUserId(), ex.Message });
            }
        }
    }
}
