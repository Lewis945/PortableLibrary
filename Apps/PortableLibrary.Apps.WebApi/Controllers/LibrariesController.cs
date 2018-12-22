using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Apps.WebApi.Models;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Infrastructure.Templating.Libraries;
using PortableLibrary.Core.Membership;
using PortableLibrary.Core.SimpleServices;
using PortableLibrary.Core.SimpleServices.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [ProducesResponseType(typeof(IEnumerable<LibraryListModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Index(LibraryType type = LibraryType.None)
        {
            var libraries = _libraryService.GetLibrariesAsync(User.GetUserId(), type);
            var librariesList = await libraries.ToList();
            if (librariesList.Count == 0)
                return BadRequest("Libraries not found!");

            return Ok(librariesList);
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
            if (!result)
                return BadRequest(query);

            return Ok(query);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody]LibraryQuery query)
        {
            var result = await _libraryService.RemoveLibraryAsync(query.Title, query.Type, User.GetUserId());
            if (!result)
                return BadRequest(query);

            return Ok(query);
        }
    }
}