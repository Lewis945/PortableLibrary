using System.ComponentModel.DataAnnotations;

namespace PortableLibrary.Core.Database.Entities.Base
{
    public class BaseLanguageEntity
    {
        public virtual int LanguageId { get; set; }

        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
