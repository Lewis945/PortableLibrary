using PortableLibrary.Core.Database.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShowsLibrary
{
    public class LibraryTvShowGenre : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryTvShowGenreId { get; set; }
        public int LibraryTvShowId { get; set; }

        public string Name { get; set; }

        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual LibraryTvShow LibraryTvShow { get; set; }
    }
}
