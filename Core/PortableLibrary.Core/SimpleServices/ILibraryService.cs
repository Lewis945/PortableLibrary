using System.Collections.Generic;
using System.Threading.Tasks;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.SimpleServices
{
    public interface ILibraryService
    {
        IAsyncEnumerable<LibraryListModel> GetLibrariesAsync(LibraryType type = LibraryType.None, bool extended = false);

        Task<bool> AddLibraryAsync(string name, LibraryType type);
        Task<bool> RemoveLibraryAsync(string name, LibraryType type);
        Task<string> GenerateAliasAsync(string name, LibraryType type);
    }
}