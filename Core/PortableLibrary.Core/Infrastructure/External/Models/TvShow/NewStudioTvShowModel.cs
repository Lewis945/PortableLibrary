using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class NewStudioTvShowModel : IExternalModel
    {
        public string Title { get; set; }
        
        public List<NewStudioTvShowSeasonModel> Seasons { get; set; }
    }
}