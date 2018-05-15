using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class NewStudioTvShowSeasonModel
    {
        public int Index { get; set; }
        public List<NewStudioTvShowEpisodeModel> Episodes { get; set; }
    }
}