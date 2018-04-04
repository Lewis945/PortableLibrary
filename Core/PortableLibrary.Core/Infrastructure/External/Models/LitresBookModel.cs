using System;
using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Models
{
    public class LitresBookModel : IExternalModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public string ImageUri { get; set; }
        public byte[] ImageByteArray { get; set; }

        public string AuthorSeries { get; set; }

        public int? Index { get; set; }
        public int? PagesCount { get; set; }

        public DateTime? DatePublished { get; set; }

        public List<string> PublishersSeries { get; set; }
        public List<string> Genres { get; set; }
    }
}