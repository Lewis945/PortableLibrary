using PortableLibrary.Core.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShowsLibrary
{
    public sealed class LibraryTvShow : BaseEntity
    {
        public LibraryTvShow()
        {
            Categories = new List<LibraryTvShowCategory>();
            Genres = new List<LibraryTvShowGenre>();
            Seasons = new List<LibraryTvShowSeason>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryTvShowId { get; set; }
        public int TvShowsLibraryId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Comments { get; set; }
        public string CoverImage { get; set; }

        public bool IsFavourite { get; set; }
        public bool IsClosed { get; set; }
        public bool IsWatched { get; set; }
        public bool IsWatchingPlanned { get; set; }
        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual TvShowsLibrary TvShowsLibrary { get; set; }

        public virtual ICollection<LibraryTvShowCategory> Categories { get; set; }
        public virtual ICollection<LibraryTvShowGenre> Genres { get; set; }
        public virtual ICollection<LibraryTvShowSeason> Seasons { get; set; }
    }
}
