using System;

namespace Razor.Services.Mapping
{
    public interface ITypeConverter
    {
        bool CanConvert(Type sourceType, Type destinationType);
        object? Convert(object source, Type destinationType);
    }
}
