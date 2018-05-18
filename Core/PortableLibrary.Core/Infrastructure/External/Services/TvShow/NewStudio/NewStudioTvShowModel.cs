using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.NewStudio
{
    public class NewStudioTvShowModel : IExternalModel
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        
        public List<NewStudioTvShowSeasonModel> Seasons { get; set; }
    }
}