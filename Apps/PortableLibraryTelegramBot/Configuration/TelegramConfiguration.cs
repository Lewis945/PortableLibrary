using System.Collections.Generic;
using PortableLibraryTelegramBot.Messaging.Mappings;

namespace PortableLibraryTelegramBot.Configuration
{
    public class TelegramConfiguration
    {
        public List<CommandMapping> Commands { get; set; }
        public List<ReplyMapping> Replies { get; set; }
        public List<MessageMapping> Messages { get; set; }
        public List<ErrorMapping> Errors { get; set; }
    }
}