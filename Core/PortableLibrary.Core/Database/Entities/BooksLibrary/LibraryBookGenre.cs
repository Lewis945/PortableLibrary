using PortableLibrary.Core.Database.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.BooksLibrary
{
    public class LibraryBookGenre : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryBookGenreId { get; set; }
        public int LibraryBookId { get; set; }

        public string Name { get; set; }

        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual LibraryBook LibraryBook { get; set; }
    }
}
