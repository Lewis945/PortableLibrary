using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PortableLibrary.Core.Database.Entities.Membership;
using PortableLibrary.Core.Membership;
using PortableLibrary.Core.Membership.Models;
using PortableLibrary.Core.Membership.Validation;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.Membership.Controllers
{
    [Route("api/membership")]
    [ApiController]
    public abstract class AuthBaseController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger _logger;

        public AuthBaseController(UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,
            IStringLocalizerFactory localizerFactory, ILogger<AuthBaseController> logger)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _localizer = localizerFactory.Create("Common.Validation", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validator = new LoginModelValidator();
            var result = await validator.ValidateAsync(credentials);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            if (identity == null)
            {
                ModelState.AddModelError("login_failure", _localizer["LoginFailure"]);
                return BadRequest(ModelState);
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return Ok(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null).ConfigureAwait(false);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password).ConfigureAwait(false))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id)).ConfigureAwait(false);
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null).ConfigureAwait(false);
        }
    }
}
