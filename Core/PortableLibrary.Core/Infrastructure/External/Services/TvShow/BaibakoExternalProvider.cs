﻿using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://baibako.tv/iplayer/
    /// </summary>
    public class BaibakoExternalProvider : IExternalServiceProvider<BaibakoTvShowModel>
    {
        public string ServiceUri => "http://baibako.tv/iplayer/";
        public string ServiceName => "BaibakoTv";

        public Task<BaibakoTvShowModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}
