using System.Collections.Generic;

namespace PortableLibrary.TelegramBot.Messaging.Mappings.Models
{
    public class SequenceOptionModel
    {
        public string Option { get; set; }
        public List<AliasModel> Aliases { get; set; }
    }
}