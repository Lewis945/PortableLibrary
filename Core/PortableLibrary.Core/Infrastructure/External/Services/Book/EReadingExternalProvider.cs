using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book
{
    /// <summary>
    /// https://www.e-reading.club
    /// </summary>
    public class EReadingExternalProvider : IExternalServiceProvider<EReadingBookModel>
    {
        public string ProviderUri => "https://www.e-reading.club";

        public Task<EReadingBookModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}
