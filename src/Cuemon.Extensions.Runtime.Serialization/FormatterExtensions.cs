using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Cuemon.Extensions.Runtime.Serialization
{
    /// <summary>
    /// Extension methods for <see cref="Formatter"/> class.
    /// </summary>
    public static class FormatterExtensions
    {
        private static readonly IList<KeyValuePair<Type, string>> ConvertibleTypes = typeof(bool).GetTypeInfo().Assembly.GetTypes().Where(t => t.GetTypeInfo().IsPrimitive).Select(t => new KeyValuePair<Type, string>(t, t.Name.Split('.').Last())).ToList();

        /// <summary>
        /// A formatter implementation that resolves a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <returns>A <see cref="TimeSpan"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static TimeSpan UseTimeSpanFormatter(this IHierarchy<DataPair> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            var ticks = hierarchy.FindSingleInstance(h => h.Instance.Name.Equals("Ticks", StringComparison.OrdinalIgnoreCase));
            return ticks == null ? hierarchy.UseGenericConverter<TimeSpan>() : TimeSpan.FromTicks(Convert.ToInt64(ticks.Value));
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="Uri"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <returns>A <see cref="Uri"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static Uri UseUriFormatter(this IHierarchy<DataPair> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            var uri = hierarchy.FindSingleInstance(h => h.Instance.Name.Equals("OriginalString", StringComparison.OrdinalIgnoreCase));
            return uri == null ? hierarchy.UseGenericConverter<Uri>() : uri.Value.ToString().ToUri();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <returns>A <see cref="DateTime"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static DateTime UseDateTimeFormatter(this IHierarchy<DataPair> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            return hierarchy.Instance.Type == typeof(DateTime) ? hierarchy.Instance.Value.As<DateTime>() : hierarchy.UseGenericConverter<DateTime>();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="IConvertible"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <returns>A <see cref="IConvertible"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static IConvertible UseConvertibleFormatter(this IHierarchy<DataPair> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            var i = hierarchy.FindSingleInstance(h => ConvertibleTypes.Select(pair => pair.Value).Contains(h.Instance.Name));
            return Decorator.Enclose(i.Value).ChangeType(ConvertibleTypes.Single(pair => pair.Value == i.Name).Key) as IConvertible;
        }

        private static T UseGenericConverter<T>(this IHierarchy<DataPair> hierarchy)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(hierarchy.Instance.Value.ToString());
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="Guid"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <returns>A <see cref="Guid"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static Guid UseGuidFormatter(this IHierarchy<DataPair> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            return UseGenericConverter<Guid>(hierarchy);
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="string"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <returns>A <see cref="string"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static string UseStringFormatter(this IHierarchy<DataPair> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            return UseGenericConverter<string>(hierarchy);
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="decimal"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <returns>A <see cref="decimal"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static decimal UseDecimalFormatter(this IHierarchy<DataPair> hierarchy)
        {
            Validator.ThrowIfNull(hierarchy, nameof(hierarchy));
            return UseGenericConverter<decimal>(hierarchy);
        }

        private static IEnumerable<object> ParseCollectionItem(this IEnumerable<IHierarchy<DataPair>> items, Type valueType)
        {
            var valueTypeInfo = valueType.GetTypeInfo();
            if (valueTypeInfo.IsPrimitive)
            {
                return items.Select(i => i.UseConvertibleFormatter());
            }

            if (valueType == typeof(Uri))
            {
                return items.Select(i => i.UseUriFormatter());
            }

            if (valueType == typeof(decimal))
            {
                return items.Select(i => i.UseDecimalFormatter()).Cast<object>();
            }

            if (valueType == typeof(string))
            {
                return items.Select(i => i.UseStringFormatter());
            }

            if (valueType == typeof(Guid))
            {
                return items.Select(i => i.UseGuidFormatter()).Cast<object>();
            }

            if (valueType == typeof(DateTime))
            {
                return items.Select(i => i.UseDateTimeFormatter()).Cast<object>();
            }

            if (valueType == typeof(TimeSpan))
            {
                return items.Select(i => i.UseTimeSpanFormatter()).Cast<object>();
            }

            return new List<object>();
        }

        private static IEnumerable<KeyValuePair<object, object>> ParseDictionaryItem(this IEnumerable<IHierarchy<DataPair>> items, Type[] valueTypes)
        {
            var valueType = valueTypes[1];
            var valueTypeInfo = valueType.GetTypeInfo();
            var dicItems = items.ToDictionary(h => h, h => h.GetChildren().SingleOrDefault());

            if (valueTypeInfo.IsPrimitive)
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), i.Value.UseConvertibleFormatter()));
            }

            if (valueType == typeof(Uri))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), i.Value.UseUriFormatter()));
            }

            if (valueType == typeof(decimal))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), i.Value.UseDecimalFormatter()));
            }

            if (valueType == typeof(string))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), i.Value.UseStringFormatter()));
            }

            if (valueType == typeof(Guid))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), i.Value.UseGuidFormatter()));
            }

            if (valueType == typeof(DateTime))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), i.Value.UseDateTimeFormatter()));
            }

            if (valueType == typeof(TimeSpan))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), i.Value.UseTimeSpanFormatter()));
            }

            return new Dictionary<object, object>();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="ICollection"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <param name="valueType">The type of the objects in the collection.</param>
        /// <returns>A <see cref="ICollection"/> of <paramref name="valueType"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static ICollection UseCollection(this IHierarchy<DataPair> hierarchy, Type valueType)
        {
            var items = hierarchy.GetChildren();
            var list = typeof(List<>).MakeGenericType(valueType);
            var listInstance = Activator.CreateInstance(list);
            var addMethod = list.GetMethod("Add");
            foreach (var item in items.ParseCollectionItem(valueType))
            {
                addMethod.Invoke(listInstance, new[] { item });
            }
            return listInstance as ICollection;
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="IDictionary"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy to parse.</param>
        /// <param name="valueTypes">The value types that forms a <see cref="KeyValuePair{TKey,TValue}"/>.</param>
        /// <returns>A <see cref="IDictionary"/> with <see cref="KeyValuePair{TKey,TValue}"/> of <paramref name="valueTypes"/> from the specified <paramref name="hierarchy"/>.</returns>
        public static IDictionary UseDictionary(this IHierarchy<DataPair> hierarchy, Type[] valueTypes)
        {
            var items = hierarchy.GetChildren();
            var dic = typeof(Dictionary<,>).MakeGenericType(valueTypes);
            var dicInstance = Activator.CreateInstance(dic);
            var addMethod = dic.GetMethod("Add");
            foreach (var item in items.ParseDictionaryItem(valueTypes))
            {
                addMethod.Invoke(dicInstance, new[] { item.Key, item.Value });
            }
            return dicInstance as IDictionary;
        }
    }
}