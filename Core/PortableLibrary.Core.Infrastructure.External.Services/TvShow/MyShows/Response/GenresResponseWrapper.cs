using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response
{
    public class GenresResponseWrapper : BaseResponse
    {
        public IEnumerable<GenreResponse> Result { get; set; }
    }
}