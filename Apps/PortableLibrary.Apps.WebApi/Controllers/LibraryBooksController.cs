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
    public class LibraryBooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public LibraryBooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{libraryAlias}/books")]
        public async Task<IActionResult> Index(string libraryAlias)
        {
            var books = await _bookService.GetBooks(libraryAlias, User.GetUserId());
            return Ok(new { user = User.GetUserName(), userid = User.GetUserId(), books });
        }

        [HttpPost("{libraryAlias}/add/book")]
        public async Task<IActionResult> Add(string libraryAlias, [FromBody]LibraryBookQuery query)
        {
            var result = await _bookService.AddLibraryBookAsync(query.Title, query.Author, libraryAlias, User.GetUserId());
            return Ok(new { query.Title, query.Author, result });
        }

        [HttpPost("{libraryAlias}/remove/book")]
        public async Task<IActionResult> Remove(string libraryAlias, [FromBody]LibraryBookQuery query)
        {
            var result = await _bookService.RemoveLibraryBookAsync(query.Title, query.Author, libraryAlias, User.GetUserId());
            return Ok(new { query.Title, query.Author, result });
        }
    }
}
