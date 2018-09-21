using System.Collections.Generic;
using System.Threading.Tasks;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.SimpleServices
{
    public interface ILibraryService
    {
        IAsyncEnumerable<LibraryListModel> GetLibrariesAsync(string userId, LibraryType type = LibraryType.None, bool extended = false);

        Task<bool> AddLibraryAsync(string userId, string name, LibraryType type);
        Task<bool> RemoveLibraryAsync(string userId, string name, LibraryType type);
        Task<string> GenerateAliasAsync(string userId, string name, LibraryType type);
    }
}