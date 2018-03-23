using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class LibraryBookCategory
    {
        public LibraryBookCategory()
        {
            SubCategories = new List<LibraryBookCategory>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryBookCategoryId { get; set; }
        public int? ParentLibraryBookCategoryId { get; set; }
        public int LibraryBookId { get; set; }

        public int LanguageId { get; set; }

        public string Name { get; set; }

        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual LibraryBookCategory ParentLibraryBookCategory { get; set; }
        public virtual LibraryBook LibraryBook { get; set; }
        public virtual ICollection<LibraryBookCategory> SubCategories { get; set; }
    }
}
