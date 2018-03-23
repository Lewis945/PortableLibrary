using PortableLibrary.Core.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class TvShow : BaseEntity
    {
        public TvShow()
        {
            Languages = new List<TvShowLanguage>();
            Seasons = new List<TvShowSeason>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowId { get; set; }

        public string CoverImage { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsWaitingModeration { get; set; }

        public virtual ICollection<TvShowLanguage> Languages { get; set; }
        public virtual ICollection<TvShowSeason> Seasons { get; set; }
    }

    public class TvShowLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowLanguageId { get; set; }
        public int TvShowId { get; set; }

        public virtual TvShow TvShow { get; set; }
    }
}
