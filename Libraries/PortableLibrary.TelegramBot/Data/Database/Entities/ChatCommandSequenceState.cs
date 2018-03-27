using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.TelegramBot.Data.Database.Entities
{
    public class ChatCommandSequenceState
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatCommandSequenceStateId { get; set; }
        public int ParentChatCommandSequenceStateId { get; set; }

        public long ChatId { get; set; }

        public string Command { get; set; }
        public string Language { get; set; }

        public bool IsComplete { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ChatCommandSequenceState ParentChatCommandSequenceState { get; set; }
    }
}
