using System.Collections.Generic;

namespace PortableLibrary.TelegramBot.Configuration.Commands.Models.Inline
{
    public class ArgumentsLine
    {
        public string Language { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
        public List<InlineArgument> Arguments { get; set; }
    }
}