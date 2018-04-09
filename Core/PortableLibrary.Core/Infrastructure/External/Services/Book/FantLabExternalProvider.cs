using System;
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
    /// https://fantlab.ru
    /// </summary>
    public class FantLabExternalProvider : IExternalServiceProvider<FantLabBookModel>
    {
        public string ServiceUri => "https://fantlab.ru";
        public string ServiceName => "FantLib";

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
                    #region Extract Title

                    var spanName = spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "name");
                    if (spanName != null)
                    {
                        model.Title = spanName.InnerText.ClearString();

                        var ps = spanName.ParentNode?.ParentNode?.SelectNodes(".//p");

                        var pOriginalTitle = ps?.FirstOrDefault();
                        if (pOriginalTitle != null && pOriginalTitle.ChildNodes.Count == 1 &&
                            pOriginalTitle.FirstChild?.NodeType == HtmlNodeType.Text)
                        {
                            model.OriginalTitle = pOriginalTitle.InnerText.ClearString();
                        }
                        else
                        {
                            model.OriginalTitle = model.Title;
                        }

                        const string otherTitlesKey = "Другие названия:";
                        var pOtherTitle = ps?.FirstOrDefault(p => p.InnerText.ClearString().StartsWith(otherTitlesKey));
                        if (pOtherTitle != null)
                        {
                            var otherTitlesString = pOtherTitle.InnerText.Replace(otherTitlesKey, string.Empty).ClearString();
                            var otherTitles = otherTitlesString.Split(';').Select(s => s.Trim());
                            model.OtherTitles = otherTitles.ToList();
                        }
                    }

                    #endregion

                    #region Extract Author

                    var spanAuthor = spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "author");
                    if (spanAuthor != null)
                    {
                        model.Author = spanAuthor.InnerText.ClearString();
                    }

                    #endregion

                    #region Extract Release Year

                    var spanDateReleased = spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "datePublished");
                    if (spanDateReleased != null)
                    {
                        model.ReleaseYear = spanDateReleased.InnerText.ParseNumber();
                    }

                    #endregion

                    #region Extract Description 

                    var spanDescription = spans.FirstOrDefault(s => s.Attributes["itemprop"]?.Value == "description");
                    if (spanDescription != null)
                    {
                        model.Description = spanDescription.InnerText.ClearString();
                    }

                    #endregion
                }

                #region Extract Series

                var divsLnTextx = divMainInfoBlockDetail.SelectNodes(".//div")?.Where(n => n.HasClass("ln-text"));
                if (divsLnTextx != null)
                {
                    var divLnTextx = divsLnTextx.FirstOrDefault(n =>
                    {
                        var decodeInnerText = HttpUtility.HtmlDecode(n.InnerText)?.ClearString();
                        var result = decodeInnerText.StartsWith("— цикл");
                        return result;
                    });

                    if (divLnTextx != null)
                    {
                        var asSeries = divLnTextx.SelectNodes(".//a");
                        if (asSeries != null)
                        {
                            var aSeries = asSeries.LastOrDefault();

                            var series = aSeries?.InnerText;
                            series = Regex.Match(series, @"[\w\s\d]+").Value;
                            model.Series = series;

                            var href = aSeries?.Attributes["href"]?.Value;
                            if (!string.IsNullOrWhiteSpace(href))
                            {
                                model.TrackingUri = ServiceUri.AppendUriPath(href);

                                #region Extract Index

                                var seriesDocument = await web.LoadFromWebAsync(model.TrackingUri);

                                var divsBooks = seriesDocument.DocumentNode.SelectNodes(".//div")?
                                    .Where(n => n.HasClass("dots"));
                                if (divsBooks != null)
                                {
                                    int counter = 1;
                                    foreach (var divBook in divsBooks)
                                    {
                                        var aBook = divBook.SelectSingleNode(".//a");
                                        if (aBook != null)
                                        {
                                            if (aBook.InnerText.ClearString() == model.Title)
                                            {
                                                model.Index = counter;
                                                break;
                                            }
                                        }
                                        counter++;
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                }

                #endregion

                #region Extract Image

                //editions_block
                var divEditionsBlock = divMainInfoBlockDetail.SelectNodes(".//div")?.FirstOrDefault(n => n.HasClass("editions_block"));
                if (divEditionsBlock != null)
                {
                    var aFirstEdition = divEditionsBlock.SelectNodes(".//a")?.FirstOrDefault();
                    if (aFirstEdition != null)
                    {
                        var link = aFirstEdition.Attributes["href"]?.Value;
                        if (!string.IsNullOrWhiteSpace(link))
                        {
                            link = ServiceUri.AppendUriPath(link);

                            var editionDocument = await web.LoadFromWebAsync(link);

                            var img = editionDocument.DocumentNode.SelectNodes(".//img")?
                                .FirstOrDefault(n => n.Attributes["itemprop"]?.Value == "image");
                            if (img != null)
                            {
                                var imageUri = img.Attributes["src"]?.Value;
                                if (!string.IsNullOrWhiteSpace(imageUri))
                                {
                                    imageUri = $"https:{imageUri}";
                                    model.ImageUri = imageUri;
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            return model;
        }
    }
}