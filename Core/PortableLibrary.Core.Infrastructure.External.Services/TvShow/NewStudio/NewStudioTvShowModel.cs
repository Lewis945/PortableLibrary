using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.NewStudio
{
    public class NewStudioTvShowModel
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }

        public List<NewStudioTvShowSeasonModel> Seasons { get; set; }
    }
}