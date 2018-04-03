using System.Threading.Tasks;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book
{
    /// <summary>
    /// https://www.litres.ru
    /// </summary>
    public class LitresExternalProvider : IExternalServiceProvider<LitresBookModel>
    {
        public string ProviderUri => "https://www.litres.ru";

        public async Task<LitresBookModel> Extract()
        {
            return new LitresBookModel();
        }
    }
}