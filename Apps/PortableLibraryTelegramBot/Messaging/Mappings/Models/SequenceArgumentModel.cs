using PortableLibraryTelegramBot.Messaging.Enums;
using System.Collections.Generic;

namespace PortableLibraryTelegramBot.Messaging.Mappings.Models
{
    public class SequenceArgumentModel
    {
        public ArgumentsSequenceType Argument { get; set; }
        public List<SequenceOptionModel> Options { get; set; }
    }
}