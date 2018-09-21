using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PortableLibrary.Core.Database.Entities.Membership;
using PortableLibrary.Core.Infrastructure.Membership;
using PortableLibrary.Core.Infrastructure.Membership.Controllers;
using PortableLibrary.Core.Membership;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    public class AuthController : AuthBaseController
    {
        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions) : base(userManager, jwtFactory, jwtOptions)
        {
        }
    }
}
