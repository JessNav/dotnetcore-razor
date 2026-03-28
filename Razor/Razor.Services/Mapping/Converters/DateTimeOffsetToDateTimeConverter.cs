using System;

namespace Razor.Services.Mapping.Converters
{
    using Razor.Services.Mapping;

    public class DateTimeOffsetToDateTimeConverter : ITypeConverter
    {
        public bool CanConvert(Type sourceType, Type destinationType)
        {
            return (sourceType == typeof(DateTimeOffset) || sourceType == typeof(DateTimeOffset?))
                && (destinationType == typeof(DateTime) || destinationType == typeof(DateTime?));
        }

        public object? Convert(object source, Type destinationType)
        {
            if (source == null) return null;
            var dto = (DateTimeOffset)source;
            var dt = dto.DateTime;
            if (destinationType == typeof(DateTime?)) return (DateTime?)dt;
            return dt;
        }
    }
}
