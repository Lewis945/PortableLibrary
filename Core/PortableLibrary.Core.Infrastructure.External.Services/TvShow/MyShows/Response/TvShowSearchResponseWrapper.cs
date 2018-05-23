using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response
{
    public class TvShowSearchResponseWrapper : BaseResponse
    {
        public IEnumerable<TvShowResponse> Result { get; set; }
    }
}