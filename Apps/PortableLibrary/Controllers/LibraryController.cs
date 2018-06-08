using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Core.SimpleServices;

namespace PortableLibrary.Controllers
{
    [Route("api/libraries")]
    public class LibraryController : Controller
    {
        #region Fields

        private readonly ILibraryService _libraryService;

        #endregion

        #region .ctor

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var libraries = _libraryService.GetLibrariesAsync();
            
            return Json("");
        }

        #endregion
    }
}