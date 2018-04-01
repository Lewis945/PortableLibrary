using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.TelegramBot.Data.Database.Entities
{
    public class ChatNavigationState
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatNavigationStateId { get; set; }
        public int? ParentChatNavigationStateId { get; set; }

        public long ChatId { get; set; }

        public string Position { get; set; }
        public string Value { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ChatNavigationState ParentChatNavigationState { get; set; }
    }
}
