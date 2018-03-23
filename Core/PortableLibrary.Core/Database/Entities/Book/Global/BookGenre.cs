using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class BookGenre : BaseEntity
    {
        public BookGenre()
        {
            Languages = new List<BookGenreLanguage>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookGenreId { get; set; }

        public virtual ICollection<BookGenreLanguage> Languages { get; set; }
    }

    public class BookGenreLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookGenreLanguageId { get; set; }
        public int BookGenreId { get; set; }

        public virtual BookGenre BookGenre { get; set; }
    }
}
