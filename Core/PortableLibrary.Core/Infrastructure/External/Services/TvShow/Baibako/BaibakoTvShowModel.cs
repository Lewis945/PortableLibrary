using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.Baibako
{
    public class BaibakoTvShowModel : IExternalModel
    {
        public string Title { get; set; }
        public List<BaibakoTvShowSeasonModel> Seasons { get; set; }
    }
}