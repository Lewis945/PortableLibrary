using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.Baibako
{
    public class BaibakoTvShowSeasonModel
    {
        public int Index { get; set; }
        public List<BaibakoTvShowEpisodeModel> Episodes { get; set; }
    }
}