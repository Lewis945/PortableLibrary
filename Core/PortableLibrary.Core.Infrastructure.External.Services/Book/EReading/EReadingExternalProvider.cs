using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book.EReading
{
    /// <summary>
    /// https://www.e-reading.club
    /// </summary>
    public class EReadingExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "https://www.e-reading.club";
        public override string ServiceName => "EReading";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public EReadingExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        #endregion

        #region IExternalServiceProvider

        public async Task<EReadingBookModel> ExtractBook(string uri)
        {
            var model = new EReadingBookModel();

            var document = await GetDocument(uri);

            var divItemScope = document.DocumentNode.SelectNodes(".//div")
                .FirstOrDefault(n => n.Attributes["itemscope"] != null);

            var tableBody = divItemScope?.SelectSingleNode("./table");

            if (tableBody != null)
            {
                var trFirst = tableBody.SelectSingleNode("./tr");
                var tdFirst = trFirst?.SelectSingleNode("./td");

                if (tdFirst != null)
                {
                    var tableInner = tdFirst.SelectSingleNode("./table");

                    if (tableInner != null)
                    {
                        trFirst = tableInner.SelectSingleNode("./tr");

                        if (trFirst != null)
                        {
                            var tdsInner = trFirst.SelectNodes("./td");

                            #region Extract Image

                            model.ImageUri = GetImageUri(tdsInner);

                            #endregion

                            var tdData = tdsInner?.LastOrDefault();

                            if (tdData != null)
                            {
                                var data = ExtractBookData(tdData);

                                #region Title

                                const string titleKey = "Название";

                                model.Title = data?.FirstOrDefault(i => i.Key == titleKey).Value;

                                #endregion

                                #region Author

                                const string authorKey = "Автор";

                                model.Author = data?.FirstOrDefault(i => i.Key == authorKey).Value;

                                #endregion

                                #region Description

                                const string descriptionKey = "Описание";

                                model.Description = data?.FirstOrDefault(i => i.Key == descriptionKey).Value;

                                #endregion

                                #region Series

                                const string seriesKey = "Серия";

                                model.Serie = data?.FirstOrDefault(i => i.Key == seriesKey).Value;

                                #endregion

                                #region Year

                                const string yearKey = "Издание";

                                var yearString = data?.FirstOrDefault(i => i.Key == yearKey).Value;
                                model.ReleaseYear = yearString.ParseNumber();

                                #endregion

                                #region Genres

                                const string genresKey = "Жанр";

                                var genresString = data?.FirstOrDefault(i => i.Key == genresKey).Value;

                                if (genresString != null)
                                {
                                    var genres = genresString.Split(',');
                                    model.Genres = genres.Length > 0
                                        ? genres.Select(g => g.ClearString()).ToList()
                                        : null;
                                }

                                #endregion

                                #region Extract Tracking Uri & Index

                                model.TrackingUri = ExtractTrackingUri(tdData, seriesKey);

                                model.Index = await ExtractBookIndex(model.TrackingUri, model.Title);

                                #endregion
                            }
                        }
                    }

                    #region Extract Download Links

                    model.DownloadLinks = GetDownloadLinks(tdFirst);

                    #endregion
                }
            }

            return model;
        }

        public async Task<List<EReadingTrackedBookModel>> ExtractBooksToTrack(string uri)
        {
            var seriesDocument = await GetDocument(uri);

            var tableBooklList = seriesDocument.DocumentNode.SelectNodes(".//table")?
                .FirstOrDefault(n => n.HasClass("booklist"));

            var trs = tableBooklList?.SelectNodes(".//tr");

            if (trs == null)
                return null;

            var regex = new Regex(@"^(?<index>[\d]+\.).*");

            var books = new List<EReadingTrackedBookModel>();

            foreach (var tr in trs)
            {
                var divBook = tr.SelectNodes(".//div")?.FirstOrDefault();

                if (divBook == null)
                    continue;

                var aLinks = divBook.SelectNodes(".//a");
                var aBook = aLinks?.LastOrDefault();

                if (aBook == null)
                    continue;

                var match = regex.Match(tr.InnerText.ClearString());

                if (!match.Success)
                    continue;

                string indexString = match.Groups["index"]?.Value.ClearString();
                var index = indexString.ParseNumber();

                string title = aBook.InnerText.ClearString();

                books.Add(new EReadingTrackedBookModel
                {
                    Title = title,
                    Index = index
                });
            }

            return books;
        }

        #endregion

        #region Private Methods

        private async Task<HtmlDocument> GetDocument(string uri)
        {
            var win1251 = Encoding.GetEncoding("windows-1251");
            var web = new HtmlWeb();
            return await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, win1251));
        }

        private string GetImageUri(HtmlNodeCollection tdsInner)
        {
            var tdImage = tdsInner?.FirstOrDefault();

            var img = tdImage?.SelectSingleNode(".//img");

            var imageSrc = img?.Attributes["src"]?.Value;

            if (imageSrc == null)
                return null;

            imageSrc = ServiceUri.AppendUriPath(imageSrc);

            return imageSrc;
        }

        private static List<(string Key, string Value)> ExtractBookData(HtmlNode tdData)
        {
            var trsAll = tdData.SelectNodes(".//tr");

            if (trsAll == null) return null;

            var items = new List<(string Key, string Value)>();

            //^(?<key>[\w\s]+):{1}(?<value>[\w\s,;\.]+)$
            //^(?<key>.+):{1}(?<value>.+)$
            var regex = new Regex(@"^(?<key>[\w\s]+):{1}(?<value>.+)$");

            foreach (var tr in trsAll)
            {
                var trText = tr.InnerText;
                trText = trText.ClearString().RemoveNewLines();

                var match = regex.Match(trText);

                if (!match.Success) continue;

                var key = match.Groups["key"]?.Value.ClearString();
                var value = match.Groups["value"]?.Value.ClearString();

                if (key == null || value == null)
                    continue;

                items.Add((key, value));
            }

            return items;
        }

        private string ExtractTrackingUri(HtmlNode tdData, string key)
        {
            var trsAll = tdData.SelectNodes(".//tr");

            if (trsAll == null) return null;

            var trSerie = trsAll.Where(tr =>
                {
                    var trText = tr.InnerText;
                    trText = trText.ClearString().RemoveNewLines();
                    var match = trText.StartsWith(key);
                    return match;
                })
                .FirstOrDefault();

            var aSerie = trSerie?.SelectSingleNode(".//a");

            var href = aSerie?.Attributes["href"]?.Value;

            return href;
        }

        private async Task<int?> ExtractBookIndex(string trackingUri, string title)
        {
            var seriesDocument = await GetDocument(trackingUri);

            var tableBooklList = seriesDocument.DocumentNode.SelectNodes(".//table")?
                .FirstOrDefault(n => n.HasClass("booklist"));

            var trs = tableBooklList?.SelectNodes(".//tr");

            if (trs == null)
                return null;

            var regex = new Regex(@"^(?<index>[\d]+\.).*");

            foreach (var tr in trs)
            {
                var aLinks = tr.SelectNodes(".//a");
                var hasBook = aLinks?.Any(a => a.InnerText.Contains(title)) ?? false;

                if (!hasBook)
                    continue;

                var match = regex.Match(tr.InnerText);

                if (!match.Success)
                    continue;

                string indexString = match.Groups["index"]?.Value.ClearString();
                return indexString.ParseNumber();
            }

            return null;
        }

        private List<(string Name, string Value)> GetDownloadLinks(HtmlNode tdFirst)
        {
            var bDowload = tdFirst.SelectNodes("./b")?
                .FirstOrDefault(n => n.InnerText.Contains("Скачать эту книгу"));

            var aItems = bDowload?.SelectNodes("./a");

            if (aItems == null)
                return null;

            var links = new List<(string Name, string Value)>();
            foreach (var aItem in aItems)
            {
                var link = aItem.Attributes["href"]?.Value;
                if (!string.IsNullOrWhiteSpace(link))
                    links.Add((aItem.InnerText, ServiceUri.AppendUriPath(HttpUtility.UrlDecode(link))));
            }

            return links;
        }

        #endregion
    }
}