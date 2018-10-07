using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Core.Database;
using System.Collections.Generic;
using System.Linq;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private PortableLibraryDataContext _dataContext;

        public InfoController(PortableLibraryDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult<string> Version()
        {
            return "1.0";
        }

        [HttpGet]
        public ActionResult<string> Author()
        {
            return "Vitalii Zakharov";
        }

        [HttpGet]
        public ActionResult<List<string>> PublicLibraries()
        {
            return _dataContext.BookLibraries.Select(l => l.Name).ToList();
        }
    }
}
