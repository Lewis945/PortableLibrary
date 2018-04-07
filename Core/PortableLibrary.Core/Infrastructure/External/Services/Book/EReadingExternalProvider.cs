using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
    /// https://www.e-reading.club
    /// </summary>
    public class EReadingExternalProvider : BaseExternalProvider, IExternalServiceProvider<EReadingBookModel>
    {
        #region Properties

        public override string ServiceUri => "https://www.e-reading.club";
        public override string ServiceName => "EReading";

        #endregion

        #region .ctor

        public EReadingExternalProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        #endregion

        #region IExternalServiceProvider

        public Task<EReadingBookModel> Extract()
        {
            throw new NotImplementedException();
        }

        public async Task<EReadingBookModel> Extract(string uri)
        {
            var model = new EReadingBookModel();

            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            var wc = new WebClient { Encoding = win1251 };
            string str = await wc.DownloadStringTaskAsync(uri);
            var document = new HtmlDocument();
            document.LoadHtml(str);

            var divItemScope = document.DocumentNode.SelectNodes(".//div").FirstOrDefault(n => n.Attributes["itemscope"] != null);

            var tableBody = divItemScope.SelectSingleNode("./table");

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
                            var tdImage = tdsInner?.FirstOrDefault();

                            if (tdImage != null)
                            {
                                var img = tdImage.SelectSingleNode(".//img");
                                if (img != null)
                                {
                                    var imageSrc = img.Attributes["src"].Value;
                                    imageSrc = ServiceUri.AppendUriPath(imageSrc);

                                    model.ImageUri = imageSrc;
                                    model.ImageByteArray = await GetImageAsByteArray(imageSrc);
                                }
                            }

                            var tdData = tdsInner?.LastOrDefault();

                            if (tdData != null)
                            {
                                List<(string Key, string Value)> items = null;

                                var trsAll = tdData.SelectNodes(".//tr");
                                if (trsAll != null)
                                {
                                    items = new List<(string Key, string Value)>();

                                    //^(?<key>[\w\s]+):{1}(?<value>[\w\s,;\.]+)$
                                    //^(?<key>.+):{1}(?<value>.+)$
                                    var regex = new Regex(@"^(?<key>[\w\s]+):{1}(?<value>.+)$");

                                    foreach (var tr in trsAll)
                                    {
                                        var trText = tr.InnerText;
                                        trText = trText.ClearString().RemoveNewLines();

                                        var match = regex.Match(trText);

                                        if (match.Success)
                                        {
                                            var key = match.Groups["key"].Value.ClearString();
                                            var value = match.Groups["value"].Value.ClearString();

                                            items.Add((key, value));
                                        }
                                    }
                                }

                                #region Title

                                const string titleKey = "Название";

                                model.Title = items?.FirstOrDefault(i => i.Key == titleKey).Value;

                                #endregion

                                #region Author

                                const string authorKey = "Автор";

                                model.Author = items?.FirstOrDefault(i => i.Key == authorKey).Value;

                                #endregion

                                #region Description

                                const string descriptionKey = "Описание";

                                model.Description = items?.FirstOrDefault(i => i.Key == descriptionKey).Value;

                                #endregion

                                #region Series

                                const string seriesKey = "Серия";

                                model.Serie = items?.FirstOrDefault(i => i.Key == seriesKey).Value;

                                #endregion

                                #region Year

                                const string yearKey = "Издание";

                                var yearString = items?.FirstOrDefault(i => i.Key == yearKey).Value;
                                model.Year = yearString.ParseNumber();

                                #endregion

                                #region Genres

                                const string genresKey = "Жанр";

                                var genresString = items?.FirstOrDefault(i => i.Key == genresKey).Value;

                                if (genresString != null)
                                {
                                    var genres = genresString.Split(',');
                                    model.Genres = genres.Length > 0 ? genres.Select(g => g.ClearString()).ToList() : null;
                                }

                                #endregion

                                var t = items;
                            }
                        }
                    }

                    var bDowload = tdFirst.SelectNodes("./b")?.FirstOrDefault(n => n.InnerText.Contains("Скачать эту книгу"));
                    if (bDowload != null)
                    {
                        List<(string Name, string Value)> links = null;

                        var aItems = bDowload.SelectNodes("./a");
                        if (aItems != null)
                        {
                            links = new List<(string Name, string Value)>();
                            foreach (var aItem in aItems)
                            {
                                var link = aItem.Attributes["href"].Value;
                                if (!string.IsNullOrWhiteSpace(link))
                                    links.Add((aItem.InnerText, ServiceUri.AppendUriPath(HttpUtility.UrlDecode(link))));
                            }
                        }

                        model.DownloadLinks = links;
                    }
                }
            }

            return model;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}