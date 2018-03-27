using System;
using System.Collections.Generic;
using System.Linq;
using PortableLibrary.TelegramBot.Messaging.Enums;
using PortableLibraryTelegramBot.Messaging.Mappings.Models;

namespace PortableLibraryTelegramBot.Messaging.Mappings
{
    public class CommandMapping
    {
        public Command Command { get; set; }

        public List<AliasModel> Aliases { get; set; }
        public List<SequenceArgumentModel> ArgumentsSequence { get; set; }
        public List<InlineArgumentModel> InlineArguments { get; set; }

        public AliasModel GetAlias(string alias)
        {
            var aliases = ArgumentsSequence.SelectMany(s => s.Options).SelectMany(o => o.Aliases).ToList();
            var aliasModel = aliases.FirstOrDefault(al =>
                string.Equals(al.Alias, alias, StringComparison.InvariantCultureIgnoreCase));
            return aliasModel;
        }

        public SequenceOptionModel GetSequenceOption(AliasModel alias) =>
            ArgumentsSequence.SelectMany(s => s.Options).FirstOrDefault(o => o.Aliases.Contains(alias));
    }
}