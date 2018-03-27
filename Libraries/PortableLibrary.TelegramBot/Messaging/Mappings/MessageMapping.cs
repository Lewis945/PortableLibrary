using PortableLibraryTelegramBot.Messaging.Mappings.Models;
using System.Collections.Generic;
using PortableLibrary.TelegramBot.Messaging.Enums;

namespace PortableLibraryTelegramBot.Messaging.Mappings
{
    public class MessageMapping
    {
        public GeneralMessage Type { get; set; }
        public List<AliasModel> Aliases { get; set; }
    }
}
