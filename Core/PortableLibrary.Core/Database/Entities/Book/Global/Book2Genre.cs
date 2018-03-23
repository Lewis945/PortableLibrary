using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class Book2Genre
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Book2GenreId { get; set; }
        public int BookId { get; set; }
        public int BookGenreId { get; set; }

        public virtual Book Book { get; set; }
        public virtual BookGenre BookGenre { get; set; }
    }
}
