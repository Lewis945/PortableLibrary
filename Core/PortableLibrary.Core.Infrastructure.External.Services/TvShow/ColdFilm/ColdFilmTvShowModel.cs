using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.ColdFilm
{
    public class ColdFilmTvShowModel : IExternalModel
    {
        public string Title { get; set; }
        public List<ColdFilmTvShowSeasonModel> Seasons { get; set; }
    }
}