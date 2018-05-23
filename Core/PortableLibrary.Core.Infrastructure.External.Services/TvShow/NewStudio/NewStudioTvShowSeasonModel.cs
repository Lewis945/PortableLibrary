using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.NewStudio
{
    public class NewStudioTvShowSeasonModel
    {
        public int Index { get; set; }
        public List<NewStudioTvShowEpisodeModel> Episodes { get; set; }
    }
}