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
        public string ServiceUri => "https://fantlab.ru";
        public string ServiceName => "FantLib";

        public Task<FantasyWorldsBookModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}