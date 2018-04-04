using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

            model.Title = divTitleRu?.InnerText.Trim();

            var divTitleEn = divFirstTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("title-en"));

            model.OriginalTitle = divTitleEn?.InnerText.Trim();

            #endregion

            #region Extract Status

            var divStatus = divFirstTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("status"));

            model.IsComplete = divStatus?.InnerText.Trim().Equals("Завершен");

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
                var match = Regex.Match(imageUriBackgroundStyle, @"background:url\(\'([.+])\'\)");
                var imageUri = match.Groups[0].Value;
                model.ImageUri = imageUri;
                model.ImageByteArray = await GetImageAsByteArray(imageUri);
            }

            #endregion

            #region Genres

//            var divBiblioBookInfo =
//                divInfo1?.SelectNodes(".//div")?.FirstOrDefault(d => d.HasClass("biblio_book_info"));
//            var liGenres = divBiblioBookInfo?.SelectNodes(".//li")
//                ?.FirstOrDefault(n => n.Descendants().First().InnerText == "Жанр:");
//
//            return liGenres?.SelectNodes("./a")?.Select(n => n.InnerText.Trim()).ToList();
            
            var divDetailsPane = divSecondTitleBlock?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("details-pane"));

            const string genreKey = "Жанр:";
            
            var divGenres = divDetailsPane?.SelectNodes(".//div")?
                .FirstOrDefault(n => n.Descendants().First().InnerText.Contains(genreKey));

            if (divGenres != null)
            {
                var genres = new List<string>();
                bool add = false;
                var list = divGenres.Descendants().ToList();
                foreach (var descendant in divGenres.Descendants())
                {
                    if ( descendant.NodeType==HtmlNodeType.Text && descendant.InnerText.Trim().Equals(genreKey))
                    {
                        add = true;
                        continue;
                    }

                    if (add)
                    {
                        if (descendant.NodeType == HtmlNodeType.Element)
                        {
                            genres.Add(descendant.InnerText.Trim());
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            
            #endregion

            return model;
        }

        #endregion
    }
}