using System;

namespace PortableLibrary.Core.Database.Entities.Base
{
    public class BaseLibraryEntity : BaseEntity
    {
        public string Comments { get; set; }
        public string CoverImage { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }
        
        public bool IsProcessing { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsProcessingPlanned { get; set; }
        
        public bool IsFavourite { get; set; }
        public bool IsWaitingToBecomeGlobal { get; set; }
    }
}