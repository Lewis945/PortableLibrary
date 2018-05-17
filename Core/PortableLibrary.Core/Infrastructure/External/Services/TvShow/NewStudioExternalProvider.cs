using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.External.Models.TvShow;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://newstudio.tv/
    /// </summary>
    public class NewStudioExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "http://newstudio.tv/";
        public override string ServiceName => "NewStudio";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        private static readonly List<string> QualityList = new List<string>
        {
            "WEBDLRip",
            "WEBDL 720p",
            "WEBDL 1080p",
            "HDTVRip",
            "BDRip",
            "BDRip 720p",
            "BDRip 1080p"
        };

        #endregion

        #region .ctor

        public NewStudioExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        #region Public Methods

        public async Task<NewStudioTvShowModel> ExtractTvShowAsync(string uri)
        {
            var model = new NewStudioTvShowModel();

            var container = await GetContainerNodeAsync(uri);

            model.Seasons = ParsePage(container, model.Seasons);

            var pagesUrls = ExtractOtherPagesUrls(container);

            foreach (var pageUrl in pagesUrls)
            {
                container = await GetContainerNodeAsync(pageUrl);
                model.Seasons = ParsePage(container, model.Seasons);
            }

            if (model.Seasons == null)
                return null;

            model.Seasons = model.Seasons.OrderBy(s => s.Index).ToList();
            model.Seasons.ForEach(s => s.Episodes = s.Episodes.OrderBy(e => e.Index).ToList());

            var episodes = model.Seasons.SelectMany(s => s.Episodes).Where(e => e != null).ToList();
            if (!episodes.Any()) return model;

            var titles = episodes.Select(e => e.TvShowTitle);
            var mostOccuredTitle = titles.GroupBy(t => t).OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            model.Title = mostOccuredTitle;

            var originalTitles = episodes.Select(e => e.OriginalTvShowSeasonTitle);
            var mostOccuredOriginalTitle = originalTitles.GroupBy(t => t).OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            model.OriginalTitle = mostOccuredOriginalTitle;

            return model;
        }

        #endregion

        #region Private Methods

        private async Task<HtmlNode> GetContainerNodeAsync(string uri)
        {
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, Encoding.UTF8));
            var divAccordionInner = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("accordion-inner"));
            return divAccordionInner;
        }

        private IEnumerable<string> ExtractOtherPagesUrls(HtmlNode container)
        {
            var divPagination = container?.SelectNodes("//div")?.Where(n => n.HasClass("pagination"))
                .FirstOrDefault();

            var lis = divPagination?.SelectNodes(".//li");

            if (lis == null)
                yield break;

            foreach (var li in lis)
            {
                var aLinks = li.SelectNodes(".//a");

                var aLink = aLinks?.FirstOrDefault(a =>
                    !string.IsNullOrWhiteSpace(a.InnerText.ClearString().ExtractNumberSubstring()));

                if (aLink == null)
                    continue;

                var value = aLink.Attributes["href"].Value;
                value = ServiceUri.AppendUriPath(value);
                value = HttpUtility.HtmlDecode(value);
                yield return value;
            }
        }

        private static List<NewStudioTvShowSeasonModel> ParsePage(HtmlNode container,
            List<NewStudioTvShowSeasonModel> seasons)
        {
            var divsTopic = container?.SelectNodes(".//div")?.Where(n => n.HasClass("topic_id"));

            if (divsTopic == null)
                return null;

            var titleRegex =
                new Regex(
                    @"^(?<title>[\w\s\d_+'!@-]+)\s\((?<season>[\w\s\d]+),(?<episode>[\w\s\d]+)\)\s/\s(?<engtitle>[\w\s\d_+'!@-]+)\s.*\s((?<quality>[a-zA-Z]{3,}\s(\d{3,4}p)?).*|[\s.]*)$");

            foreach (var divTopic in divsTopic)
            {
                NewStudioTvShowSeasonModel season = null;
                NewStudioTvShowEpisodeModel episode = null;

                var aTopic = divTopic.SelectNodes(".//a")?.FirstOrDefault(n => n.HasClass("torTopic"));
                if (aTopic != null)
                {
                    //Шерлок Холмс (Сезон 1, Серия 1) / Sherlock (2010) BDRip | Первый
                    //Шерлок (Сезон 4) / Sherlock (2017) WEBDL 1080p
                    var text = aTopic.InnerText.ClearString();
                    text = HttpUtility.HtmlDecode(text);
                    if (string.IsNullOrWhiteSpace(text))
                        continue;

                    var match = titleRegex.Match(text);

                    var seasonString = match.Groups["season"]?.Value;
                    seasonString = seasonString?.ExtractNumberSubstring();
                    if (int.TryParse(seasonString, out var seasonIndex))
                    {
                        season = seasons?.FirstOrDefault(s => s.Index == seasonIndex);
                        if (season == null)
                        {
                            if (seasons == null)
                                seasons = new List<NewStudioTvShowSeasonModel>();

                            season = new NewStudioTvShowSeasonModel {Index = seasonIndex};
                            seasons.Add(season);
                        }
                    }

                    var episodeString = match.Groups["episode"]?.Value;
                    episodeString = episodeString?.ExtractNumberSubstring();
                    if (season != null && int.TryParse(episodeString, out var episodeIndex))
                    {
                        if (season.Episodes == null)
                            season.Episodes = new List<NewStudioTvShowEpisodeModel>();

                        var title = match.Groups["title"]?.Value.ClearString();
                        var originalTitle = match.Groups["engtitle"]?.Value.ClearString();
                        var quality = match.Groups["quality"]?.Value.ClearString();

                        episode = season.Episodes.FirstOrDefault(e => e.Index == episodeIndex);

                        if (episode != null)
                        {
                            var episode1 = episode;

                            int episodeQualityIndex = QualityList.FindIndex(s => s == episode1.Quality);
                            int qualityIndex = QualityList.FindIndex(s =>
                                s.Equals(quality, StringComparison.InvariantCultureIgnoreCase));

                            if (qualityIndex > episodeQualityIndex)
                            {
                                season.Episodes.Remove(episode);
                                episode = null;
                            }
                        }

                        if (episode == null)
                        {
                            var divLastPost = divTopic.SelectNodes(".//div")
                                ?.FirstOrDefault(n => n.HasClass("lastpostt"));
                            var dateText = divLastPost?.InnerText.ClearString();
                            if (!DateTime.TryParse(dateText, out var date))
                                continue;

                            episode = new NewStudioTvShowEpisodeModel
                            {
                                Index = episodeIndex,
                                TvShowTitle = title,
                                OriginalTvShowSeasonTitle = originalTitle,
                                Quality = quality,
                                DateReleased = date
                            };
                            season.Episodes.Add(episode);
                        }
                    }
                }
            }

            return seasons;
        }

        #endregion
    }
}