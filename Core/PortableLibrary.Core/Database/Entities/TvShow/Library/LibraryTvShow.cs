using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class LibraryTvShow
    {
        public LibraryTvShow()
        {
            Seasons = new List<LibraryTvShowSeason>();
            //Genres = new List<LibraryBookGenre>();
            //Categories = new List<LibraryBookCategory>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryTvShowId { get; set; }
        public int? TvShowId { get; set; }
        public int TvShowsLibraryId { get; set; }

        public int LanguageId { get; set; }

        [Required]
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Comments { get; set; }
        public string CoverImage { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsFavourite { get; set; }
        public bool IsClosed { get; set; }
        public bool IsWatched { get; set; }
        public bool IsWatchingPlanned { get; set; }
        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual TvShow TvShow { get; set; }
        public virtual TvShowsLibrary TvShowsLibrary { get; set; }

        public virtual ICollection<LibraryTvShowSeason> Seasons { get; set; }
        //public virtual ICollection<LibraryBookGenre> Genres { get; set; }
        //public virtual ICollection<LibraryBookCategory> Categories { get; set; }
    }
}
