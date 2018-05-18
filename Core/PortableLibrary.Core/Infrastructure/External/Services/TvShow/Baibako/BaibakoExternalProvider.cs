using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.Baibako
{
    /// <summary>
    /// http://baibako.tv/iplayer/
    /// </summary>
    public class BaibakoExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "http://baibako.tv/iplayer/";
        public override string ServiceName => "BaibakoTv";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        private readonly Regex _titleRegex =
            new Regex(
                @"^(?<title>[\w\s\d_+'!@-]+)\sS(?<seasonIndex>\d+)\sEP(?<episodeIndex>\d+)(-(?<episodeEndIndex>\d+))?.*$",
                RegexOptions.Compiled);

        #endregion

        #region .ctor

        public BaibakoExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        #region Public Methods

        public async Task<BaibakoTvShowModel> ExtractTvShowAsync(string uri)
        {
            var model = new BaibakoTvShowModel();

            var container = await GetContainerNodeAsync(uri);

            var spanTitle = container?.SelectSingleNode("//h1")?.SelectSingleNode(".//em");

            model.Title = spanTitle?.InnerText.ClearString();

            model.Seasons = ParsePage(container, model.Seasons);

            var pagesUrls = await ExtractOtherPagesUrlsAsync(container, uri);
            if (pagesUrls == null)
                return model;

            foreach (var pageUrl in pagesUrls.Values)
            {
                container = await GetContainerNodeAsync(pageUrl);
                model.Seasons = ParsePage(container, model.Seasons);
            }

            if (model.Seasons == null)
                return null;

            model.Seasons = model.Seasons.OrderBy(s => s.Index).ToList();
            model.Seasons.ForEach(s => s.Episodes = s.Episodes.OrderBy(e => e.Index).ToList());

            return model;
        }

        #endregion

        #region Private Methods

        private async Task<HtmlNode> GetContainerNodeAsync(string uri)
        {
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, Encoding.UTF8));
            var divAccordionInner = document.DocumentNode.SelectSingleNode("//div[@id='content']");
            return divAccordionInner;
        }

        private async Task<Dictionary<int, string>> ExtractOtherPagesUrlsAsync(HtmlNode container, string tvShowUri)
        {
            var result = new Dictionary<int, string>();

            var divPagination = container?.SelectNodes("//div")?.FirstOrDefault(n => n.HasClass("loop-nav"));

            var aLinks = divPagination?.SelectNodes(".//a");
//                ?.Where(n => !string.IsNullOrWhiteSpace(n.InnerText.ExtractNumberSubstring()));

            if (aLinks == null)
                return null;

            var aLinkLast = aLinks.FirstOrDefault(n => n.HasClass("last"));
            var href = aLinkLast?.Attributes["href"]?.Value;

            var pages = aLinks.Select(n => n.InnerText.ParseNumber()).Where(p => p.HasValue).Select(p => p.Value)
                .ToList();

            if (!string.IsNullOrWhiteSpace(href))
            {
                var lastContainer = await GetContainerNodeAsync(href);
                var lastItems = await ExtractOtherPagesUrlsAsync(lastContainer, tvShowUri);

                var lastItemUri = lastItems?.LastOrDefault();

                if (!pages.Any() || lastItemUri == null)
                    return null;

                var firstItem = pages.FirstOrDefault();
                var count = lastItemUri.Value.Key;

                foreach (var pageNumber in Enumerable.Range(firstItem, count))
                    result.Add(pageNumber, tvShowUri.AppendUriPath("page").AppendUriPath(pageNumber.ToString()));
            }
            else
            {
                foreach (var pageNumber in pages.Distinct())
                {
                    result.Add(pageNumber, tvShowUri.AppendUriPath("page").AppendUriPath(pageNumber.ToString()));
                }
            }

            return result;
        }

        private List<BaibakoTvShowSeasonModel> ParsePage(HtmlNode container,
            List<BaibakoTvShowSeasonModel> seasons)
        {
            var divsPosts = container?.SelectNodes(".//div")?.FirstOrDefault(n => n.HasClass("loop-content"))
                ?.SelectNodes("//div[starts-with(@id, 'post-')]");

            if (divsPosts == null)
                return null;

            foreach (var divPost in divsPosts)
            {
                BaibakoTvShowSeasonModel season = null;

                var h2Title = divPost.SelectNodes(".//div")?.FirstOrDefault(n => n.HasClass("data"))
                    ?.SelectNodes(".//h2")?.FirstOrDefault(n => n.HasClass("entry-title"));
                if (h2Title != null)
                {
                    var text = h2Title.InnerText.ClearString();
                    text = HttpUtility.HtmlDecode(text);
                    if (string.IsNullOrWhiteSpace(text))
                        continue;

                    var match = _titleRegex.Match(text);

                    string seasonString = match.Groups["seasonIndex"]?.Value;
                    int? seasonIndex = seasonString?.ParseNumber();
                    if (seasonIndex.HasValue)
                    {
                        season = seasons?.FirstOrDefault(s => s.Index == seasonIndex);
                        if (season == null)
                        {
                            if (seasons == null)
                                seasons = new List<BaibakoTvShowSeasonModel>();

                            season = new BaibakoTvShowSeasonModel {Index = seasonIndex.Value};
                            seasons.Add(season);
                        }
                    }

                    string episodeEndIndexString = match.Groups["episodeEndIndex"]?.Value.ClearString();
                    string episodeString = match.Groups["episodeIndex"]?.Value.ClearString();
                    int? episodeEndIndex = episodeEndIndexString?.ParseNumber();
                    int? episodeIndex = episodeString?.ParseNumber();
                    if (season != null && episodeIndex.HasValue && episodeIndex.Value != 0)
                    {
                        if (season.Episodes == null)
                            season.Episodes = new List<BaibakoTvShowEpisodeModel>();

                        var title = match.Groups["title"]?.Value.ClearString();

                        var episode = season.Episodes.FirstOrDefault(e => e.Index == episodeIndex);
                        if (episode == null)
                        {
                            var pMeta = divPost.SelectSingleNode(".//div[@class='data']")
                                ?.SelectSingleNode(".//p[@class='entry-meta']");
                            var timeEntryDate = pMeta?.SelectSingleNode(".//time");
                            var dateText = timeEntryDate?.Attributes["datetime"]?.Value.ClearString();

                            if (!DateTime.TryParse(
                                dateText,
                                out var date))
                                continue;

                            episode = new BaibakoTvShowEpisodeModel
                            {
                                Index = episodeIndex.Value,
                                Title = title,
                                DateReleased = date.ToUniversalTime().Date
                            };
                            season.Episodes.Add(episode);

                            if (episodeEndIndex.HasValue)
                            {
                                episode = new BaibakoTvShowEpisodeModel
                                {
                                    Index = episodeEndIndex.Value,
                                    Title = title,
                                    DateReleased = date.ToUniversalTime().Date
                                };
                                season.Episodes.Add(episode);
                            }
                        }
                    }
                }
            }

            return seasons;
        }

        #endregion
    }
}