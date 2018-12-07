using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Apps.WebApi.Models;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Infrastructure.Templating.Libraries;
using PortableLibrary.Core.Membership;
using PortableLibrary.Core.SimpleServices;
using System.Linq;
using System.Threading.Tasks;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    [Route("api/libraries")]
    [ApiController]
    [Authorize]
    public class LibrariesController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibrariesController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("{type?}")]
        public async Task<IActionResult> Index(LibraryType type = LibraryType.None)
        {
            var libraries = _libraryService.GetLibrariesAsync(User.GetUserId(), type);
            var t = await libraries.ToList();
            if (t.Count == 0)
                return BadRequest();

            return Ok(new { user = User.GetUserName(), userid = User.GetUserId(), libs = t });
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf(bool extended)
        {
            var librariesPdfService = new LibrariesPdfService();
            var libraries = await _libraryService.GetLibrariesAsync(User.GetUserId()).ToList();
            var bytes = librariesPdfService.GeneratePdf(libraries, extended);
            return File(bytes, "application/pdf");
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]LibraryQuery query)
        {
            var result = await _libraryService.AddLibraryAsync(query.Title, query.Type, User.GetUserId());
            return Ok(new { query.Title, type = query.Type.ToString(), result });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody]LibraryQuery query)
        {
            var result = await _libraryService.RemoveLibraryAsync(query.Title, query.Type, User.GetUserId());
            return Ok(new { query.Title, type = query.Type.ToString(), result });
        }
    }
}