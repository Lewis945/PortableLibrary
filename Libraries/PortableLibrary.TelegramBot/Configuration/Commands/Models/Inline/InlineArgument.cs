using System.Collections.Generic;
using PortableLibrary.TelegramBot.Configuration.Commands.Enums.Inline;

namespace PortableLibrary.TelegramBot.Configuration.Commands.Models.Inline
{
    public class InlineArgument
    {
        public string Argument { get; set; }
        public ArgumentType Type { get; set; }
        public List<ArgumentOption> Options { get; set; }
    }
}