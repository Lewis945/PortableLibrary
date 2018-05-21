using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response
{
    public class TvShowSearchResponse : BaseResponse
    {
        public IEnumerable<ResultResponse> Result { get; set; }
    }
}