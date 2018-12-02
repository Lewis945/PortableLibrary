using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow
{
    public abstract class TvShowExternalProviderTestsBase
    {
        #region Protected Methods

        protected static void ValidateTvShow(TvShowDataExtractionModel tvShowModel, string title,
            string originalTitle, string imageUri, TvShowStatus? status, List<string> genres, string description,
            int? seasonsCount = null)
        {
            Assert.Equal(imageUri, tvShowModel.ImageUri, true);

            Assert.Equal(new List<string> { title }, tvShowModel.Titles);
            Assert.Equal(originalTitle, tvShowModel.TitleOriginal, true);

            Assert.Equal(TvShowStatus.CanceledOrEnded, tvShowModel.Status);

            Assert.Equal(genres, tvShowModel.Genres);

            //            Assert.Collection(tvShowModel.Genres,
            //                item => Assert.Equal("Комедия", item, true),
            //                item => Assert.Equal("Мистика", item, true),
            //                item => Assert.Equal("Фантастика", item, true),
            //                item => Assert.Equal("Детектив", item, true)
            //            );

            string modelDescription = Regex.Replace(tvShowModel.Description, @"\t|\n|\r|\s", string.Empty);
            description = Regex.Replace(description, @"\t|\n|\r|\s", string.Empty);

            Assert.Equal(description, modelDescription, true);

            if (seasonsCount.HasValue)
            {
                Assert.NotNull(tvShowModel.Seasons);
                Assert.Equal(seasonsCount.Value, tvShowModel.Seasons.Count);
            }
            else
            {
                Assert.Empty(tvShowModel.Seasons);
            }
        }

        protected static void ValidateSeason(TvShowSeasonDataExtractionModel season,
            int? episodesCount = null, int? specialsCount = null)
        {
            int? total = null;

            if (episodesCount.HasValue)
            {
                Assert.NotNull(season.Episodes);
                Assert.Equal(episodesCount.Value, season.Episodes.Count);
                total = episodesCount.Value;
            }
            else
            {
                Assert.Null(season.Episodes);
            }

            if (specialsCount.HasValue)
            {
                Assert.NotNull(season.Specials);
                Assert.Equal(specialsCount.Value, season.Specials.Count);
                total = !total.HasValue ? specialsCount.Value : total + specialsCount.Value;
            }
            else
            {
                Assert.Null(season.Specials);
            }

            if (total.HasValue)
                Assert.Equal(total.Value, season.TotalEpisodesCount);
            else
                Assert.Null(season.TotalEpisodesCount);
        }

        protected static void ValidateEpisode(TvShowEpisodeDataExtractionModel episode, string title,
            string originalTitle, string shortName = "-1", string image = "-1", DateTime? dateReleasedOrigianl = null,
            DateTime? dateReleased = null, int? id = null)
        {
            if (id.HasValue)
                Assert.Equal(id.Value, episode.Id);
            else
                Assert.Null(episode.Id);

            Assert.Equal(new List<string> { title }, episode.Titles);
            Assert.Equal(originalTitle, episode.OriginalTitle, true);

            if (shortName != "-1")
                Assert.Equal(shortName, episode.ShortName, true);
            else
                Assert.Null(episode.ShortName);

            if (image != "-1")
                Assert.Equal(image, episode.ImageUri, true);
            else
                Assert.Null(episode.ImageUri);

            //if (dateReleasedOrigianl.HasValue)
            //    Assert.Equal(new DateTimeOffset(dateReleasedOrigianl.Value), episode.DateReleasedOrigianl);
            //else
            //    Assert.Null(episode.DateReleasedOrigianl);

            //if (dateReleased.HasValue)
            //    Assert.Equal(new DateTimeOffset(dateReleased.Value), episode.DateReleased);
            //else
            //    Assert.Null(episode.DateReleased);
        }

        #endregion
    }
}