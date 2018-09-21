using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Apps.WebApi.Models;
using PortableLibrary.Core.Infrastructure.SimpleServices;
using System.Linq;
using System.Threading.Tasks;
using PortableLibrary.Core.Membership;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibrariesController : ControllerBase
    {
        private LibraryService _libraryService;

        public LibrariesController(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public async Task<IActionResult> Index()
        {
            var libraries = _libraryService.GetLibrariesAsync(User.GetUserId());
            var t = await libraries.ToList();
            if (t.Count == 0)
                return BadRequest();

            return Ok(new { user = User.GetUserName(), userid = User.GetUserId(), libs = t.Select(l => l.Title) });
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]LibraryQuery query)
        {
            var result = await _libraryService.AddLibraryAsync(User.GetUserId(), query.Name, query.Type);
            return Ok(new { query.Name, type = query.Type.ToString(), result });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody]LibraryQuery query)
        {
            var result = await _libraryService.RemoveLibraryAsync(User.GetUserId(), query.Name, query.Type);
            return Ok(new { query.Name, type = query.Type.ToString(), result });
        }
    }
}