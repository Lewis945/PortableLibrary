using PortableLibrary.Core.Enums;

namespace PortableLibrary.TelegramBot.EventHandlers
{
    public delegate void AddLibraryEventHandler(string name, LibraryType type);
}