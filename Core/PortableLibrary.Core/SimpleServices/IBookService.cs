using System.Threading.Tasks;

namespace PortableLibrary.Core.SimpleServices
{
    public interface IBookService
    {
        Task<bool> AddLibraryBookAsync(string bookName, string author, string userId, string libraryName);
    }
}
