using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models.Book;

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
                var items = ExtractBookData(ps);

                if (items != null)
                {
                    #region Extract Title

                    model.Title = ExtractTitle(items);

                    #endregion

                    #region Extract Original Title

                    model.OriginalTitle = ExtractOriginalTitle(items, model.Title);

                    #endregion

                    #region Extract Other Titles

                    model.OtherTitles = ExtractOtherTitles(items);

                    #endregion

                    #region Extract Author

                    model.Author = ExtractAuthor(items);

                    #endregion

                    #region Extract Series

                    const string seriesKey = "Серия";

                    model.Series = ExtractSeries(items);

                    #endregion

                    #region Extract Index

                    model.Index = ExtractIndex(items);

                    #endregion

                    #region Extract Release Year

                    model.ReleaseYear = ExtractReleaseYear(items);

                    #endregion
                }

                var pSecond = ps.Skip(1).FirstOrDefault();

                #region Extract Description

                model.Description = ExtractDescription(ps);

                #endregion

                var pThird = ps.Skip(2).FirstOrDefault();

                #region Extract Dowload Links

                model.DownloadLinks = ExtractDowloadLinks(ps);

                #endregion
            }

            #region Extract Image

            model.ImageUri = ExtractImage(tds);

            #endregion

            return model;
        }

        #endregion

        #region Private Methods

        private List<(string Key, string Value)> ExtractBookData(HtmlNodeCollection ps)
        {
            var pFirst = ps.FirstOrDefault();
            var text = pFirst?.InnerText;

            if (text == null)
                return null;

            //^(?<key>[\w\s]+):{1}(?<value>[\w\s,;\.]+)$
            //^(?<key>.+):{1}(?<value>.+)$
            var regex = new Regex(@"^(?<key>[\w\s]+):{1}(?<value>.+)$");

            var items = text.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None)
                .Select(l => HttpUtility.HtmlDecode(l.Trim()))
                .Where(l => l != null && regex.IsMatch(l))
                .Select(l =>
                {
                    var match = regex.Match(l);
                    var key = match.Groups["key"]?.Value.ClearString();
                    var value = match.Groups["value"]?.Value.ClearString();

                    return (Key: key, Value: value);
                })
                .Where(l => l.Key != null && l.Value != null)
                .ToList();

            return items;
        }

        private string ExtractTitle(List<(string Key, string Value)> items)
        {
            const string titleKey = "Название";

            var title = items.FirstOrDefault(i =>
                string.Equals(i.Key, titleKey, StringComparison.InvariantCultureIgnoreCase));

            return string.IsNullOrWhiteSpace(title.Value) ? null : title.Value.ClearString();
        }

        private string ExtractOriginalTitle(List<(string Key, string Value)> items, string title)
        {
            const string originalTitleKey = "Оригинальное название";

            var originalTitle = items.FirstOrDefault(i =>
                string.Equals(i.Key, originalTitleKey, StringComparison.InvariantCultureIgnoreCase));

            return string.IsNullOrWhiteSpace(originalTitle.Value) ? title : originalTitle.Value.ClearString();
        }

        private List<string> ExtractOtherTitles(List<(string Key, string Value)> items)
        {
            const string otherTitlesKey = "Другие названия";

            var otherTitlesTuple = items.FirstOrDefault(i =>
                string.Equals(i.Key, otherTitlesKey, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrWhiteSpace(otherTitlesTuple.Value))
                return null;

            var otherTitles = otherTitlesTuple.Value.Split(';')
                .Select(t => t.ClearString())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();

            return otherTitles;
        }

        private string ExtractAuthor(List<(string Key, string Value)> items)
        {
            const string authorKey = "Автор";

            var author = items.FirstOrDefault(i =>
                string.Equals(i.Key, authorKey, StringComparison.InvariantCultureIgnoreCase));

            return string.IsNullOrWhiteSpace(author.Value) ? null : author.Value.ClearString();
        }

        private List<string> ExtractSeries(List<(string Key, string Value)> items)
        {
            const string seriesKey = "Серия";

            var seriesTuple = items.FirstOrDefault(i =>
                string.Equals(i.Key, seriesKey, StringComparison.InvariantCultureIgnoreCase));

            var series = seriesTuple.Value;

            if (string.IsNullOrWhiteSpace(series))
                return null;

            var seriesList = new List<string>();
            if (series.Contains(':'))
            {
                var allSeries = series.Split(':');
                seriesList.AddRange(allSeries.Select(s => s.ClearString()).Where(s => !string.IsNullOrWhiteSpace(s)));
            }
            else
                seriesList.Add(series);

            return seriesList;
        }

        private int? ExtractIndex(List<(string Key, string Value)> items)
        {
            const string indexKey = "Номер книги в серии";

            var indexString = items.FirstOrDefault(i =>
                string.Equals(i.Key, indexKey, StringComparison.InvariantCultureIgnoreCase));

            if (!int.TryParse(indexString.Value.ClearString(), out var index))
                return null;

            return index;
        }

        private int? ExtractReleaseYear(List<(string Key, string Value)> items)
        {
            const string yearKey = "Год";

            var yearString = items.FirstOrDefault(i =>
                string.Equals(i.Key, yearKey, StringComparison.InvariantCultureIgnoreCase));

            if (!int.TryParse(yearString.Value.ClearString(), out var year))
                return null;

            return year;
        }

        private string ExtractDescription(HtmlNodeCollection ps)
        {
            var pSecond = ps.Skip(1).FirstOrDefault();

            var spanDescription = pSecond?.SelectSingleNode("./span");

            if (spanDescription == null)
                return null;

            var description = spanDescription.InnerText.ClearString();
            description = HttpUtility.HtmlDecode(description);
            return description;
        }

        private string ExtractImage(HtmlNodeCollection tds)
        {
            var tdFirst = tds.FirstOrDefault();

            var img = tdFirst?.SelectSingleNode(".//img");
            var imageUri = img?.Attributes["src"]?.Value;
            if (string.IsNullOrWhiteSpace(imageUri))
                return null;

            imageUri = ServiceUri.AppendUriPath(imageUri);
            return imageUri;
        }

        private List<(string Key, string Value)> ExtractDowloadLinks(HtmlNodeCollection ps)
        {
            var pThird = ps.Skip(2).FirstOrDefault();

            var aBookLink = pThird?.SelectSingleNode("./a");
            var href = aBookLink?.Attributes["href"]?.Value;

            if (string.IsNullOrWhiteSpace(href))
                return null;

            href = ServiceUri.AppendUriPath(href);

            var selectOptions = pThird.SelectSingleNode(".//select");
            var options = selectOptions?.SelectNodes("./option")?
                .Select(n => n.Attributes["value"]?.Value)
                .Where(o => o != null)
                .ToList();

            if (options == null || !options.Any())
                return null;

            var links = options
                .Select((option, i) => i == 0 ? (option, href) : (option, href.AppendUriPath(option)))
                .ToList();

            return links;
        }

        #endregion
    }
}