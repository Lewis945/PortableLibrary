using System.Threading.Tasks;
using PortableLibrary.Core.Enums;

namespace PortableLibrary.TelegramBot.EventHandlers
{
    public delegate Task AddLibraryEventHandler(string name, LibraryType type);
}