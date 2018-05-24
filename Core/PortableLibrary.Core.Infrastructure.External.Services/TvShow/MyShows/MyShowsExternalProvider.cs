using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PortableLibrary.Core.External.Services.TvShow;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using PortableLibrary.Core.External.Services.TvShow.Models.Search;
using PortableLibrary.Core.External.Services.TvShow.Models.Tracking;
using PortableLibrary.Core.Http;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Request;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows
{
    /// <summary>
    /// https://myshows.me/
    /// </summary>
    public class MyShowsExternalProvider : BaseExternalProvider,
        ITvShowDataExtractionProvider, ITvShowTrackingProvider, ITvShowSearchProvider
    {
        #region Properties

        public override string ServiceUri => "https://api.myshows.me/v2/rpc/";
        public override string ServiceName => "MyShows";

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

        #region ITvShowDataExtractionProvider, ITvShowTrackingProvider, ITvShowSearchProvider

        public async Task<TvShowDataExtractionModel> ExtractTvShowAsync(string uri)
        {
            return await ExtractTvShowAsync(GetTvShowId(uri)).ConfigureAwait(false);
        }

        public async Task<TvShowTrackingModel> TrackTvShowAsync(string uri)
        {
            return await TrackTvShowAsync(GetTvShowId(uri)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TvShowSearchModel>> FindTvShowAsync(string title)
        {
            var request = new GetTvShowByTitleRequest
            {
                Params = new GetTvShowByTitleRequest.RequestParams
                {
                    Query = title
                }
            };

            var response = await _retryService.ExecuteAsync(() =>
                _httpService.PostAsync<GetTvShowByTitleRequest, TvShowSearchResponseWrapper>(
                    ServiceUri, request, _language)).ConfigureAwait(false);

            var model = _mapper.Map<IEnumerable<TvShowSearchModel>>(response.Result);

            return model;
        }

        #endregion

        #region Public Methods

        internal async Task<TvShowDataExtractionModel> ExtractTvShowAsync(int id)
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
                _httpService.PostAsync<GetTvShowByIdRequest, TvShowResponseWrapper>(
                    ServiceUri, request, _language)).ConfigureAwait(false);

            var genresResponse = await GetTvShowGenresAsync().ConfigureAwait(false);
            var genreIds = response.Result.GenreIds.OrderBy(g => g).ToList();
            var genres = genresResponse.OrderBy(g => g.Id).Where(g => genreIds.Contains(g.Id))
                .Select(g => g.Title);

            var model = _mapper.Map<TvShowDataExtractionModel>(response);
            model.Genres = genres.ToList();

            return model;
        }

        internal async Task<TvShowTrackingModel> TrackTvShowAsync(int id)
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
                _httpService.PostAsync<GetTvShowByIdRequest, TvShowResponseWrapper>(
                    ServiceUri, request, _language)).ConfigureAwait(false);

            var model = _mapper.Map<TvShowTrackingModel>(response);

            return model;
        }

        #endregion

        #region  Private Methods

        private async Task<IEnumerable<GenreResponse>> GetTvShowGenresAsync()
        {
            var request = new GetTvShowGenresRequest();

            var response = await _retryService.ExecuteAsync(() =>
                _httpService.PostAsync<GetTvShowGenresRequest, GenresResponseWrapper>(
                    ServiceUri, request, _language)).ConfigureAwait(false);

            return response.Result;
        }

        private int GetTvShowId(string uri)
        {
            if (uri.EndsWith("/")) uri = uri.Remove(uri.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase));
            string idString = uri.Substring(uri.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);

            if (!int.TryParse(idString, out int id))
                throw new Exception($"Id {idString} can't be parsed to an integer.");

            return id;
        }

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