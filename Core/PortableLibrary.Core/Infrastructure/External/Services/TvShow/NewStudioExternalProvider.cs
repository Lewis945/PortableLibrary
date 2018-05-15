using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        #endregion

        #region .ctor

        public NewStudioExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        public async Task<NewStudioTvShowModel> ExtractTvShow(string uri)
        {
            var model = new NewStudioTvShowModel();

            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, Encoding.UTF8));

            var divAccordionInner = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("accordion-inner"));

            var divsTopic = divAccordionInner?.SelectNodes(".//div")?.Where(n => n.HasClass("topic_id"));

            if (divsTopic == null)
                return model;

            var titleRegex =
                new Regex(
                    @"^(?<title>[\w\s\d_+!@-]+)\s\((?<season>[\w\s\d]+),(?<episode>[\w\s\d]+)\)\s/\s(?<engtitle>[\w\s\d_+!@-]+)\s.*\s((?<quality>\d{3,4}p)|.*)$");

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
                    var match = titleRegex.Match(text);

                    var seasonString = match.Groups["season"]?.Value;
                    seasonString = seasonString?.ExtractNumberSubstring();
                    if (int.TryParse(seasonString, out var seasonIndex))
                    {
                        season = model.Seasons?.FirstOrDefault(s => s.Index == seasonIndex);
                        if (season == null)
                        {
                            if (model.Seasons == null)
                                model.Seasons = new List<NewStudioTvShowSeasonModel>();

                            season = new NewStudioTvShowSeasonModel {Index = seasonIndex};
                            model.Seasons.Add(season);
                        }
                    }

                    var episodeString = match.Groups["episode"]?.Value;
                    episodeString = episodeString?.ExtractNumberSubstring();
                    if (season != null && int.TryParse(episodeString, out var episodeIndex))
                    {
                        if (season.Episodes == null)
                            season.Episodes = new List<NewStudioTvShowEpisodeModel>();

                        var title = match.Groups["title"]?.Value;
                        var originalTitle = match.Groups["engtitle"]?.Value;
                        var quality = match.Groups["quality"]?.Value;

                        episode = season.Episodes.FirstOrDefault(e => e.Index == episodeIndex);

                        if (episode != null && (string.IsNullOrWhiteSpace(episode.Quality) ||
                                                (episode.Quality == "720p" && quality == "1080p")))
                        {
                            season.Episodes.Remove(episode);
                            episode = null;
                        }

                        if (episode == null)
                        {
                            episode = new NewStudioTvShowEpisodeModel
                            {
                                Index = episodeIndex,
                                TvShowTitle = title,
                                OriginalTvShowSeasonTitle = originalTitle,
                                Quality = quality
                            };
                            season.Episodes.Add(episode);
                        }
                    }
                }

                var divLastPost = divTopic.SelectNodes(".//div")?.FirstOrDefault(n => n.HasClass("lastpostt"));
                if (divLastPost != null)
                {
                    //2013-11-25 16:11
                    var text = divLastPost.InnerText.ClearString();
                    if (episode != null && DateTime.TryParse(text, out var date))
                    {
                        episode.DateReleased = date;
                    }
                }
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
    }
}