using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows
{
    public class MyShowsTvShowSeasonModel
    {
        public int SeasonIndex { get; set; }
        public List<MyShowsTvShowEpisodeModel> Episodes { get; set; }
    }
}