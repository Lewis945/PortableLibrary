using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class LostFilmTvShowModel : IExternalModel
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Description { get; set; }

        public string ImageUri { get; set; }
        public byte[] ImageByteArray { get; set; }
        
        public bool? IsComplete { get; set; }

        public List<string> Genres { get; set; }
        
        public List<LostFilmTvShowSeasonModel> Seasons { get; set; }
    }
}