using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace PortableLibrary.Core.Database
{
    public class PortableLibraryDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PortableLibraryDataContext>
    {
        public PortableLibraryDataContext CreateDbContext(string[] args)
        {
            var connectionString = "";

            return Create(connectionString);
        }

        private PortableLibraryDataContext Create(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<PortableLibraryDataContext>();
            //builder.UseSqlServer(connectionString);

            return (PortableLibraryDataContext)Activator.CreateInstance(typeof(PortableLibraryDataContext), builder.Options as object);
        }
    }
}
