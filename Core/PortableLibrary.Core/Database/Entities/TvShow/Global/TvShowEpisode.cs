using PortableLibrary.Core.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class TvShowEpisode : BaseEntity
    {
        public TvShowEpisode()
        {
            Languages = new List<TvShowEpisodeLanguage>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowEpisodeId { get; set; }
        public int TvShowSeasonId { get; set; }

        public string CoverImage { get; set; }

        public int Index { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsWaitingModeration { get; set; }

        public virtual TvShowSeason TvShowSeason { get; set; }
        public virtual ICollection<TvShowEpisodeLanguage> Languages { get; set; }
    }

    public class TvShowEpisodeLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowEpisodeLanguageId { get; set; }
        public int TvShowEpisodeId { get; set; }

        public virtual TvShowEpisode TvShowEpisode { get; set; }
    }
}
