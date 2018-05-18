using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class BaibakoTvShowModel : IExternalModel
    {
        public string Title { get; set; }
        public List<BaibakoTvShowSeasonModel> Seasons { get; set; }
    }
}