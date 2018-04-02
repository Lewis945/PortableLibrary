using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.BooksLibrary
{
    public class BooksLibrary : BaseLibrary
    {
        public BooksLibrary()
        {
            Books = new List<LibraryBook>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BooksLibraryId { get; set; }

        public ICollection<LibraryBook> Books { get; set; }
    }
}
