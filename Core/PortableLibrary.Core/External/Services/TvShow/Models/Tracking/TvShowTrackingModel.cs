using System;
using System.Collections.Generic;
using PortableLibrary.Core.External.Services.Base.Model;

namespace PortableLibrary.Core.External.Services.TvShow.Models.Tracking
{
    public class TvShowTrackingModel : IItemTrackingModel
    {
        public int? TotalSeasons { get; set; }
        public TvShowStatus? Status { get; set; }
        public DateTimeOffset? Started { get; set; }
        public DateTimeOffset? Ended { get; set; }
        public int? Year { get; set; }

        public List<TvShowSeasonTrackingModel> Seasons { get; set; }
    }
}