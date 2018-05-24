using System.Collections.Generic;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services.Base.Provider;
using PortableLibrary.Core.External.Services.TvShow.Models.Search;

namespace PortableLibrary.Core.External.Services.TvShow
{
    public interface ITvShowSearchProvider : ISearchProvider
    {
        Task<IEnumerable<TvShowSearchModel>> FindTvShowAsync(string title);
    }
}