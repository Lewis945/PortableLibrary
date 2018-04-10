using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Models.Book
{
    public class EReadingBookModel : IExternalModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Serie { get; set; }
        public string Description { get; set; }

        public int? ReleaseYear { get; set; }

        public List<string> Genres { get; set; }

        public string ImageUri { get; set; }

        public List<(string Type, string Uri)> DownloadLinks { get; set; }
    }
}