using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://newstudio.tv/
    /// </summary>
    public class NewStudioExternalProvider : IExternalServiceProvider<NewStudioTvShowModel>
    {
        public string ProviderUri => "http://newstudio.tv/";
        public Task<NewStudioTvShowModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}
