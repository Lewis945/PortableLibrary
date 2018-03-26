using System.Collections.Generic;

namespace PortableLibraryTelegramBot.Messaging.Mappings.Models
{
    public class InlineArgumentModel
    {
        public string Language { get; set; }
        public string Arguments { get; set; }
        public List<InlineOptionModel> Options { get; set; }
    }
}