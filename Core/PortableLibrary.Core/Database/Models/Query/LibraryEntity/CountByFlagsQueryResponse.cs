namespace PortableLibrary.Core.Database.Models.Query.LibraryEntity
{
    public struct CountByFlagsQueryResponse
    {
        public int All { get; set; }
        public int IsPublished { get; set; }
        public int IsFavourite { get; set; }
        public int IsProcessing { get; set; }
        public int IsProcessed { get; set; }
        public int IsProcessingPlanned { get; set; }
        public int IsWaitingToBecomeGlobal { get; set; }
    }
}