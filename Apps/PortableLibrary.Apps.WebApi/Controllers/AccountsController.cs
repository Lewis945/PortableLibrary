using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.Membership;
using PortableLibrary.Core.Infrastructure.Membership.Controllers;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    public class AccountsController : AccountsBaseController
    {
        public AccountsController(UserManager<AppUser> userManager, IMapper mapper,
            IStringLocalizerFactory localizerFactory, ILogger<AccountsBaseController> logger, PortableLibraryDataContext appDbContext)
            : base(userManager, mapper, localizerFactory, logger, appDbContext)
        {
        }
    }
}
