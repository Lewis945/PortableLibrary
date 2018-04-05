using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// https://www.lostfilm.tv
    /// </summary>
    public class LostFilmExternalProvider : BaseExternalProvider, IExternalServiceProvider<LostFilmTvShowModel>
    {
        #region Properties

        public override string ServiceUri => "https://www.lostfilm.tv";
        public override string ServiceName => "LostFilm";

        #endregion

        #region IExternalServiceProvider

        public Task<LostFilmTvShowModel> Extract()
        {
            throw new NotImplementedException();
        }

        public async Task<LostFilmTvShowModel> Extract(string uri)
        {
            var model = new LostFilmTvShowModel();

            var web = new HtmlWeb();
            var document = web.Load(uri);

            //first title-block
            var divFirstTitleBlock = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("title-block"));

            #region Extract Titles

            var titles = ExtractTitles(divFirstTitleBlock);

            model.Title = titles.Title;
            model.OriginalTitle = titles.OriginalTitle;

            #endregion

            #region Extract Status

            model.IsComplete = ExtractStatus(divFirstTitleBlock);

            #endregion

            //second title-block
            var divSecondTitleBlock = document.DocumentNode.SelectNodes(".//div")?
                .Where(n => n.HasClass("title-block")).Skip(1).FirstOrDefault();

            #region Extract Image

            model.ImageUri = ExtractImage(divSecondTitleBlock);
            model.ImageByteArray = await GetImageAsByteArray(model.ImageUri);

            #endregion

            #region Extract Genres

            model.Genres = ExtractGenres(divSecondTitleBlock);

            #endregion

            #region Extract Description

            model.Description = ExtractDescription(document);

            #endregion

            #region Extract Seasons

            model.Seasons = ExtractSeasons(web, uri);

            #endregion

            return model;
        }

        #endregion

        #region Private Methods

        private (string Title, string OriginalTitle) ExtractTitles(HtmlNode divFirstTitleBlock)
        {
            var divTitleRu = divFirstTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("title-ru"));

            string title = HttpUtility.HtmlDecode(divTitleRu?.InnerText.Trim());

            var divTitleEn = divFirstTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("title-en"));

            string originalTitle = HttpUtility.HtmlDecode(divTitleEn?.InnerText.Trim());

            return (title, originalTitle);
        }

        private bool? ExtractStatus(HtmlNode divFirstTitleBlock)
        {
            var divStatus = divFirstTitleBlock?.SelectNodes(".//div")?
               .FirstOrDefault(n => n.HasClass("status"));

            const string statusKey = "Статус:";

            string statusText = divStatus?.InnerText;
            statusText = statusText?.Replace(statusKey, string.Empty).Trim();
            statusText = HttpUtility.HtmlDecode(statusText);

            return statusText?.Equals("Завершен");
        }

        private string ExtractImage(HtmlNode divSecondTitleBlock)
        {
            var divMainPoster = divSecondTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("main_poster"));

            var imageUriBackgroundStyle = divMainPoster?.Attributes["style"].Value;

            if (string.IsNullOrWhiteSpace(imageUriBackgroundStyle))
                return null;

            var match = Regex.Match(imageUriBackgroundStyle, @"background:url\('(?<uri>.*)'\)");
            var imageUri = HttpUtility.HtmlDecode(match.Groups["uri"].Value);
            if (imageUri.StartsWith("//"))
                imageUri = imageUri.Substring(2);

            return imageUri;
        }

        private List<string> ExtractGenres(HtmlNode divSecondTitleBlock)
        {
            var divDetailsPane = divSecondTitleBlock?.SelectNodes(".//div")?
                 .FirstOrDefault(n => n.HasClass("details-pane"));

            const string genreKey = "Жанр:";

            var divGenres = divDetailsPane?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.Descendants().First().InnerText.Contains(genreKey));

            if (divGenres == null)
                return null;

            bool addDescendants = false;
            var descendants = divGenres.Descendants().ToList();
            var filteredDescendant = new List<HtmlNode>();
            foreach (var descendant in descendants)
            {
                if (addDescendants)
                {
                    var innerText = descendant.InnerText.ToLowerInvariant().Trim();
                    var match = Regex.Match(innerText, @"^\w+:{1}$");
                    if (descendant.NodeType == HtmlNodeType.Text && match.Success)
                        break;

                    filteredDescendant.Add(descendant);
                }

                if (descendant.NodeType == HtmlNodeType.Text && descendant.InnerText.Trim().Equals(genreKey))
                    addDescendants = true;
            }

            var aNodes = divGenres.SelectNodes(".//a");
            var genresNodes = aNodes?.Where(n => filteredDescendant.Contains(n)).ToList();
            var genres = genresNodes?.Select(n => HttpUtility.HtmlDecode(n.InnerText.Trim())).ToList();
            return genres;
        }

        private string ExtractDescription(HtmlDocument document)
        {
            var divDescriptionTextBlock = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("text-block") && n.HasClass("description"));

            if (divDescriptionTextBlock == null)
                return null;

            var divBody = divDescriptionTextBlock.SelectNodes("./div")?
                .FirstOrDefault(n => n.HasClass("body"));

            var divInnerBody = divBody?.SelectNodes("./div")?
                .FirstOrDefault(n => n.HasClass("body"));

            if (divInnerBody == null)
                return null;

            return HttpUtility.HtmlDecode(divInnerBody.InnerText.Trim());
        }

        private List<LostFilmTvShowSeasonModel> ExtractSeasons(HtmlWeb web, string uri)
        {
            var document = web.Load($"{(uri.EndsWith("/") ? uri.Substring(0, uri.Length - 1) : uri)}/seasons");

            var divSeriesBlock = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("series-block"));

            var divSeriesBlocks = divSeriesBlock?.SelectNodes(".//div")?
                .Where(n => n.HasClass("serie-block")).ToList();

            if (divSeriesBlocks == null || !divSeriesBlocks.Any())
                return null;

            var seasons = new List<LostFilmTvShowSeasonModel>();
            foreach (var divSerieBlock in divSeriesBlocks)
            {
                var season = ExtractSeason(divSerieBlock);
                if (season == null)
                    continue;
                seasons.Add(season);
            }

            return seasons;
        }

        private LostFilmTvShowSeasonModel ExtractSeason(HtmlNode divSerieBlock)
        {
            var season = new LostFilmTvShowSeasonModel();

            var h2Title = divSerieBlock.SelectSingleNode("./h2");
            if (h2Title != null)
            {
                string title = h2Title.InnerText.Trim();
                season.Title = HttpUtility.HtmlDecode(title);

                string indexString = Regex.Match(title, @"\d+").Value;
                int.TryParse(indexString, out var index);
                season.Index = index;
            }

            var divMovieDetailsBlock = divSerieBlock.SelectNodes("./div")?
                .FirstOrDefault(n => n.HasClass("movie-details-block"));

            var divBody = divMovieDetailsBlock?.SelectNodes("./div")?
                .FirstOrDefault(n => n.HasClass("body"));

            var divDetails = divBody?.SelectNodes("./div")?
                .FirstOrDefault(n => n.HasClass("details"));

            var spanEpisodes = divDetails?.SelectSingleNode("./span");

            if (spanEpisodes != null)
            {
                string episodeCountText = spanEpisodes.InnerText.Trim();
                string episodeCountString = Regex.Match(episodeCountText, @"\d+").Value;
                int.TryParse(episodeCountString, out var episodeCount);
                season.TotalEpisodesCount = episodeCount;
            }

            #region Extract Episodes

            var tableEpisodes = divSerieBlock.SelectSingleNode("./table");

            var trEpisodes = tableEpisodes?.SelectNodes(".//tr");

            if (trEpisodes != null && trEpisodes.Count > 0)
            {
                season.Episodes = new List<LostFilmTvShowEpisodeModel>();

                foreach (var trEpisode in trEpisodes)
                {
                    var episode = ExtractEpisode(trEpisode);
                    if (episode == null)
                        continue;
                    season.Episodes.Add(episode);
                }
            }

            #endregion

            return season;
        }

        private LostFilmTvShowEpisodeModel ExtractEpisode(HtmlNode trEpisode)
        {
            var tdsEpisode = trEpisode.SelectNodes("./td");

            if (tdsEpisode == null || !tdsEpisode.Any())
                return null;

            var episode = new LostFilmTvShowEpisodeModel();

            var tdSeason = tdsEpisode.FirstOrDefault(n => n.HasClass("beta"));
            if (tdSeason != null)
            {
                string text = tdSeason.InnerText.Trim();

                var match = Regex.Matches(text, @"\d+");
                string episodeIndexString = match.Last()?.Value;
                int.TryParse(episodeIndexString, out var episodeIndex);

                episode.Index = episodeIndex;
            }

            var tdTitles = tdsEpisode.FirstOrDefault(n => n.HasClass("gamma"));
            if (tdTitles != null)
            {
                string text = tdTitles.InnerText.Trim();

                var spanEnglishTitle =
                    tdTitles.SelectSingleNode("./div")?.SelectSingleNode("./span");
                episode.OriginalTitle = HttpUtility.HtmlDecode(spanEnglishTitle?.InnerText.Trim());

                string title = text.Replace(episode.OriginalTitle, string.Empty).Trim();
                episode.Title = HttpUtility.HtmlDecode(title);
            }

            var tdDates = tdsEpisode.FirstOrDefault(n => n.HasClass("delta"));
            if (tdDates != null)
            {
                string text = tdDates.InnerText.Trim();

                var spanEnglishDate = tdDates.SelectSingleNode("./span");
                var dateOriginalReleasedString =
                    HttpUtility.HtmlDecode(spanEnglishDate?.InnerText.Trim());
                dateOriginalReleasedString =
                    Regex.Match(dateOriginalReleasedString, @"[\d\.]+").Value;

                DateTime.TryParseExact(dateOriginalReleasedString,
                    "d.M.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var dateOriginalReleased);

                episode.DateOriginalReleased = dateOriginalReleased;

                string dateReleasedString =
                    text.Replace(dateOriginalReleasedString, string.Empty).Trim();
                dateReleasedString = Regex.Match(dateReleasedString, @"[\d\.]+").Value;

                DateTime.TryParseExact(dateReleasedString,
                    "d.M.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var dateReleased);

                episode.DateReleased = dateReleased;
            }

            return episode;
        }

        #endregion
    }
}