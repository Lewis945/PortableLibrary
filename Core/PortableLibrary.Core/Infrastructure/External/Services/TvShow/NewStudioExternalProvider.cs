using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;
using PortableLibrary.Core.Infrastructure.External.Models.TvShow;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://newstudio.tv/
    /// </summary>
    public class NewStudioExternalProvider : IExternalServiceProvider<NewStudioTvShowModel>
    {
        public string ServiceUri => "http://newstudio.tv/";
        public string ServiceName => "NewStudio";

        public Task<NewStudioTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
