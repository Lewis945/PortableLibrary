using System.Collections.Generic;
using PortableLibrary.TelegramBot.Messaging.Enums;

namespace PortableLibraryTelegramBot.Messaging.Mappings.Models
{
    public class SequenceArgumentModel
    {
        public string Argument { get; set; }
        public List<SequenceOptionModel> Options { get; set; }
    }
}