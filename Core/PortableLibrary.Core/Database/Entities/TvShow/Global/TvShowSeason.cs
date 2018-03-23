using PortableLibrary.Core.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class TvShowSeason : BaseEntity
    {
        public TvShowSeason()
        {
            Languages = new List<TvShowSeasonLanguage>();
            Episodes = new List<TvShowEpisode>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowSeasonId { get; set; }
        public int TvShowId { get; set; }

        public string CoverImage { get; set; }

        public int Index { get; set; }

        public DateTime ReleaseDate { get; set; }

        public virtual TvShow TvShow { get; set; }
        public virtual ICollection<TvShowSeasonLanguage> Languages { get; set; }
        public virtual ICollection<TvShowEpisode> Episodes { get; set; }
    }

    public class TvShowSeasonLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowSeasonLanguageId { get; set; }
        public int TvShowSeasonId { get; set; }

        public virtual TvShowSeason TvShowSeason { get; set; }
    }
}
