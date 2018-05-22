﻿using System;
using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows
{
    public class MyShowsTvShowModel : IExternalModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleOriginal { get; set; }
        public string Description { get; set; }
        public int? TotalSeasons { get; set; }
        public TvShowStatus Status { get; set; }
        public string Country { get; set; }
        public DateTimeOffset? Started { get; set; }
        public DateTimeOffset? Ended { get; set; }
        public int? Year { get; set; }
        public int? KinopoiskId { get; set; }
        public int? TvrageId { get; set; }
        public int? ImdbId { get; set; }
        public string Image { get; set; }
        public decimal? Rating { get; set; }
        public int? Runtime { get; set; }

        public List<string> Genres { get; set; }

        public List<MyShowsTvShowSeasonModel> Seasons { get; set; }
    }
}