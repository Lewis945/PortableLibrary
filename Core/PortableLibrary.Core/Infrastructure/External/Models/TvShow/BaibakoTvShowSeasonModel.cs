using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class BaibakoTvShowSeasonModel
    {
        public int Index { get; set; }
        public List<BaibakoTvShowEpisodeModel> Episodes { get; set; }
    }
}