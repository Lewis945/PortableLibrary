using PortableLibraryTelegramBot.Configuration;
using PortableLibraryTelegramBot.Extensions;
using System;
using System.Linq;

namespace PortableLibraryTelegramBot.Processing.Sequence
{
    public class BaseCommandSequenceProcessor
    {
        protected TelegramConfiguration Configuration { get; }

        public BaseCommandSequenceProcessor(TelegramConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected static string GenerateKey(params object[] args)
        {
            return args.Length > 0 ? string.Join(".", args.Select(a => a.ToString())) : null;
        }

        protected string GenerateCurrentStateMessage(string language, params object[] args)
        {
            var message = Configuration.GetGeneralMessage(Messaging.Enums.Message.YourCurrentSelection, language);
            return string.Format(message,
                string.Join(" -> ", args.Select(a => GetLocalizedMessage(language, a))));
        }

        protected string GetLocalizedMessage(string language, object value)
        {
            var command = Configuration.Commands.FirstOrDefault(c =>
                string.Equals(c.Command.ToString(), value.ToString(), StringComparison.InvariantCultureIgnoreCase));
            if (command != null)
            {
                var alias = command.Aliases.FirstOrDefault(a => a.Language == language);
                if (alias.IsEmpty)
                    throw new Exception("");

                return alias.Alias;
            }

            var option = Configuration.Commands.SelectMany(c => c.ArgumentsSequence).SelectMany(s => s.Options)
                .FirstOrDefault(o => string.Equals(o.Option, value.ToString(), StringComparison.InvariantCultureIgnoreCase));

            if (option != null)
            {
                var alias = option.Aliases.FirstOrDefault(a => a.Language == language);
                if (alias.IsEmpty)
                    throw new Exception("");

                return alias.Alias;
            }

            return value.ToString();
        }
    }
}
