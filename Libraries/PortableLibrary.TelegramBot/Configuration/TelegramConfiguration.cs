using System;
using System.Collections.Generic;
using System.Linq;
using PortableLibrary.TelegramBot.Extensions;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibrary.TelegramBot.Messaging.Mappings;
using PortableLibrary.TelegramBot.Messaging.Mappings.Models;

namespace PortableLibrary.TelegramBot.Configuration
{
    public class TelegramConfiguration
    {
        public List<CommandMapping> Commands { get; set; }
        public List<MessageMapping> GeneralMessages { get; set; }

        public CommandMapping GetCommand(string alias, out AliasModel aliasModel)
        {
            var aliases = Commands.SelectMany(o => o.Aliases);
            var aliasObject = aliases.FirstOrDefault(al =>
                string.Equals(al.Alias, alias, StringComparison.InvariantCultureIgnoreCase));

            if (aliasObject.IsEmpty)
            {
                aliasModel = default;
                return null;
            }

            aliasModel = aliasObject;

            var command = Commands.FirstOrDefault(c => c.Aliases.Contains(aliasObject));
            return command;
        }

        public string GetSequenceOption(Command command, string argument, string option, string language)
        {
            var commandMapping = Commands.GetMapping(m => m.Command == command);
            var alias = commandMapping.Aliases.FirstOrDefault(a => a.Language == language);

            if (alias.IsEmpty)
                throw new Exception($"{nameof(Commands)} is not found for the {language} language.");

            var argumentMapping = commandMapping.ArgumentsSequence.FirstOrDefault(s => s.Argument == argument);
            if (argumentMapping == null)
                throw new Exception($"Argument {argument} is not found.");

            var optionModel = argumentMapping.Options.FirstOrDefault(o =>
                string.Equals(o.Option, option, StringComparison.InvariantCultureIgnoreCase));
            if (optionModel == null)
                throw new Exception($"Option {option} is not found.");

            var aliasModel = optionModel.Aliases.FirstOrDefault(a => a.Language == language);
            if (aliasModel.IsEmpty)
                throw new Exception($"Option's alias is not found for the {language} language.");

            return aliasModel.Alias;
        }

        public string GetGeneralMessage(GeneralMessage type, string language)
        {
            var message = GeneralMessages.GetMapping(m => m.Type == type);
            var alias = message.Aliases.FirstOrDefault(a => a.Language == language);

            if (alias.IsEmpty)
                throw new Exception($"{nameof(GeneralMessages)} is not found for the {language} language.");

            return alias.Alias;
        }
    }
}