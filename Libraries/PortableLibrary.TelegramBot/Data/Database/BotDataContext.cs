using Microsoft.EntityFrameworkCore;
using PortableLibrary.TelegramBot.Data.Database.Entities;

namespace PortableLibrary.TelegramBot.Data.Database
{
    public class BotDataContext : DbContext
    {
        #region .ctor

        public BotDataContext(DbContextOptions<BotDataContext> options)
            : base(options)
        { }

        #endregion

        #region DbSets

        public DbSet<ChatCommandSequenceState> ChatCommandSequencesState { get; set; }

        #endregion
    }
}
