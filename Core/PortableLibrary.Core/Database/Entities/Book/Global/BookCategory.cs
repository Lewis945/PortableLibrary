using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class BookCategory : BaseEntity
    {
        public BookCategory()
        {
            Languages = new List<BookCategoryLanguage>();
            SubCategories = new List<BookCategory>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookCategoryId { get; set; }
        public int? ParentBookCategoryId { get; set; }

        public virtual BookCategory ParentBookCategory { get; set; }
        public virtual ICollection<BookCategory> SubCategories { get; set; }
        public virtual ICollection<BookCategoryLanguage> Languages { get; set; }
    }

    public class BookCategoryLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookCategoryLanguageId { get; set; }
        public int BookCategoryId { get; set; }

        public virtual BookCategory BookCategory { get; set; }
    }
}
