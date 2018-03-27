using PortableLibrary.TelegramBot.Messaging.Enums;

namespace PortableLibraryTelegramBot.Messaging.Mappings.Models
{
    public struct InlineOptionModel
    {
        public string Option { get; set; }
        public InlineArgumentOptionsType Type { get; set; }
        public string Value { get; set; }
    }
}