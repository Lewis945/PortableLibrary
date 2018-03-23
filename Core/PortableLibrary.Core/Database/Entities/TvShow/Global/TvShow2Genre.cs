using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class TvShow2Genre
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShow2GenreId { get; set; }
        public int TvShowId { get; set; }
        public int TvShowGenreId { get; set; }

        public virtual TvShow TvShow { get; set; }
        public virtual TvShowGenre TvShowGenre { get; set; }
    }
}
