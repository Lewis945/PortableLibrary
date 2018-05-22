using System;
using System.Globalization;
using AutoMapper;

namespace PortableLibrary.Core.Automapper
{
    public class DateTimeOffsetTypeConverter : ITypeConverter<string, DateTimeOffset?>
    {
        #region Fields

        private readonly string _pattern;

        #endregion

        #region .ctor

        public DateTimeOffsetTypeConverter()
        {
        }

        public DateTimeOffsetTypeConverter(string pattern)
        {
            _pattern = pattern;
        }

        #endregion

        #region ITypeConverter

        public DateTimeOffset? Convert(string source, DateTimeOffset? destination, ResolutionContext context)
        {
            DateTimeOffset date;
            bool succeed;
            if (!string.IsNullOrWhiteSpace(_pattern))
                succeed = DateTimeOffset.TryParseExact(
                    source,
                    _pattern,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out date);
            else
                succeed = DateTimeOffset.TryParse(
                    source,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out date);

            if (!succeed)
                return null;

            return date;
        }

        #endregion
    }
}