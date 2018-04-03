using System;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book
{
    /// <summary>
    /// https://www.e-reading.club
    /// </summary>
    public class EReadingExternalProvider : IExternalServiceProvider<EReadingBookModel>
    {
        public string ServiceUri => "https://www.e-reading.club";
        public string ServiceName => "EReading";

        public Task<EReadingBookModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}