﻿using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// https://en.myshows.me/
    /// </summary>
    public class MyShowsExternalProvider : IExternalServiceProvider<MyShowsTvShowModel>
    {
        public string ServiceUri => "https://en.myshows.me/";
        public string ServiceName => "MyShowsMe";

        public Task<MyShowsTvShowModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}
