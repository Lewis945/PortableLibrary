using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class Book2Author
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Book2AuthorId { get; set; }
        public int BookId { get; set; }
        public int BookAuthorId { get; set; }

        public virtual Book Book { get; set; }
        public virtual BookAuthor BookAuthor { get; set; }
    }
}
