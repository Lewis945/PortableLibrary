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

            var divTitleRu = divFirstTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("title-ru"));

            model.Title = HttpUtility.HtmlDecode(divTitleRu?.InnerText.Trim());

            var divTitleEn = divFirstTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("title-en"));

            model.OriginalTitle = HttpUtility.HtmlDecode(divTitleEn?.InnerText.Trim());

            #endregion

            #region Extract Status

            var divStatus = divFirstTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("status"));

            const string statusKey = "Статус:";

            string statusText = divStatus?.InnerText;
            statusText = statusText?.Replace(statusKey, string.Empty).Trim();
            statusText = HttpUtility.HtmlDecode(statusText);
            
            model.IsComplete = statusText?.Equals("Завершен");

            #endregion

            //second title-block
            var divSecondTitleBlock = document.DocumentNode.SelectNodes(".//div")?
                .Where(n => n.HasClass("title-block")).Skip(1).FirstOrDefault();

            #region Extract Image

            var divMainPoster = divSecondTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("main_poster"));

            var imageUriBackgroundStyle = divMainPoster?.Attributes["style"].Value;

            if (!string.IsNullOrWhiteSpace(imageUriBackgroundStyle))
            {
                //background:url('//static.lostfilm.tv/Images/293/Posters/poster.jpg');
                var match = Regex.Match(imageUriBackgroundStyle, @"background:url\('(?<uri>.*)'\)");
                var imageUri = HttpUtility.HtmlDecode(match.Groups["uri"].Value);
                if (imageUri.StartsWith("//"))
                    imageUri = imageUri.Substring(2);
                model.ImageUri = imageUri;
                model.ImageByteArray = await GetImageAsByteArray(imageUri);
            }

            #endregion

            #region Extract Genres

            var divDetailsPane = divSecondTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("details-pane"));

            const string genreKey = "Жанр:";

            var divGenres = divDetailsPane?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.Descendants().First().InnerText.Contains(genreKey));

            if (divGenres != null)
            {
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
                    {
                        addDescendants = true;
                    }
                }

                var aNodes = divGenres.SelectNodes(".//a");
                var genresNodes = aNodes?.Where(n => filteredDescendant.Contains(n)).ToList();
                var genres = genresNodes?.Select(n => HttpUtility.HtmlDecode(n.InnerText.Trim())).ToList();
                model.Genres = genres;
            }

            #endregion

            #region Extract Description

            //text-block description
            var divDescriptionTextBlock = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("text-block") && n.HasClass("description"));

            if (divDescriptionTextBlock != null)
            {
                var divBody = divDescriptionTextBlock.SelectNodes("./div")
                    ?.FirstOrDefault(n => n.HasClass("body"));

                var divInnerBody = divBody?.SelectNodes("./div")
                    ?.FirstOrDefault(n => n.HasClass("body"));

                if (divInnerBody != null)
                {
                    model.Description = HttpUtility.HtmlDecode(divInnerBody.InnerText.Trim());
                }
            }

            #endregion

            #region Extract Seasons

            document = web.Load($"{(uri.EndsWith("/") ? uri.Substring(0, uri.Length - 1) : uri)}/seasons");

            var divSeriesBlock = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("series-block"));

            var divSeriesBlocks = divSeriesBlock?.SelectNodes(".//div")?
                .Where(n => n.HasClass("serie-block")).ToList();

            List<LostFilmTvShowSeasonModel> seasons = null;
            if (divSeriesBlocks != null && divSeriesBlocks.Any())
            {
                seasons = new List<LostFilmTvShowSeasonModel>();
                foreach (var divSerieBlock in divSeriesBlocks)
                {
                    var season = new LostFilmTvShowSeasonModel();
                    seasons.Add(season);

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
                            var tdsEpisode = trEpisode.SelectNodes("./td");
                            if (tdsEpisode != null && tdsEpisode.Count > 0)
                            {
                                var episode = new LostFilmTvShowEpisodeModel();

                                var tdSeason = tdsEpisode.FirstOrDefault(n => n.HasClass("beta"));
                                if (tdSeason != null)
                                {
                                    //2 сезон 10 серия
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

                                season.Episodes.Add(episode);
                            }
                        }
                    }

                    #endregion
                }
            }

            model.Seasons = seasons;

            #endregion

            return model;
        }

        #endregion
    }
}