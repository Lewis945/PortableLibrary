using Microsoft.EntityFrameworkCore;
using PortableLibraryTelegramBot.Data.Database.Entities;

namespace PortableLibraryTelegramBot.Data.Database
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
