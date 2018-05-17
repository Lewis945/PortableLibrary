using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class ColdFilmTvShowModel : IExternalModel
    {
        public string Title { get; set; }
        public List<ColdFilmTvShowSeasonModel> Seasons { get; set; }
    }
}