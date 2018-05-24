using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book.EReading
{
    public class EReadingBookModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Serie { get; set; }
        public string Description { get; set; }

        public int? ReleaseYear { get; set; }
        public int? Index { get; set; }

        public List<string> Genres { get; set; }

        public string ImageUri { get; set; }
        public string TrackingUri { get; set; }

        public List<(string Type, string Uri)> DownloadLinks { get; set; }
    }
}