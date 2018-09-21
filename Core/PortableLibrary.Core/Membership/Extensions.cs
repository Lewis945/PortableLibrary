using System.Linq;
using System.Security.Claims;

namespace PortableLibrary.Core.Membership
{
    public static class Extensions
    {
        public static string GetUserName(this ClaimsPrincipal principal)
            => principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


        public static string GetUserId(this ClaimsPrincipal principal) =>
            principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

    }
}
