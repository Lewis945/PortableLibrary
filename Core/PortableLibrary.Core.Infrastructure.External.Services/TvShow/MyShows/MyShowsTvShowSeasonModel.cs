using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows
{
    public class MyShowsTvShowSeasonModel
    {
        public int Index { get; set; }
        public List<MyShowsTvShowEpisodeModel> Episodes { get; set; }
        public List<MyShowsTvShowEpisodeModel> Specials { get; set; }
    }
}