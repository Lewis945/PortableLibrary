using System.Threading.Tasks;
using PortableLibrary.Core.External.Services.Base.Provider;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.Tracking;

namespace PortableLibrary.Core.External.Services.TvShow
{
    public interface ITvShowTrackingProvider : ITrackingProvider
    {
        Task<TvShowTrackingModel> TrackTvShowAsync(string uri);
    }
}