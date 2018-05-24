using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.Baibako
{
    public class BaibakoTvShowModel
    {
        public string Title { get; set; }
        public List<BaibakoTvShowSeasonModel> Seasons { get; set; }
    }
}