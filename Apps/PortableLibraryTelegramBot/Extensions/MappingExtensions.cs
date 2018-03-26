using PortableLibraryTelegramBot.Messaging.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PortableLibraryTelegramBot.Extensions
{
    public static class MappingExtensions
    {
        public static T GetMapping<T>(this List<T> mappings, Expression<Func<T, bool>> filter)
            where T : class
        {
            var mapping = mappings.FirstOrDefault(filter.Compile());

            if (mapping == null)
            {
                throw new Exception($"Mapping not found for. Filter: {filter.ToString()} Collection: {mappings.GetType().FullName}");
            }

            return mapping;
        }
    }
}
