using System.Collections.Generic;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Messaging.Mappings.Models;

namespace PortableLibrary.TelegramBot.Messaging.Mappings
{
    public class MessageMapping
    {
        public GeneralMessage Type { get; set; }
        public List<AliasModel> Aliases { get; set; }
    }
}
