using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class LibraryTvShowSeason
    {
        public LibraryTvShowSeason()
        {
            Episodes = new List<LibraryTvShowEpisode>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryTvShowSeasonId { get; set; }
        public int LibraryTvShowId { get; set; }

        public int LanguageId { get; set; }

        [Required]
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Comments { get; set; }
        public string CoverImage { get; set; }

        public int Index { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsFavourite { get; set; }
        public bool IsWatchingPlanned { get; set; }
        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual LibraryTvShow LibraryTvShow { get; set; }
        public virtual ICollection<LibraryTvShowEpisode> Episodes { get; set; }
    }
}
