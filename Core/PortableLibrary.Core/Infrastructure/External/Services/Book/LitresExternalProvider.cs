using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book
{
    /// <summary>
    /// https://www.litres.ru
    /// </summary>
    public class LitresExternalProvider : IExternalServiceProvider<LitresBookModel>
    {
        #region Properties

        public string ServiceUri => "https://www.litres.ru";
        public string ServiceName => "Litres";

        #endregion

        #region IExternalServiceProvider

        public async Task<LitresBookModel> Extract()
        {
            throw new NotImplementedException();
        }

        public async Task<LitresBookModel> Extract(string uri)
        {
            var model = new LitresBookModel();

            var web = new HtmlWeb();
            var document = web.Load(uri);

            //biblio_book_top block_table
            var divInfo1 = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("biblio_book_top") && n.HasClass("block_table"));

            if (divInfo1 != null)
            {
                #region Extract book's cover image

                var imageUri = ExtractImage(divInfo1);
                var imageByteArray = await GetImageAsByteArray(imageUri);

                model.ImageUri = imageUri;
                model.ImageByteArray = imageByteArray;

                #endregion

                #region Extract book's title

                model.Title = ExtractBookTitle(divInfo1);

                #endregion

                #region Extract book's author

                model.Author = ExtractBookAuthor(divInfo1);

                #endregion

                #region Extract book's series

                var series = ExtractBookSeries(divInfo1).ToList();

                var authorSeries = series.FirstOrDefault(s => s.Index > 0);
                if (!authorSeries.Equals(default((string, int))))
                {
                    model.AuthorSeries = authorSeries.Name;
                    model.Index = authorSeries.Index;
                }

                var publisherSeries = series.Where(s => s.Index == 0).Select(s => s.Name).ToList();
                model.PublishersSeries = publisherSeries.Count > 0 ? publisherSeries : null;

                #endregion

                #region Extract book's genres

                model.Genres = ExtractBookGenres(divInfo1);

                #endregion
            }

            var divInfo2 = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("biblio_book_bottom") && n.HasClass("biblio_bottom__tab"));

            if (divInfo2 != null)
            {
                #region Extract book's description

                model.Description = ExtractBookDescription(divInfo2);

                #endregion

                var divBiblioBookInfoDetailed = divInfo2.SelectNodes("//div")?
                    .FirstOrDefault(n => n.HasClass("biblio_book_info_detailed"));

                #region Extract book's publish date

                model.DatePublished = ExtractBookPublishDate(divBiblioBookInfoDetailed);

                #endregion

                #region Extract book's pages count

                model.PagesCount = ExtractBookPagesCount(divBiblioBookInfoDetailed);

                #endregion
            }

            return model;
        }

        #endregion

        #region Private Methods

        private static string ExtractImage(HtmlNode divInfo1)
        {
            var divBiblioBookCover =
                divInfo1.SelectNodes(".//div")?.FirstOrDefault(d => d.HasClass("biblio_book_cover"));

            var divBiblioBookCoverInside = divBiblioBookCover?.SelectNodes(".//div")?
                .FirstOrDefault(d => d.HasClass("biblio_book_cover_inside"));

            var img = divBiblioBookCoverInside?.SelectNodes(".//img")?.FirstOrDefault();

            var imageUri = img?.Attributes["data-original-image-url"].Value;
            if (string.IsNullOrWhiteSpace(imageUri))
                return null;

            if (imageUri.StartsWith("//"))
                imageUri = imageUri.Substring(2);

            imageUri = $"https://{imageUri}";
            return imageUri;
        }

        private static string ExtractBookTitle(HtmlNode divInfo1)
        {
            var h1Title = divInfo1.SelectNodes(".//h1")?.FirstOrDefault(n => n.HasClass("biblio_book_name"));
            return h1Title?.InnerText.Trim();
        }

        private static string ExtractBookAuthor(HtmlNode divInfo1)
        {
            var divBiblioBookAuthor =
                divInfo1.SelectNodes(".//div")?.FirstOrDefault(d => d.HasClass("biblio_book_author"));
            var authorLink = divBiblioBookAuthor?.SelectSingleNode(".//a");
            return authorLink?.InnerText.Trim();
        }

        private static IEnumerable<(string Name, int Index)> ExtractBookSeries(HtmlNode divInfo1)
        {
            var divBiblioBookSequences = divInfo1.SelectNodes(".//div")?
                .Where(d => d.HasClass("biblio_book_sequences"))
                .ToList();

            if (divBiblioBookSequences == null)
                yield break;

            foreach (var divBiblioBookSequence in divBiblioBookSequences)
            {
                var spanSeries = divBiblioBookSequence.SelectNodes("./span")?
                    .FirstOrDefault(n => n.HasClass("serie_item"));
                if (spanSeries == null) continue;

                var spanNumber = spanSeries.SelectNodes("./span")?.FirstOrDefault(n => n.HasClass("number"));
                if (spanNumber != null)
                {
                    var numberString = spanNumber.InnerText.Replace("#", string.Empty);
                    int.TryParse(numberString, out var index);
                    string series = spanSeries.InnerText.Replace(spanNumber.InnerText, string.Empty).Trim();
                    yield return (series, index);
                }
                else
                    yield return (spanSeries.InnerText.Trim(), 0);
            }
        }

        private static List<string> ExtractBookGenres(HtmlNode divInfo1)
        {
            var divBiblioBookInfo =
                divInfo1?.SelectNodes(".//div")?.FirstOrDefault(d => d.HasClass("biblio_book_info"));
            var liGenres = divBiblioBookInfo?.SelectNodes(".//li")
                ?.FirstOrDefault(n => n.Descendants().First().InnerText == "Жанр:");

            return liGenres?.SelectNodes("./a")?.Select(n => n.InnerText.Trim()).ToList();
        }

        private static string ExtractBookDescription(HtmlNode divInfo2)
        {
            var divBiblioBookDescrPublishers = divInfo2?.SelectNodes(".//div")
                ?.FirstOrDefault(n => n.HasClass("biblio_book_descr_publishers"));

            return divBiblioBookDescrPublishers?.InnerText.Trim();
        }

        private static DateTime? ExtractBookPublishDate(HtmlNode divBiblioBookInfoDetailed)
        {
            if (divBiblioBookInfoDetailed == null) return null;

            const string key = "Дата написания:";
            var liDatePublished = divBiblioBookInfoDetailed.SelectNodes("//li")
                ?.FirstOrDefault(n => n.Descendants().First().InnerText == key);

            if (liDatePublished == null) return null;

            var date = Regex.Match(liDatePublished.InnerText, @"\d+").Value;
            int.TryParse(date, out var year);
            return new DateTime(year, 1, 1);
        }

        private static int? ExtractBookPagesCount(HtmlNode divBiblioBookInfoDetailed)
        {
            if (divBiblioBookInfoDetailed == null) return null;

            const string key = "Объем:";
            var liPagesCount = divBiblioBookInfoDetailed.SelectNodes("//li")
                ?.FirstOrDefault(n => n.Descendants().First().InnerText == key);

            if (liPagesCount == null) return null;

            var pagesCountString = Regex.Match(liPagesCount.InnerText, @"\d+").Value;
            int.TryParse(pagesCountString, out var pagesCount);
            return pagesCount;
        }

        //https://github.com/SixLabors/ImageSharp
        private async Task<byte[]> GetImageAsByteArray(string imageUri)
        {
            var client = new HttpClient {BaseAddress = new Uri(ServiceUri)};
            var response = await client.GetAsync(imageUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                //log this
                //throw new Exception($"The image ({imageUri}) is not found.");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                //log this
                //throw new Exception($"Access to the image ({imageUri}) is forbidden.");
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        #endregion
    }
}