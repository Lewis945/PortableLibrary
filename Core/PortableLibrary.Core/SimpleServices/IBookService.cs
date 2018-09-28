using PortableLibrary.Core.SimpleServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortableLibrary.Core.SimpleServices
{
    public interface IBookService
    {
        Task<IEnumerable<LibraryBookListModel>> GetBooks(string libraryAlias, string userId);
        Task<bool> AddLibraryBookAsync(string bookTitle, string author, string libraryAlias, string userId);
        Task<bool> RemoveLibraryBookAsync(string bookTitle, string author, string libraryAlias, string userId);
    }
}
