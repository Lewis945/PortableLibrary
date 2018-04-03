using System;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book
{
    /// <summary>
    /// https://fantasy-worlds.org
    /// </summary>
    public class FantasyWorldsExternalProvider : IExternalServiceProvider<FantasyWorldsBookModel>
    {
        public string ServiceUri => "https://fantasy-worlds.org";
        public string ServiceName => "FantasyWorlds";

        public Task<FantasyWorldsBookModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}