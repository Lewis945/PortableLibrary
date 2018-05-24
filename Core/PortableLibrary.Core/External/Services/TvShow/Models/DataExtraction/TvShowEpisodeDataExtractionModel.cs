using System;
using System.Collections.Generic;

namespace PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction
{
    public class TvShowEpisodeDataExtractionModel
    {
        public int? Id { get; set; }
        public List<string> Titles { get; set; }
        public int? EpisodeNumber { get; set; }
        public DateTimeOffset? AirDate { get; set; }
        public string Image { get; set; }
        public string ShortName { get; set; }
    }
}