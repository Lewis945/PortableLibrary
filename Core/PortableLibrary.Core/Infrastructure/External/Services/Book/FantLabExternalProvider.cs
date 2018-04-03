using System;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book
{
    /// <summary>
    /// https://fantlab.ru
    /// </summary>
    public class FantLabExternalProvider : IExternalServiceProvider<FantasyWorldsBookModel>
    {
        public string ProviderUri => "https://fantlab.ru";

        public Task<FantasyWorldsBookModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}