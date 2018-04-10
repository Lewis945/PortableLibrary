using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Models.Book
{
    public class ExternalBookModel
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }

        public string ImageUri { get; set; }
        public string TrackingUri { get; set; }

        public string Author { get; set; }
        public string Description { get; set; }

        public string Series { get; set; }

        public int? Index { get; set; }
        public int? ReleaseYear { get; set; }

        public List<string> OtherTitles { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Categories { get; set; }
        public List<(string Type, string Uri)> DownloadLinks { get; set; }
    }
}