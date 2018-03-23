using System;
using System.ComponentModel.DataAnnotations;

namespace PortableLibrary.Core.Database.Entities.Base
{
    public class BaseEntity
    {
        [Required, MaxLength(50)]
        public virtual string Alias { get; set; }

        public virtual int? Position { get; set; }

        public virtual bool IsPublished { get; set; }
        public virtual bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
