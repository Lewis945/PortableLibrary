using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.ColdFilm
{
    /// <summary>
    /// http://coldfilm.info/
    /// </summary>
    public class ColdFilmExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "http://coldfilm.cc/";
        public override string ServiceName => "ColdFilm";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        private readonly Regex _titleRegex =
            new Regex(
                @"^(?<title>[\w\s\d_+'!@-]+)\s(?<seasonIndex>\d+)\sсезон\s(?<episodeIndex>\d+)(-(?<episodeEndIndex>\d+))?\sсерия.*$",
                RegexOptions.Compiled);

        #endregion

        #region .ctor

        public ColdFilmExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        #region Public Methods

        public async Task<ColdFilmTvShowModel> FindAndExtractTvShowAsync(string tvShowName)
        {
            var model = new ColdFilmTvShowModel {Title = tvShowName};

            string uri = ServiceUri.AppendUriPath("search").AppendUriPath($"?q={tvShowName}");

            var container = await GetSearchContainerNodeAsync(uri);
            var pagesUrls = ExtractSearchOtherPagesUrls(container, uri);

            var seasons = new Dictionary<int, string>();
            await ParseSearchPageAsync(container, tvShowName, seasons);

            foreach (var pageUrl in pagesUrls)
            {
                container = await GetSearchContainerNodeAsync(pageUrl);
                await ParseSearchPageAsync(container, tvShowName, seasons);
            }

            foreach (var season in seasons.OrderBy(s => s.Key))
            {
                var seasonObject = await ExtractTvShowSeasonAsync(season.Value);
                if (model.Seasons == null)
                    model.Seasons = new List<ColdFilmTvShowSeasonModel>();
                model.Seasons.Add(seasonObject);
            }

            if (model.Seasons == null)
                return null;

            model.Seasons = model.Seasons.OrderBy(s => s.Index).ToList();
            model.Seasons.ForEach(s => s.Episodes = s.Episodes.OrderBy(e => e.Index).ToList());

            return model;
        }

        public async Task<ColdFilmTvShowSeasonModel> ExtractTvShowSeasonAsync(string uri)
        {
            var model = new ColdFilmTvShowSeasonModel();

            var container = await GetSeasonContainerNodeAsync(uri);

            int seasonIndex = 0;
            model.Episodes = ParseSeasonPage(container, model.Episodes, ref seasonIndex);

            var pagesUrls = ExtractSeasonsOtherPagesUrls(container);

            foreach (var pageUrl in pagesUrls)
            {
                container = await GetSeasonContainerNodeAsync(pageUrl);
                model.Episodes = ParseSeasonPage(container, model.Episodes, ref seasonIndex);
            }

            if (model.Episodes == null || !model.Episodes.Any())
                return null;

            model.Index = seasonIndex;
            model.Episodes = model.Episodes.OrderBy(s => s.Index).ToList();

            return model;
        }

        #endregion

        #region Private Methods

        private async Task<HtmlNode> GetSearchContainerNodeAsync(string uri)
        {
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, Encoding.UTF8));
            var divMainItems = document.DocumentNode.SelectNodes("//div")
                ?.FirstOrDefault(n => n.HasClass("main-items"));
            return divMainItems;
        }

        private async Task<HtmlNode> GetSeasonContainerNodeAsync(string uri)
        {
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, Encoding.UTF8));
            var divAllEntries = document.DocumentNode.SelectSingleNode("//div[@id='allEntries']");
            return divAllEntries;
        }

        private static IEnumerable<string> ExtractSearchOtherPagesUrls(HtmlNode container, string uri)
        {
            var divPagination = container?.SelectNodes(".//div")?.FirstOrDefault(n => n.HasClass("pagi-nav"));

            var aLinks = divPagination?.SelectNodes(".//a")?.Where(a =>
                !string.IsNullOrWhiteSpace(a.InnerText.ClearString().ExtractNumberSubstring())).ToList();

            if (aLinks == null)
                yield break;

            int? firstPage = aLinks.FirstOrDefault()?.InnerText.ClearString().ParseNumber();
            int? lastPage = aLinks.LastOrDefault()?.InnerText.ClearString().ParseNumber();

            if (!firstPage.HasValue || !lastPage.HasValue)
                yield break;

            if (firstPage.Value == lastPage.Value)
                yield return $"{uri};p={firstPage.Value}";

            int count = lastPage.Value + 1 - firstPage.Value;
            
            foreach (var pageNumber in Enumerable.Range(firstPage.Value, count))
                yield return $"{uri};p={pageNumber}";
        }

        private IEnumerable<string> ExtractSeasonsOtherPagesUrls(HtmlNode container)
        {
            var divPagination = container?.SelectSingleNode("//div[@id='pagesBlock1']");

            var nodes = divPagination?.SelectNodes(".//a")?.ToList();
            var aLinks = nodes?.Where(a =>
                !string.IsNullOrWhiteSpace(a.InnerText.ClearString().ExtractNumberSubstring()));

            if (aLinks == null)
                yield break;

            foreach (var aLink in aLinks)
            {
                var value = aLink.Attributes["href"].Value;
                value = ServiceUri.AppendUriPath(value);
                value = HttpUtility.HtmlDecode(value);
                yield return value;
            }
        }

        private List<ColdFilmTvShowEpisodeModel> ParseSeasonPage(HtmlNode container,
            List<ColdFilmTvShowEpisodeModel> episodes, ref int seasonIndex)
        {
            var divsEntries = container?.SelectNodes(".//div[starts-with(@id, 'entryID')]");

            if (divsEntries == null)
                return null;

            foreach (var divEntry in divsEntries)
            {
                var divs = divEntry.SelectNodes(".//div")?.ToList();

                var divDescription = divs?.FirstOrDefault(d => d.HasClass("kino-desc"));
                var spanDescription = divDescription?.SelectSingleNode(".//div")?.SelectSingleNode(".//span");
                var description = spanDescription?.InnerText.ClearString();
                if (description != null)
                {
                    var isTrailer = description.Contains("[Трейлер]");
                    if (isTrailer)
                        continue;
                }

                var divTitle = divs?.FirstOrDefault(d => d.HasClass("kino-title"));
                var aTitle = divTitle?.SelectSingleNode(".//a");
                string title = aTitle?.InnerText.ClearString();

                int episodeIndex = 0;
                int? episodeEndIndex = 0;

                if (title != null)
                {
                    var match = _titleRegex.Match(title);

                    title = match.Groups["title"]?.Value.ClearString();
                    string seasonIndexString = match.Groups["seasonIndex"]?.Value.ClearString();
                    string episodeIndexString = match.Groups["episodeIndex"]?.Value.ClearString();
                    string episodeEndIndexString = match.Groups["episodeEndIndex"]?.Value.ClearString();

                    int.TryParse(seasonIndexString, out seasonIndex);
                    int.TryParse(episodeIndexString, out episodeIndex);
                    episodeEndIndex = episodeEndIndexString?.ParseNumber();
                }

                var divDate = divs?.FirstOrDefault(d => d.HasClass("kino-date"));
                string dateString = divDate?.InnerText.ClearString();
                DateTime.TryParseExact(
                    dateString,
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date);

                var divImage = divs?.FirstOrDefault(d => d.HasClass("kino-img"));
                var img = divImage?.SelectSingleNode(".//img");
                string imgSrc = img?.Attributes["src"]?.Value.ClearString();
                string imageUri = !string.IsNullOrWhiteSpace(imgSrc) ? ServiceUri.AppendUriPath(imgSrc) : null;

                if (episodes == null)
                    episodes = new List<ColdFilmTvShowEpisodeModel>();

                var episode = new ColdFilmTvShowEpisodeModel
                {
                    Index = episodeIndex,
                    ImageUri = imageUri,
                    Title = title,
                    DateReleased = date
                };
                episodes.Add(episode);

                if (episodeEndIndex.HasValue)
                {
                    episode = new ColdFilmTvShowEpisodeModel
                    {
                        Index = episodeEndIndex.Value,
                        ImageUri = imageUri,
                        Title = title,
                        DateReleased = date
                    };
                    episodes.Add(episode);
                }
            }

            return episodes;
        }

        private async Task ParseSearchPageAsync(HtmlNode container, string tvShowName,
            Dictionary<int, string> seasons)
        {
            var aItems = container?.SelectNodes("//a")?.Where(n => n.HasClass("sres-wrap"));

            if (aItems == null)
                return;

            foreach (var aItem in aItems)
            {
                var link = aItem.Attributes["href"]?.Value;
                var divText = aItem.SelectNodes(".//div")?.FirstOrDefault(n => n.HasClass("sres-text"));
                var h2Title = divText?.SelectSingleNode(".//h2");
                string h2Text = h2Title?.InnerText.ClearString();

                if (string.IsNullOrWhiteSpace(h2Text))
                    continue;

                var match = _titleRegex.Match(h2Text);

                string title = match.Groups["title"]?.Value.ClearString();

                if (!string.Equals(title, tvShowName, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                string seasonIndexString = match.Groups["seasonIndex"]?.Value.ClearString();
                string episodeIndexString = match.Groups["episodeIndex"]?.Value.ClearString();

                if (!int.TryParse(seasonIndexString, out var seasonIndex) ||
                    !int.TryParse(episodeIndexString, out var episodeIndex))
                    continue;

                if (seasons.ContainsKey(seasonIndex))
                    continue;

                var seasonLink = await ExtractSeasonLinkAsync(link, tvShowName);
                if (string.IsNullOrWhiteSpace(seasonLink))
                    continue;

                seasons[seasonIndex] = seasonLink;
            }
        }

        private async Task<string> ExtractSeasonLinkAsync(string episodeLink, string tvShowName)
        {
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(episodeLink, Encoding.UTF8));
            var divSpeedbar = document.DocumentNode.SelectNodes("//div")
                ?.FirstOrDefault(n => n.HasClass("speedbar"));

            var aSeasonItem = divSpeedbar?.SelectNodes(".//a")?.LastOrDefault();

            string seasonTitle = aSeasonItem?.InnerText;
            if (string.IsNullOrWhiteSpace(seasonTitle))
                return null;

            if (!seasonTitle.StartsWith(tvShowName))
                return null;

            string href = aSeasonItem.Attributes["href"]?.Value;
            return string.IsNullOrWhiteSpace(href) ? null : ServiceUri.AppendUriPath(href);
        }

        #endregion
    }
}