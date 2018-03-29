using PortableLibrary.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortableLibrary.TelegramBot.Services
{
    public interface ILibraryService
    {
        void AddLibrary(string name, LibraryType type);
    }
}
