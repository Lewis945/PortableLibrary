namespace PortableLibrary.Core.SimpleServices.Models
{
    public class LibraryExtendedModel : LibraryModel
    {
        public int Published { get; set; }
        public int Favourits { get; set; }
        public int Processing { get; set; }
        public int Processed { get; set; }
        public int Planned { get; set; }
        public int AreWaitingToBecomeGlobal { get; set; }
    }
}
