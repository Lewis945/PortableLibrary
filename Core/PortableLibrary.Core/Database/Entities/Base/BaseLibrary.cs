namespace PortableLibrary.Core.Database.Entities.Base
{
    public class BaseLibrary : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public bool IsPublic { get; set; }
    }
}
