using System.Collections.Generic;
using System.Linq;

namespace Razor.Services.Mapping
{
    public static class MappingExtensions
    {
        public static IEnumerable<TDestination> MapList<TSource, TDestination>(this IMapper mapper, IEnumerable<TSource> list)
            where TSource : class
            where TDestination : class, new()
        {
            if (list == null) return Enumerable.Empty<TDestination>();
            return list.Select(item => mapper.Map<TSource, TDestination>(item));
        }

        public static TDestination MapWith<TSource, TDestination>(this IMapper mapper, TSource source, params ITypeConverter[] converters)
            where TSource : class
            where TDestination : class, new()
        {
            if (mapper is SimpleMapper sm && converters != null && converters.Length > 0)
            {
                return sm.WithConverters(converters).Map<TSource, TDestination>(source);
            }

            return mapper.Map<TSource, TDestination>(source);
        }
    }
}
