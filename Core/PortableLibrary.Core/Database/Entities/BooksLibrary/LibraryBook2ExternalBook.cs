using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.BooksLibrary
{
    public class LibraryBook2ExternalBook
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryBook2ExternalBookId { get; set; }
        public int LibraryBookId { get; set; }
        public int ExternalBookId { get; set; }

        public virtual LibraryBook LibraryBook { get; set; }
        public virtual ExternalBook ExternalBook { get; set; }
    }
}