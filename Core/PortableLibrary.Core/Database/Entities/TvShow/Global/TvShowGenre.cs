using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class TvShowGenre : BaseEntity
    {
        public TvShowGenre()
        {
            Languages = new List<TvShowGenreLanguage>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowGenreId { get; set; }

        public virtual ICollection<TvShowGenreLanguage> Languages { get; set; }
    }

    public class TvShowGenreLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowGenreLanguageId { get; set; }
        public int TvShowGenreId { get; set; }

        public virtual TvShowGenre TvShowGenre { get; set; }
    }
}
