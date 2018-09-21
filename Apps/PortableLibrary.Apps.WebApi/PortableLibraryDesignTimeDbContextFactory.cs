using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PortableLibrary.Core.Database;
using System;

namespace PortableLibrary.Apps.WebApi
{
    public class PortableLibraryDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PortableLibraryDataContext>
    {
        public PortableLibraryDataContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            var configuration = builder.Build();

            return Create(configuration.GetConnectionString("Default"));
        }

        private PortableLibraryDataContext Create(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<PortableLibraryDataContext>();
            builder.UseSqlServer(connectionString);

            return (PortableLibraryDataContext)Activator.CreateInstance(typeof(PortableLibraryDataContext), builder.Options as object);
        }
    }
}
