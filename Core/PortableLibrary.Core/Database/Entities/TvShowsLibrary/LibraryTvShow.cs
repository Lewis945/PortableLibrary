using PortableLibrary.Core.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShowsLibrary
{
    public class LibraryTvShow : BaseLibraryEntity
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

        public bool IsClosed { get; set; }

        public virtual TvShowsLibrary TvShowsLibrary { get; set; }

        public virtual ICollection<LibraryTvShowCategory> Categories { get; set; }
        public virtual ICollection<LibraryTvShowGenre> Genres { get; set; }
        public virtual ICollection<LibraryTvShowSeason> Seasons { get; set; }
    }
}
