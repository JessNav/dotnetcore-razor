using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Razor.Services.Mapping
{
    // A very small reflection-based mapper that maps properties with the same name and compatible types.
    // It's intentionally simple to avoid bringing AutoMapper as a dependency.
    public class SimpleMapper : IMapper
    {
        private readonly ILogger<SimpleMapper>? _logger;
        private readonly IEnumerable<ITypeConverter> _converters;

        public SimpleMapper(IEnumerable<ITypeConverter>? converters = null, ILogger<SimpleMapper>? logger = null)
        {
            _converters = converters ?? Enumerable.Empty<ITypeConverter>();
            _logger = logger;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class, new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // check for a custom converter first
            var srcType = source.GetType();
            var destType = typeof(TDestination);
            var conv = _converters.FirstOrDefault(c => c.CanConvert(srcType, destType));
            if (conv != null)
            {
                var converted = conv.Convert(source, destType);
                if (converted is TDestination td) return td;
            }

            var dest = new TDestination();

            var sourceType = typeof(TSource);

            var sourceProps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destProps = destType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var s in sourceProps)
            {
                var d = Array.Find(destProps, p => p.Name == s.Name && p.CanWrite);
                if (d == null) continue;

                try
                {
                    var value = s.GetValue(source);
                    if (value == null) continue;

                    if (d.PropertyType.IsAssignableFrom(s.PropertyType))
                    {
                        d.SetValue(dest, value);
                    }
                    else
                    {
                        // Try convert for simple types
                        if (IsSimpleType(d.PropertyType) && IsSimpleType(s.PropertyType))
                        {
                            var converted = Convert.ChangeType(value, d.PropertyType);
                            d.SetValue(dest, converted);
                        }
                        else if (IsEnumerableButNotString(s.PropertyType) && IsEnumerableButNotString(d.PropertyType))
                        {
                            // Map collections
                            var srcElementType = GetEnumerableElementType(s.PropertyType) ?? value.GetType().GetElementType();
                            var destElementType = GetEnumerableElementType(d.PropertyType);

                            if (destElementType != null && srcElementType != null)
                            {
                                var srcEnumerable = ((IEnumerable)value).Cast<object>();
                                var listType = typeof(List<>).MakeGenericType(destElementType);
                                var destList = (IList)Activator.CreateInstance(listType)!;

                                foreach (var item in srcEnumerable)
                                {
                                    if (item == null) continue;

                                    if (destElementType.IsAssignableFrom(item.GetType()))
                                    {
                                        destList.Add(item);
                                    }
                                    else if (IsSimpleType(destElementType))
                                    {
                                        destList.Add(Convert.ChangeType(item, destElementType));
                                    }
                                    else
                                    {
                                        // call Map dynamically: Map<srcType,destType>(item)
                                        var mapMethod = this.GetType().GetMethod("Map")?.MakeGenericMethod(item.GetType(), destElementType);
                                        if (mapMethod != null)
                                        {
                                            var mapped = mapMethod.Invoke(this, new[] { item });
                                            destList.Add(mapped);
                                        }
                                    }
                                }

                                // assign to destination property
                                if (d.PropertyType.IsArray)
                                {
                                    var array = Array.CreateInstance(destElementType, destList.Count);
                                    destList.CopyTo(array, 0);
                                    d.SetValue(dest, array);
                                }
                                else if (d.PropertyType.IsAssignableFrom(listType))
                                {
                                    d.SetValue(dest, destList);
                                }
                                else
                                {
                                    // try to create instance of destination collection and add items
                                    var destCollection = Activator.CreateInstance(d.PropertyType);
                                    if (destCollection is IList targetList)
                                    {
                                        foreach (var itm in destList) targetList.Add(itm);
                                        d.SetValue(dest, destCollection);
                                    }
                                    else
                                    {
                                        // last resort: set if assignable from IEnumerable<destElementType>
                                        d.SetValue(dest, destList);
                                    }
                                }
                            }
                        }
                        else if (!IsSimpleType(d.PropertyType) && !IsSimpleType(s.PropertyType))
                        {
                            // Map nested complex object
                            var mapMethod = this.GetType().GetMethod("Map")?.MakeGenericMethod(value.GetType(), d.PropertyType);
                            if (mapMethod != null)
                            {
                                var mapped = mapMethod.Invoke(this, new[] { value });
                                d.SetValue(dest, mapped);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log mapping failures for individual properties, but continue
                    _logger?.LogWarning(ex, "Failed mapping property '{Property}' from {SourceType} to {DestType}", s.Name, sourceType.FullName, destType.FullName);
                }
            }

            return dest;
        }

        private static bool IsSimpleType(Type t)
        {
            if (t.IsPrimitive) return true;
            if (t == typeof(string)) return true;
            if (t == typeof(decimal)) return true;
            if (t == typeof(DateTime)) return true;
            if (t == typeof(Guid)) return true;
            if (t.IsEnum) return true;
            var nullable = Nullable.GetUnderlyingType(t);
            if (nullable != null) return IsSimpleType(nullable);
            return false;
        }

        private static bool IsEnumerableButNotString(Type t)
        {
            return typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string);
        }

        private static Type? GetEnumerableElementType(Type t)
        {
            if (t.IsArray) return t.GetElementType();
            if (t.IsGenericType)
            {
                var args = t.GetGenericArguments();
                if (args.Length == 1) return args[0];
            }
            // try interfaces
            var ienum = t.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            return ienum?.GetGenericArguments()[0];
        }

        // return a new mapper that includes additional converters
        public SimpleMapper WithConverters(params ITypeConverter[] converters)
        {
            var combined = _converters.Concat(converters ?? Enumerable.Empty<ITypeConverter>());
            return new SimpleMapper(combined, _logger);
        }
    }
}
