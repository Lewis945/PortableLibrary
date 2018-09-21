using PortableLibrary.Core.Enums;

namespace PortableLibrary.Apps.WebApi.Models
{
    public class LibraryQuery
    {
        public string Name { get; set; }
        public LibraryType Type { get; set; }
    }
}
