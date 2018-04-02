using PortableLibrary.Core.Database.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShowsLibrary
{
    public sealed class LibraryTvShowEpisode : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryTvShowEpisodeId { get; set; }
        public int LibraryTvShowSeasonId { get; set; }

        [Required]
        public string Name { get; set; }
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
