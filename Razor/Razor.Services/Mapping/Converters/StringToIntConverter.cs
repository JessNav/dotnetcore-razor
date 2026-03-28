using System;

namespace Razor.Services.Mapping.Converters
{
    using Razor.Services.Mapping;

    public class StringToIntConverter : ITypeConverter
    {
        public bool CanConvert(Type sourceType, Type destinationType)
        {
            return sourceType == typeof(string) && (destinationType == typeof(int) || destinationType == typeof(int?));
        }

        public object? Convert(object source, Type destinationType)
        {
            if (source == null) return null;
            if (int.TryParse((string)source, out var v))
            {
                if (destinationType == typeof(int?)) return (int?)v;
                return v;
            }
            return null;
        }
    }
}
