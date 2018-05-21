using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Request;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows
{
    /// <summary>
    /// https://myshows.me/
    /// </summary>
    public class MyShowsExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "https://api.myshows.me/v2/rpc/";
        public override string ServiceName => "MyShowsMe";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;
        private readonly IHttpService _httpService;
        private readonly IMapper _mapper;
        private readonly string _language;

        #endregion

        #region .ctor

        public MyShowsExternalProvider(IHttpService httpService, IRetryService retryService, IMapper mapper,
            Language language)
        {
            _retryService = retryService;
            _httpService = httpService;
            _mapper = mapper;
            _language = GetLanguage(language);
        }

        #endregion

        #region Public Methods

        public async Task<MyShowsTvShowModel> ExtractTvShowByIdAsync(int id)
        {
            var request = new GetTvShowByIdRequest
            {
                Params = new GetTvShowByIdRequest.RequestParams
                {
                    ShowId = id,
                    WithEpisodes = true
                }
            };

            var response = await _retryService.ExecuteAsync(() =>
                _httpService.PostAsync<GetTvShowByIdRequest, TvShowResponse>(
                    ServiceUri, request, _language)).ConfigureAwait(false);

            var model = _mapper.Map<MyShowsTvShowModel>(response);

            return model;
        }

        public async Task<MyShowsTvShowModel> ExtractTvShowByUriAsync(string uri)
        {
            if (uri.EndsWith("/")) uri = uri.Remove(uri.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase));
            string idString = uri.Substring(uri.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);

            if (!int.TryParse(idString, out int id))
                throw new Exception("");

            return await ExtractTvShowByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<List<MyShowsTvShowModel>> GetTvShowsByTitleAsync(string title)
        {
            var request = new GetTvShowByTitleRequest
            {
                Params = new GetTvShowByTitleRequest.RequestParams
                {
                    Query = title
                }
            };

            var response = await _retryService.ExecuteAsync(() =>
                _httpService.PostAsync<GetTvShowByTitleRequest, TvShowSearchResponse>(
                    ServiceUri, request, _language)).ConfigureAwait(false);

            var model = _mapper.Map<List<MyShowsTvShowModel>>(response.Result);

            return model;
        }

        #endregion

        #region  Private Methods

        private string GetLanguage(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "en";
                case Language.Russian:
                    return "ru";
                case Language.Ukrainian:
                    return "ua";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

        #endregion
    }
}