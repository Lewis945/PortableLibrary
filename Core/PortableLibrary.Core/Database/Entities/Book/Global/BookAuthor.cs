using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class BookAuthor : BaseEntity
    {
        public BookAuthor()
        {
            Languages = new List<BookAuthorLanguage>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookAuthorId { get; set; }

        public string Photo { get; set; }

        public virtual ICollection<BookAuthorLanguage> Languages { get; set; }
    }

    public class BookAuthorLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookAuthorLanguageId { get; set; }
        public int BookAuthorId { get; set; }

        public virtual BookAuthor BookAuthor { get; set; }
    }

}
