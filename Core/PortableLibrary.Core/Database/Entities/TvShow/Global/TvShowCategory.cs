using PortableLibrary.Core.Database.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.TvShow
{
    public class TvShowCategory : BaseEntity
    {
        public TvShowCategory()
        {
            Languages = new List<TvShowCategoryLanguage>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowCategoryId { get; set; }

        public virtual ICollection<TvShowCategoryLanguage> Languages { get; set; }
    }

    public class TvShowCategoryLanguage : BaseLanguageEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TvShowCategoryLanguageId { get; set; }
        public int TvShowCategoryId { get; set; }

        public virtual TvShowCategory TvShowCategory { get; set; }
    }
}
