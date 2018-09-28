using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Apps.WebApi.Models;
using PortableLibrary.Core.Membership;
using PortableLibrary.Core.SimpleServices;
using System.Threading.Tasks;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    [Route("api/library")]
    [ApiController]
    [Authorize]
    public class LibraryTvShowsController : ControllerBase
    {
        private readonly ITvShowService _tvShowService;

        public LibraryTvShowsController(ITvShowService tvShowService)
        {
            _tvShowService = tvShowService;
        }

        [HttpGet("{libraryAlias}/tvshows")]
        public async Task<IActionResult> Index(string libraryAlias)
        {
            var tvShows = await _tvShowService.GetTvShows(libraryAlias, User.GetUserId());
            return Ok(new { user = User.GetUserName(), userid = User.GetUserId(), tvShows });
        }

        [HttpPost("{libraryAlias}/add/tvshow")]
        public async Task<IActionResult> Add(string libraryAlias, [FromBody]LibraryTvShowQuery query)
        {
            var result = await _tvShowService.AddLibraryTvShowAsync(query.Title, libraryAlias, User.GetUserId());
            return Ok(new { query.Title, result });
        }

        [HttpPost("{libraryAlias}/remove/tvshow")]
        public async Task<IActionResult> Remove(string libraryAlias, [FromBody]LibraryTvShowQuery query)
        {
            var result = await _tvShowService.RemoveLibraryTvShowAsync(query.Title, libraryAlias, User.GetUserId());
            return Ok(new { query.Title, result });
        }
    }
}
