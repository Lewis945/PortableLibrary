using System.Collections.Generic;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Book.FantLab
{
    public class FantLabBookModel : IExternalModel
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public List<string> OtherTitles { get; set; }
        
        public List<string> Genres{ get; set; }
        
        public string ImageUri { get; set; }
        public string TrackingUri { get; set; }

        public string Author { get; set; }
        public string Description { get; set; }

        public string Series { get; set; }

        public int? Index { get; set; }
        public int? ReleaseYear { get; set; }
    }
}