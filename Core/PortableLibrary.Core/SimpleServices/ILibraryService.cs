using System.Threading.Tasks;
using PortableLibrary.Core.Enums;

namespace PortableLibrary.Core.SimpleServices
{
    public interface ILibraryService
    {
        Task<bool> AddLibraryAsync(string name, LibraryType type);
        Task<bool> RemoveLibraryAsync(string name, LibraryType type);
        Task<string> GenerateAliasAsync(string name, LibraryType type);
    }
}