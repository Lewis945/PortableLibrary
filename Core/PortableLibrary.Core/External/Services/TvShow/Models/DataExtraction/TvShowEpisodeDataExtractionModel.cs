using System;
using System.Collections.Generic;

namespace PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction
{
    public class TvShowEpisodeDataExtractionModel
    {
        public int? Id { get; set; }

        public string OriginalTitle { get; set; }
        public string ShortName { get; set; }
        public List<string> Titles { get; set; }

        public int Index { get; set; }

        public DateTimeOffset? DateReleased { get; set; }
        public DateTimeOffset? DateReleasedOrigianl { get; set; }

        public string ImageUri { get; set; }
    }
}