using PortableLibrary.Core.Enums;

namespace PortableLibrary.Core.SimpleServices.Models
{
    public class LibraryListModel
    {
        public string Title { get; set; }
        public LibraryType Type { get; set; }
        
        public int Items { get; set; }
        public int? Published { get; set; }
        public int? Favourits { get; set; }
        public int? Processing { get; set; }
        public int? Processed { get; set; }
        public int? Planned { get; set; }
        public int? AreWaitingToBecomeGlobal { get; set; }

        public bool Public { get; set; }
    }
}