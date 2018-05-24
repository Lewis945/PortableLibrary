using System.Threading.Tasks;
using PortableLibrary.Core.External.Services.Base.Provider;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;

namespace PortableLibrary.Core.External.Services.TvShow
{
    public interface ITvShowDataExtractionProvider : IDataExtractionProvider
    {
        Task<TvShowDataExtractionModel> ExtractTvShowAsync(string uri);
    }
}