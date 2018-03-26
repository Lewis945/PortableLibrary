using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Messaging.Mappings.Models;
using System.Collections.Generic;

namespace PortableLibraryTelegramBot.Messaging.Mappings
{
    public class MessageMapping
    {
        public Message Type { get; set; }
        public List<AliasModel> Aliases { get; set; }
    }
}
