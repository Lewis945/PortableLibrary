using System.Collections.Generic;

namespace PortableLibrary.TelegramBot.Messaging.Mappings.Models
{
    public class SequenceArgumentModel
    {
        public string Argument { get; set; }
        public List<SequenceOptionModel> Options { get; set; }
    }
}