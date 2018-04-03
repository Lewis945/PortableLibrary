using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.BooksLibrary
{
    public class LibraryBookExternalService
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryBookExternalServiceId { get; set; }

        public int LibraryBookId { get; set; }

        public string Name { get; set; }
        public string Uri { get; set; }

        public bool IsTrackable { get; set; }

        #region Litres fields to track for changes

        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public string AuthorSeries { get; set; }

        public int Index { get; set; }
        public int PagesCount { get; set; }

        public List<string> PublishersSeries { get; set; }
        public List<string> Genres { get; set; }

        #endregion

        public virtual LibraryBook LibraryBook { get; set; }
    }
}