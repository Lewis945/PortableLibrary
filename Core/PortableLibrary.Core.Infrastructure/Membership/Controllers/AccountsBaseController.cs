using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.Membership;
using PortableLibrary.Core.Infrastructure.Models;
using PortableLibrary.Core.Membership.Validation;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.Membership.Controllers
{
    [Route("api/membership")]
    [ApiController]
    public abstract class AccountsBaseController : ControllerBase
    {
        private readonly PortableLibraryDataContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger _logger;

        public AccountsBaseController(UserManager<AppUser> userManager, IMapper mapper,
            IStringLocalizerFactory localizerFactory, ILogger<AccountsBaseController> logger, PortableLibraryDataContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _localizer = localizerFactory.Create("Common.Validation", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            _logger = logger;
            //_appDbContext = appDbContext;
        }

        // POST api/accounts
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IList<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody]RegisterModel credentials)
        {
            _logger.LogInformation($"Register request for {credentials.Email}.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validator = new RegisterModelValidator();
            var validationResult = await validator.ValidateAsync(credentials);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userIdentity = _mapper.Map<AppUser>(credentials);

            var result = await _userManager.CreateAsync(userIdentity, credentials.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            //await _appDbContext.SaveChangesAsync();

            return Ok("Account created");
        }
    }
}
