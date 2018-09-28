using PortableLibrary.Core.SimpleServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortableLibrary.Core.SimpleServices
{
    public interface ITvShowService
    {
        Task<IEnumerable<LibraryTvShowListModel>> GetTvShows(string libraryAlias, string userId);
        Task<bool> AddLibraryTvShowAsync(string tvShowTitle, string libraryTitle, string userId);
        Task<bool> RemoveLibraryTvShowAsync(string tvShowTitle, string libraryTitle, string userId);
    }
}
