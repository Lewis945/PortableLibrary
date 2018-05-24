using System.Collections.Generic;
using PortableLibrary.Core.External.Services.Base.Model;

namespace PortableLibrary.Core.External.Services.TvShow.Models.Search
{
    public class TvShowSearchModel : IItemSearchModel
    {
        public int? Id { get; set; }
        public List<string> Titles { get; set; }
        public string TitleOriginal { get; set; }
        public int? Year { get; set; }
        public string Image { get; set; }
    }
}