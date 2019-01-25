using PortableLibrary.Core.Enums;

namespace PortableLibrary.Core.SimpleServices.Models
{
    public class LibraryModel
    {
        public string Title { get; set; }
        public LibraryType Type { get; set; }
        public int Items { get; set; }
        public bool Public { get; set; }
    }
}
