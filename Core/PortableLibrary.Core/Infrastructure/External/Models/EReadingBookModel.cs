using PortableLibrary.Core.External.Models;
using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Models
{
    public class EReadingBookModel : IExternalModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Serie { get; set; }
        public string Description { get; set; }

        public int? Year { get; set; }

        public List<string> Genres { get; set; }

        public string ImageUri { get; set; }
        public byte[] ImageByteArray { get; set; }

        public List<(string Type, string Uri)> DownloadLinks { get; set; }
        public List<(string Type, byte[] BookByteArray)> DownloadBooks { get; set; }
    }
}