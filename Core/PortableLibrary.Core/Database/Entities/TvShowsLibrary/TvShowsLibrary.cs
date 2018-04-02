using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShowsLibrary
{
    public sealed class TvShowsLibrary : BaseLibrary
    {
        public TvShowsLibrary()
        {
            TvShows = new List<LibraryTvShow>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowsLibraryId { get; set; }

        public virtual ICollection<LibraryTvShow> TvShows { get; set; }
    }
}
