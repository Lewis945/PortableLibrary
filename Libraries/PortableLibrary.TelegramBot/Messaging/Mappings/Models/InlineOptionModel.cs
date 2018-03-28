using System.Collections.Generic;
using PortableLibrary.TelegramBot.Messaging.Enums;

namespace PortableLibrary.TelegramBot.Messaging.Mappings.Models
{
    public struct InlineOptionModel
    {
        public string Option { get; set; }
        public InlineArgumentOptionsType Type { get; set; }
        public List<string> Values { get; set; }
    }
}