using System.Collections.Generic;
using PortableLibraryTelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Messaging.Mappings.Models;

namespace PortableLibraryTelegramBot.Messaging.Mappings
{
    public class CommandMapping : Mapping
    {
        public Command Command { get; set; }

        public List<string> Aliases { get; set; }
        public List<SequenceArgumentModel> SequenceArguments { get; set; }
        public List<InlineArgumentModel> InlineArguments { get; set; }

    }
}
