﻿using System;
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
    /// https://fantlab.ru
    /// </summary>
    public class FantLabExternalProvider : BaseExternalProvider, IExternalServiceProvider<FantLabBookModel>
    {
        #region Properties

        public override string ServiceUri => "https://fantlab.ru";
        public override string ServiceName => "FantLib";

        #endregion

        #region IExternalServiceProvider

        public async Task<FantLabBookModel> Extract(string uri)
        {
            var model = new FantLabBookModel();

            var web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(uri);

            //main-info-block-detail
            var divMainInfoBlockDetail = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("main-info-block-detail"));

            if (divMainInfoBlockDetail != null)
            {
                var spans = divMainInfoBlockDetail.SelectNodes(".//span");
                if (spans != null)
                {
                    #region Extract Titles

                    var spanName = spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "name");
                    if (spanName != null)
                    {
                        model.Title = spanName.InnerText.ClearString();

                        var ps = spanName.ParentNode?.ParentNode?.SelectNodes(".//p");

                        model.OriginalTitle = ExtractOriginalTitle(ps, model.Title);
                        model.OtherTitles = ExtractOtherTitle(ps);
                    }

                    #endregion

                    #region Extract Author

                    model.Author = ExtractAuthor(spans);

                    #endregion

                    #region Extract Release Year

                    model.ReleaseYear = ExtractReleaseYear(spans);

                    #endregion

                    #region Extract Description 

                        model.Description = ExtractDescription(spans);

                    #endregion
                }

                #region Extract Series

                var aSeries = GetSeriesLink(divMainInfoBlockDetail);

                model.Series = ExtractSeries(aSeries);
                model.TrackingUri = ExtractTrackingUri(aSeries);

                #endregion

                #region Extract Image

                model.ImageUri = await ExtractImage(web, divMainInfoBlockDetail);

                #endregion
            }

            return model;
        }

        #endregion

        #region Private Methods

        private string ExtractOriginalTitle(HtmlNodeCollection ps, string title)
        {
            var pOriginalTitle = ps?.FirstOrDefault();
            if (pOriginalTitle != null && pOriginalTitle.ChildNodes.Count == 1 &&
                pOriginalTitle.FirstChild?.NodeType == HtmlNodeType.Text)
                return pOriginalTitle.InnerText.ClearString();

            return title;
        }

        private List<string> ExtractOtherTitle(HtmlNodeCollection ps)
        {
            const string otherTitlesKey = "Другие названия:";
            var pOtherTitle = ps?.FirstOrDefault(p => p.InnerText.ClearString().StartsWith(otherTitlesKey));
            if (pOtherTitle == null)
                return null;

            var otherTitlesString = pOtherTitle.InnerText.Replace(otherTitlesKey, string.Empty)
                .ClearString();
            var otherTitles = otherTitlesString.Split(';')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            return otherTitles.Count > 0 ? otherTitles : null;
        }

        private string ExtractAuthor(HtmlNodeCollection spans)
        {
            var spanAuthor = spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "author");
            return spanAuthor?.InnerText.ClearString();
        }

        private int? ExtractReleaseYear(HtmlNodeCollection spans)
        {
            var spanDateReleased =
                spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "datePublished");

            return spanDateReleased?.InnerText.ParseNumber();
        }

        private string ExtractDescription(HtmlNodeCollection spans)
        {
            var spanDescription = spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "description");
            return spanDescription?.InnerText.ClearString();
        }

        private HtmlNode GetSeriesLink(HtmlNode divMainInfoBlockDetail)
        {
            var divsLnTextx = divMainInfoBlockDetail.SelectNodes(".//div")?.Where(n => n.HasClass("ln-text"));
            var divLnTextx = divsLnTextx?.FirstOrDefault(n =>
            {
                var decodeInnerText = HttpUtility.HtmlDecode(n.InnerText)?.ClearString();
                var result = decodeInnerText?.StartsWith("— цикл") ?? false;
                return result;
            });

            var asSeries = divLnTextx?.SelectNodes(".//a");
            var aSeries = asSeries?.LastOrDefault();
            return aSeries;
        }

        private string ExtractSeries(HtmlNode aSeries)
        {
            var series = aSeries?.InnerText;
            if (series == null)
                return null;

            series = Regex.Match(series, @"[\w\s\d]+").Value;
            return series;
        }

        private string ExtractTrackingUri(HtmlNode aSeries)
        {
            var href = aSeries?.Attributes["href"]?.Value;
            return string.IsNullOrWhiteSpace(href) ? null : ServiceUri.AppendUriPath(href);
        }

        private async Task<int?> ExtractIndex(HtmlWeb web, string trackingUri, string title)
        {
            var seriesDocument = await web.LoadFromWebAsync(trackingUri);

            var divsBooks = seriesDocument.DocumentNode.SelectNodes(".//div")?
                .Where(n => n.HasClass("dots"))
                .ToList();

            if (divsBooks == null)
                return null;

            int counter = 0;

            foreach (var divBook in divsBooks)
            {
                counter++;

                var aBook = divBook.SelectSingleNode(".//a");
                if (aBook == null)
                    continue;

                if (!string.Equals(aBook.InnerText.ClearString(), title, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                return counter;
            }

            return null;
        }

        private async Task<string> ExtractImage(HtmlWeb web, HtmlNode divMainInfoBlockDetail)
        {
            //editions_block
            var divEditionsBlock = divMainInfoBlockDetail.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("editions_block"));

            var aFirstEdition = divEditionsBlock?.SelectNodes(".//a")?.FirstOrDefault();

            var link = aFirstEdition?.Attributes["href"]?.Value;

            if (string.IsNullOrWhiteSpace(link))
                return null;

            link = ServiceUri.AppendUriPath(link);

            var editionDocument = await web.LoadFromWebAsync(link);

            var img = editionDocument.DocumentNode?.SelectNodes(".//img")?
                .FirstOrDefault(n => n.Attributes["itemprop"]?.Value == "image");
            var imageUri = img?.Attributes["src"]?.Value;

            if (string.IsNullOrWhiteSpace(imageUri))
                return null;

            imageUri = $"https:{imageUri}";
            return imageUri;
        }

        #endregion
    }
}