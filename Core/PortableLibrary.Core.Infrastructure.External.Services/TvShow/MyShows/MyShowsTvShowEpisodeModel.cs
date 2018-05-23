using System;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows
{
    public class MyShowsTvShowEpisodeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTimeOffset AirDate { get; set; }
        public string Image { get; set; }
        public string ShortName { get; set; }
    }
}