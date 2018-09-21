using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Database.Entities.Membership;
using PortableLibrary.Core.Infrastructure.Membership.Controllers;

namespace PortableLibrary.Apps.WebApi.Controllers
{
    public class AccountsController : AccountsBaseController
    {
        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, PortableLibraryDataContext appDbContext) : base(userManager, mapper, appDbContext)
        {
        }
    }
}
