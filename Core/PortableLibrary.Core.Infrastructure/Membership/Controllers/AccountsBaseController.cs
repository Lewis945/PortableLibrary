using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.Membership;
using PortableLibrary.Core.Infrastructure.Models;
using PortableLibrary.Core.Membership;

namespace PortableLibrary.Core.Infrastructure.Membership.Controllers
{
    [Route("api/membership")]
    [ApiController]
    public abstract class AccountsBaseController : ControllerBase
    {
        private readonly PortableLibraryDataContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsBaseController(UserManager<AppUser> userManager, IMapper mapper, PortableLibraryDataContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        // POST api/accounts
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            await _appDbContext.SaveChangesAsync();

            return Ok("Account created");
        }
    }
}
