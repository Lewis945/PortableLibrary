using System.Threading.Tasks;

namespace PortableLibrary.Core.SimpleServices
{
    public interface ITvShowService
    {
        Task<bool> AddLibraryTvShowAsync(string tvShowName, string userId, string libraryName);
    }
}
