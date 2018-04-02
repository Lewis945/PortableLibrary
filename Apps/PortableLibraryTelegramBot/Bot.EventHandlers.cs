using PortableLibrary.Core.Enums;
using PortableLibrary.TelegramBot.Processing.Inline;

namespace PortableLibraryTelegramBot
{
    public partial class Bot
    {
        private void RegisterInlineProcessingEventHandlers(InlineCommandProcessor processor)
        {
            processor.OnAddLibrary += OnAddLibraryEventHandler;
            processor.OnEnterLibrary += OnEnterLibraryEventHandler;
            processor.OnExitLibrary += OnExitLibraryEventHandler;
        }
    }
}