using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public string ServiceUri => "https://www.litres.ru";
        public string ServiceName => "Litres";

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
            var divInfo1 = document.DocumentNode.SelectNodes(".//div")
                .FirstOrDefault(n => n.HasClass("biblio_book_top") && n.HasClass("block_table"));

            if (divInfo1 == null)
                throw new Exception("The first information container div.biblio_book_top.block_table is not found.");

            #region Extract book's cover image

            var imageUri = ExtractImage(divInfo1);
            var imageByteArray = await GetImageAsByteArray(imageUri);

            model.ImageUri = imageUri;
            model.ImageByteArray = imageByteArray;

            #endregion

            #region Extract book's title

            var h1Title = divInfo1.SelectNodes(".//h1").FirstOrDefault(n => n.HasClass("biblio_book_name"));
            if (h1Title == null)
                throw new Exception("Title h1.biblio_book_name is not found");

            model.Title = h1Title.InnerText.Trim();

            #endregion

            #region Extract book's author

            var divBiblioBookAuthor =
                divInfo1.SelectNodes(".//div").FirstOrDefault(d => d.HasClass("biblio_book_author"));
            if (divBiblioBookAuthor == null)
                throw new Exception("div.biblio_book_author is not found.");

            var authorLink = divBiblioBookAuthor.SelectSingleNode(".//a");
            if (authorLink == null)
                throw new Exception("Link (<a href=''><a/>) to author is not found.");

            model.Author = authorLink.InnerText.Trim();

            #endregion

            #region Extract book's series

            var divBiblioBookSequences = divInfo1.SelectNodes(".//div").Where(d => d.HasClass("biblio_book_sequences"))
                .ToList();

            foreach (var divBiblioBookSequence in divBiblioBookSequences)
            {
                var spanSeries = divBiblioBookSequence.SelectNodes("./span")
                    .FirstOrDefault(n => n.HasClass("serie_item"));
                if (spanSeries != null)
                {
                    var spanNumber = spanSeries.SelectNodes("./span")?.FirstOrDefault(n => n.HasClass("number"));
                    if (spanNumber != null)
                    {
                        var numberString = spanNumber.InnerText.Replace("#", string.Empty);
                        int.TryParse(numberString, out var index);
                        model.Index = index;
                        model.AuthorSeries = spanSeries.InnerText.Replace(spanNumber.InnerText, string.Empty).Trim();
                    }
                    else
                        model.PublishersSeries.Add(spanSeries.InnerText.Trim());
                }
            }

            #endregion

            #region Extract book's genres

            var divBiblioBookInfo = divInfo1.SelectNodes(".//div")?.FirstOrDefault(d => d.HasClass("biblio_book_info"));
            if (divBiblioBookInfo != null)
            {
                var liGenres = divBiblioBookInfo.SelectNodes(".//li")
                    ?.FirstOrDefault(n => n.Descendants().First().InnerText == "Жанр:");

                if (liGenres != null)
                {
                    model.Genres = liGenres.SelectNodes("./a")?.Select(n => n.InnerText.Trim()).ToList() ??
                                   new List<string>();
                }
            }

            #endregion

            //biblio_book_bottom biblio_bottom__tab
            var divInfo2 = document.DocumentNode.SelectNodes(".//div")
                .FirstOrDefault(n => n.HasClass("biblio_book_bottom") && n.HasClass("biblio_bottom__tab"));

            if (divInfo2 == null)
                throw new Exception("The second information container div.biblio_book_top.block_table is not found.");

            #region Extract book's description

            //biblio_book_descr_publishers

            var divBiblioBookDescrPublishers = divInfo2.SelectNodes(".//div")
                ?.FirstOrDefault(n => n.HasClass("biblio_book_descr_publishers"));

            if (divBiblioBookDescrPublishers != null)
            {
                model.Description = divBiblioBookDescrPublishers.InnerText.Trim();
            }

            #endregion

            #region Extract book's publish date

            #endregion

            #region Extract book's pages count

            #endregion

            return model;
        }

        private static string ExtractImage(HtmlNode divInfo1)
        {
            var divBiblioBookCover =
                divInfo1.SelectNodes(".//div").FirstOrDefault(d => d.HasClass("biblio_book_cover"));
            if (divBiblioBookCover == null)
                throw new Exception("div.biblio_book_cover is not found.");

            var divBiblioBookCoverInside = divBiblioBookCover.SelectNodes(".//div")
                .FirstOrDefault(d => d.HasClass("biblio_book_cover_inside"));
            if (divBiblioBookCoverInside == null)
                throw new Exception("div.biblio_book_cover_inside is not found.");

            var img = divBiblioBookCoverInside.SelectNodes(".//img").FirstOrDefault();
            if (img == null)
                throw new Exception("img is not found.");

            var imageUri = img.Attributes["data-original-image-url"].Value;
            if (string.IsNullOrWhiteSpace(imageUri))
                throw new Exception($"No source image is found: {img}.");

            if (imageUri.StartsWith("//"))
                imageUri = imageUri.Substring(2);

            imageUri = $"https://{imageUri}";
            return imageUri;
        }

        //https://github.com/SixLabors/ImageSharp
        private async Task<byte[]> GetImageAsByteArray(string imageUri)
        {
            var client = new HttpClient {BaseAddress = new Uri(ServiceUri)};
            var response = await client.GetAsync(imageUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new Exception($"The image ({imageUri}) is not found.");
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new Exception($"Access to the image ({imageUri}) is forbidden.");

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}