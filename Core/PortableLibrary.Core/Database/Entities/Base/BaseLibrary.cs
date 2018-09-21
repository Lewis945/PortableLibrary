using PortableLibrary.Core.Database.Entities.Membership;

namespace PortableLibrary.Core.Database.Entities.Base
{
    public class BaseLibrary : BaseEntity
    {
        public string AppUserId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public bool IsPublic { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
