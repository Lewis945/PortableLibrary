using PortableLibrary.Core.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class Book : BaseEntity
    {
        public Book()
        {
            Languages = new List<BookLanguage>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        public string OriginalName { get; set; }
        [Required]
        public string CoverImage { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<BookLanguage> Languages { get; set; }
    }

    public class BookLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookLanguageId { get; set; }
        public int BookId { get; set; }

        public string LanguageSpecificCoverImage { get; set; }

        public virtual Book Book { get; set; }
    }
}
