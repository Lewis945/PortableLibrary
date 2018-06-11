using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PortableLibrary.Core.Membership
{
    public class MembershipDataContext : IdentityDbContext<AppUser>
    {
        public MembershipDataContext(DbContextOptions options)
              : base(options)
        {
        }
    }
}
