using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PortableLibrary.Core.Database.Entities.Base;

namespace PortableLibrary.Core.Database.Entities.BooksLibrary
{
    public class LibraryBook : BaseLibraryEntity
    {
        public LibraryBook()
        {
            Genres = new List<LibraryBookGenre>();
            Categories = new List<LibraryBookCategory>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryBookId { get; set; }

        public int BooksLibraryId { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Author { get; set; }

        public virtual BooksLibrary BooksLibrary { get; set; }

        public virtual ICollection<LibraryBookGenre> Genres { get; set; }
        public virtual ICollection<LibraryBookCategory> Categories { get; set; }
    }
}