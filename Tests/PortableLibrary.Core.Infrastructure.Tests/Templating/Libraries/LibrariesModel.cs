using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.Tests.Templating.Libraries
{
    public class LibrariesModel
    {
        public string Title { get; set; }

        public List<LibraryModel> Libraries { get; set; }
    }
}