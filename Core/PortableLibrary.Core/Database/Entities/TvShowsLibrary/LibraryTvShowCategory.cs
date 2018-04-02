using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShowsLibrary
{
    public sealed class LibraryTvShowCategory : BaseEntity
    {
        public LibraryTvShowCategory()
        {
            SubCategories = new List<LibraryTvShowCategory>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryTvShowCategoryId { get; set; }
        public int? ParentLibraryTvShowCategoryId { get; set; }
        public int LibraryTvShowId { get; set; }

        public string Name { get; set; }

        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual LibraryTvShowCategory ParentLibraryTvShowCategory { get; set; }
        public virtual LibraryTvShow LibraryTvShow { get; set; }
        public virtual ICollection<LibraryTvShowCategory> SubCategories { get; set; }
    }
}
