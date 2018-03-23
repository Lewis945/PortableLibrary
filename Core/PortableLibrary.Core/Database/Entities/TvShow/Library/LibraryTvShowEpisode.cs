using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class LibraryTvShowEpisode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryTvShowEpisodeId { get; set; }
        public int LibraryTvShowSeasonId { get; set; }

        public int LanguageId { get; set; }

        [Required]
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Comments { get; set; }
        public string CoverImage { get; set; }

        public int Index { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsFavourite { get; set; }
        public bool IsWatched { get; set; }
        public bool IsWatchingPlanned { get; set; }
        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual LibraryTvShowSeason LibraryTvShowSeason { get; set; }
    }
}
