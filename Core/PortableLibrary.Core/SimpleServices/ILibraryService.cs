using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.SimpleServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortableLibrary.Core.SimpleServices
{
    public interface ILibraryService
    {
        IAsyncEnumerable<LibraryListModel> GetLibrariesAsync(string userId, LibraryType type = LibraryType.None, bool extended = false);

        Task<LibraryModel> GetLibraryAsync(LibraryType type, string alias, string userId, bool extended = false);
        Task<bool> AddLibraryAsync(string title, LibraryType type, string userId);
        Task<bool> RemoveLibraryAsync(string title, LibraryType type, string userId);
    }
}