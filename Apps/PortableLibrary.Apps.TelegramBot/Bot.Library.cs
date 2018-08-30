using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Infrastructure.SimpleServices;

namespace PortableLibraryTelegramBot
{
    public partial class Bot
    {
        private async Task OnAddLibraryEventHandler(string name, LibraryType type)
        {
            var options = new DbContextOptionsBuilder<PortableLibraryDataContext>()
                .UseInMemoryDatabase(databaseName: "PortableLibrary")
                .Options;

            using (var context = new PortableLibraryDataContext(options))
            {
                var service = new LibraryService(context);
                var result = await service.AddLibraryAsync(name, type);
            }
        }

        private void OnEnterLibraryEventHandler(string name)
        {
        }

        private void OnExitLibraryEventHandler()
        {
        }
    }
}