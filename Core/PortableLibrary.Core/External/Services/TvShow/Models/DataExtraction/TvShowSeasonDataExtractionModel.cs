using System.Collections.Generic;

namespace PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction
{
    public class TvShowSeasonDataExtractionModel
    {
        public int Index { get; set; }
        public List<TvShowEpisodeDataExtractionModel> Episodes { get; set; }
        public List<TvShowEpisodeDataExtractionModel> Specials { get; set; }
    }
}