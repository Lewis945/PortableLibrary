using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book
{
    /// <summary>
    /// https://fantasy-worlds.org
    /// </summary>
    public class FantasyWorldsExternalProvider : BaseExternalProvider, IExternalServiceProvider<FantasyWorldsBookModel>
    {
        #region Properties

        public override string ServiceUri => "https://fantasy-worlds.org";
        public override string ServiceName => "FantasyWorlds";

        #endregion

        #region IExternalServiceProvider

        public Task<FantasyWorldsBookModel> Extract()
        {
            throw new NotImplementedException();
        }

        public async Task<FantasyWorldsBookModel> Extract(string uri)
        {
            var model = new FantasyWorldsBookModel();

            var web = new HtmlWeb();
            var document = web.Load(uri);

            //news_body
            var divNewsBody = document.DocumentNode.SelectNodes(".//table")?
                .FirstOrDefault(n => n.HasClass("news_body"));

            var tds = divNewsBody?.SelectNodes(".//td");

            if (tds == null || !tds.Any())
                return null;

            var tdSecond = tds.LastOrDefault();
            var ps = tdSecond?.SelectNodes(".//p");

            if (ps != null && ps.Any())
            {
                var pFirst = ps.FirstOrDefault();
                var text = pFirst?.InnerText;

                //^(?<key>[\w\s]+):{1}(?<value>[\w\s,;\.]+)$
                //^(?<key>.+):{1}(?<value>.+)$
                var regex = new Regex(@"^(?<key>[\w\s]+):{1}(?<value>.+)$");

                var items = text?.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None)
                    .Select(l => HttpUtility.HtmlDecode(l.Trim()))
                    .Where(l => l != null && regex.IsMatch(l))
                    .Select(l =>
                    {
                        var match = regex.Match(l);
                        var key = match.Groups["key"].Value.ClearString();
                        var value = match.Groups["value"].Value.ClearString();

                        return new
                        {
                            Key = key,
                            Value = value
                        };
                    })
                    .ToList();

                #region Extract Title

                const string titleKey = "Название";

                var title = items?.FirstOrDefault(i =>
                    string.Equals(i.Key, titleKey, StringComparison.InvariantCultureIgnoreCase));
                model.Title = title?.Value.ClearString();

                #endregion

                #region Extract Original Title

                const string originalTitleKey = "Оригинальное название";

                var originalTitle = items?.FirstOrDefault(i =>
                    string.Equals(i.Key, originalTitleKey, StringComparison.InvariantCultureIgnoreCase));
                model.OriginalTitle = originalTitle?.Value.ClearString();

                if (string.IsNullOrWhiteSpace(model.OriginalTitle))
                    model.OriginalTitle = model.Title;
                
                #endregion

                #region Extract Other Titles

                const string otherTitlesKey = "Другие названия";

                var otherTitles = items?.FirstOrDefault(i =>
                    string.Equals(i.Key, otherTitlesKey, StringComparison.InvariantCultureIgnoreCase));
                model.OtherTitles = otherTitles?.Value.Split(';').Select(t => t.ClearString()).ToList();

                #endregion

                #region Extract Author

                const string authorKey = "Автор";

                var author = items?.FirstOrDefault(i =>
                    string.Equals(i.Key, authorKey, StringComparison.InvariantCultureIgnoreCase));
                model.Author = author?.Value.ClearString();

                #endregion

                #region Extract Series

                const string seriesKey = "Серия";

                var series = items?.FirstOrDefault(i =>
                    string.Equals(i.Key, seriesKey, StringComparison.InvariantCultureIgnoreCase));

                List<string> seriesList = null;
                if (series != null)
                {
                    seriesList = new List<string>();
                    if (series.Value.Contains(':'))
                    {
                        var allSeries = series.Value.Split(':');
                        seriesList.AddRange(allSeries.Select(s => s.ClearString()));
                    }
                    else
                        seriesList.Add(series.Value);
                }

                model.Series = seriesList;

                #endregion

                #region Extract Index

                const string indexKey = "Номер книги в серии";

                var indexString = items?.FirstOrDefault(i =>
                    string.Equals(i.Key, indexKey, StringComparison.InvariantCultureIgnoreCase));
                var indexParseResult = int.TryParse(indexString?.Value.ClearString(), out var index);
                model.Index = indexParseResult ? index : (int?) null;

                #endregion

                #region Extract Release Year

                const string yearKey = "Год";

                var yearString = items?.FirstOrDefault(i =>
                    string.Equals(i.Key, yearKey, StringComparison.InvariantCultureIgnoreCase));
                var yearParseResult = int.TryParse(yearString?.Value.ClearString(), out var year);
                model.ReleaseYear = yearParseResult ? year : (int?) null;

                #endregion

                var pSecond = ps.Skip(1).FirstOrDefault();

                #region Extract Description

                var spanDescription = pSecond?.SelectSingleNode("./span");

                var description = spanDescription?.InnerText.ClearString();
                description = HttpUtility.HtmlDecode(description);
                model.Description = description;

                #endregion

                var pThird = ps.Skip(2).FirstOrDefault();

                #region Extract Dowload Links

                var aBookLink = pThird?.SelectSingleNode("./a");
                var href = aBookLink?.Attributes["href"].Value;

                List<(string Key, string Value)> links = null;
                if (!string.IsNullOrWhiteSpace(href))
                {
                    href = ServiceUri.AppendUriPath(href);

                    var selectOptions = pThird?.SelectSingleNode(".//select");
                    var options = selectOptions?.SelectNodes("./option")?.Select(n => n.Attributes["value"].Value)
                        .ToList();

                    links = new List<(string Key, string Value)>();
                    if (options != null && options.Any())
                    {
                        links.AddRange(options.Select((option, i) =>
                            i == 0 ? (option, href) : (option, href.AppendUriPath(option))));
                    }
                }

                model.DownloadLinks = links;

                #endregion
            }

            #region Extract Image

            var tdFirst = tds.FirstOrDefault();

            var img = tdFirst?.SelectSingleNode(".//img");
            var imageUri = img?.Attributes["src"].Value;
            if (!string.IsNullOrWhiteSpace(imageUri))
                imageUri = ServiceUri.AppendUriPath(imageUri);

            model.ImageUri = imageUri;

            var imageByteArray = await GetImageAsByteArray(imageUri);
            model.ImageByteArray = imageByteArray;

            #endregion

            return model;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}