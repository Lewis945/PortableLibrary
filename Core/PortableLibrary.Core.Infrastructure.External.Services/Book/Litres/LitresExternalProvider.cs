using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book.Litres
{
    /// <summary>
    /// https://www.litres.ru
    /// </summary>
    public class LitresExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "https://www.litres.ru";
        public override string ServiceName => "Litres";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public LitresExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        #region IExternalServiceProvider

        public async Task<LitresBookModel> ExtractBook(string uri)
        {
            var model = new LitresBookModel();

            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri));

            //biblio_book_top block_table
            var divInfo1 = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("biblio_book_top") && n.HasClass("block_table"));

            if (divInfo1 != null)
            {
                #region Extract book's cover image

                var imageUri = ExtractImage(divInfo1);

                model.ImageUri = imageUri;

                #endregion

                #region Extract book's title

                model.Title = ExtractBookTitle(divInfo1);

                #endregion

                #region Extract book's author

                model.Author = ExtractBookAuthor(divInfo1);

                #endregion

                #region Extract book's series, Index and Tracking Uri

                var series = ExtractBookSeries(divInfo1).ToList();

                var authorSeries = series.FirstOrDefault(s => s.Index.HasValue);

                model.AuthorSeries = string.IsNullOrWhiteSpace(authorSeries.Name) ? null : authorSeries.Name;
                model.Index = authorSeries.Index.HasValue ? authorSeries.Index.Value : (int?)null;
                model.TrackingUri = string.IsNullOrWhiteSpace(authorSeries.Uri) ? null : authorSeries.Uri;

                var publisherSeries = series.Where(s => !s.Index.HasValue).Select(s => s.Name).ToList();
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

                model.ReleaseYear = ExtractBookReleaseYear(divBiblioBookInfoDetailed);

                #endregion

                #region Extract book's pages count

                model.PagesCount = ExtractBookPagesCount(divBiblioBookInfoDetailed);

                #endregion
            }

            return model;
        }

        public async Task<List<LitresTrackedBookModel>> ExtractBooksToTrack(string uri)
        {
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri));

            var divSerie = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("biblio_series__wrap"));

            var divsBookTitle = divSerie?.SelectNodes(".//div").Where(n => n.HasClass("art-item__name"));

            if (divsBookTitle == null)
                return null;

            var books = new List<LitresTrackedBookModel>();

            int counter = 1;
            foreach (var divBookTitle in divsBookTitle)
            {
                var aTitle = divBookTitle.SelectNodes(".//a")?.FirstOrDefault();

                var title = aTitle?.InnerText.ClearString();

                books.Add(new LitresTrackedBookModel
                {
                    Title = title,
                    Index = counter
                });

                counter++;
            }

            return books;
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

            var imageUri = img?.Attributes["data-src"]?.Value;
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

        private IEnumerable<(string Name, string Uri, int? Index)> ExtractBookSeries(HtmlNode divInfo1)
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

                var aSerie = spanSeries.SelectNodes("./a")?
                    .FirstOrDefault(n => n.HasClass("biblio_book_sequences__link"));

                if (aSerie == null)
                    yield break;

                var href = aSerie.Attributes["href"]?.Value;
                if (!string.IsNullOrWhiteSpace(href))
                    href = ServiceUri.AppendUriPath(href);

                var spanNumber = spanSeries.SelectNodes("./span")?.FirstOrDefault(n => n.HasClass("number"));
                var index = spanNumber?.InnerText.ParseNumber();

                yield return (aSerie.InnerText.Trim(), href, index);
            }
        }

        private static List<string> ExtractBookGenres(HtmlNode divInfo1)
        {
            var divBiblioBookInfo =
                divInfo1?.SelectNodes(".//div")?.FirstOrDefault(d => d.HasClass("biblio_book_info"));
            var liGenres = divBiblioBookInfo?.SelectNodes(".//li")
                ?.FirstOrDefault(n => n.Descendants().FirstOrDefault()?.InnerText == "Жанр:");

            return liGenres?.SelectNodes("./a")?.Select(n => n.InnerText.Trim()).ToList();
        }

        private static string ExtractBookDescription(HtmlNode divInfo2)
        {
            var divBiblioBookDescrPublishers = divInfo2?.SelectNodes(".//div")
                ?.FirstOrDefault(n => n.HasClass("biblio_book_descr_publishers"));

            return divBiblioBookDescrPublishers?.InnerText.Trim();
        }

        private static int? ExtractBookReleaseYear(HtmlNode divBiblioBookInfoDetailed)
        {
            if (divBiblioBookInfoDetailed == null) return null;

            const string key = "Дата написания:";

            var liDatePublished = divBiblioBookInfoDetailed.SelectNodes(".//li")
                ?.FirstOrDefault(n => n.Descendants().First().InnerText == key);

            if (liDatePublished == null) return null;

            var date = Regex.Match(liDatePublished.InnerText, @"\d+").Value;
            if (!int.TryParse(date, out var year))
                return null;
            return year;
        }

        private static int? ExtractBookPagesCount(HtmlNode divBiblioBookInfoDetailed)
        {
            if (divBiblioBookInfoDetailed == null) return null;

            const string key = "Объем:";
            var liPagesCount = divBiblioBookInfoDetailed.SelectNodes(".//li")
                ?.FirstOrDefault(n => n.Descendants().First().InnerText == key);

            if (liPagesCount == null) return null;

            var pagesCountString = Regex.Match(liPagesCount.InnerText, @"\d+").Value;
            int.TryParse(pagesCountString, out var pagesCount);
            return pagesCount;
        }

        #endregion
    }
}