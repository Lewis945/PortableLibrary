using System;
using System.Collections.Generic;
using System.Linq;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TCompare>(
            this IEnumerable<TSource> source, Func<TSource, TCompare> selector)
        {
            return source.Distinct(new LambdaEqualityComparer<TSource, TCompare>(selector));
        }
    }
}