namespace PortableLibrary.TelegramBot.Messaging.Mappings.Models
{
    public struct AliasModel
    {
        public string Alias { get; set; }
        public string Language { get; set; }

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(Alias) &&
            string.IsNullOrWhiteSpace(Language);
    }
}