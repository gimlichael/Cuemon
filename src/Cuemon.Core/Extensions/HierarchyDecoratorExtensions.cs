using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="IHierarchy{T}"/> interface tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class HierarchyDecoratorExtensions
    {
        private static readonly IList<KeyValuePair<Type, string>> ConvertibleTypes = typeof(bool).GetTypeInfo().Assembly.GetTypes().Where(t => t.GetTypeInfo().IsPrimitive).Select(t => new KeyValuePair<Type, string>(t, t.Name.Split('.').Last())).ToList();

        /// <summary>
        /// A formatter implementation that resolves a <see cref="IConvertible"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <returns>A <see cref="IConvertible"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IConvertible UseConvertibleFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var i = decorator.Inner.FindSingleInstance(h => ConvertibleTypes.Select(pair => pair.Value).Contains(h.Instance.Name));
            return Decorator.Enclose(i.Value).ChangeType(ConvertibleTypes.Single(pair => pair.Value == i.Name).Key) as IConvertible;
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <returns>A <see cref="TimeSpan"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static TimeSpan UseTimeSpanFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var ticks = decorator.Inner.FindSingleInstance(h => h.Instance.Name.Equals("Ticks", StringComparison.OrdinalIgnoreCase));
            return ticks == null ? decorator.Inner.UseGenericConverter<TimeSpan>() : TimeSpan.FromTicks(Convert.ToInt64(ticks.Value));
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="Uri"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <returns>A <see cref="Uri"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Uri UseUriFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var uri = decorator.Inner.FindSingleInstance(h => h.Instance.Name.Equals("OriginalString", StringComparison.OrdinalIgnoreCase));
            return uri == null ? decorator.Inner.UseGenericConverter<Uri>() : Decorator.Enclose(uri.Value.ToString()).ToUri();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <returns>A <see cref="DateTime"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        public static DateTime UseDateTimeFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return decorator.Inner.Instance.Type == typeof(DateTime) ? Decorator.Enclose(decorator.Inner.Instance.Value).ChangeTypeOrDefault<DateTime>() : decorator.Inner.UseGenericConverter<DateTime>();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="Guid"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <returns>A <see cref="Guid"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Guid UseGuidFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return UseGenericConverter<Guid>(decorator.Inner);
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="string"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <returns>A <see cref="string"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static string UseStringFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return UseGenericConverter<string>(decorator.Inner);
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="decimal"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <returns>A <see cref="decimal"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static decimal UseDecimalFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return UseGenericConverter<decimal>(decorator.Inner);
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="ICollection"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <param name="valueType">The type of the objects in the collection.</param>
        /// <returns>A <see cref="ICollection"/> of <paramref name="valueType"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static ICollection UseCollection(this IDecorator<IHierarchy<DataPair>> decorator, Type valueType)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var items = decorator.Inner.GetChildren();
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
        /// <param name="decorator">The <see cref="T:IDecorator{IHierarchy{DataPair}}"/> to extend.</param>
        /// <param name="valueTypes">The value types that forms a <see cref="KeyValuePair{TKey,TValue}"/>.</param>
        /// <returns>A <see cref="IDictionary"/> with <see cref="KeyValuePair{TKey,TValue}"/> of <paramref name="valueTypes"/> from the enclosed <see cref="T:IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IDictionary UseDictionary(this IDecorator<IHierarchy<DataPair>> decorator, Type[] valueTypes)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var items = decorator.Inner.GetChildren();
            var dic = typeof(Dictionary<,>).MakeGenericType(valueTypes);
            var dicInstance = Activator.CreateInstance(dic);
            var addMethod = dic.GetMethod("Add");
            foreach (var item in items.ParseDictionaryItem(valueTypes))
            {
                addMethod.Invoke(dicInstance, new[] { item.Key, item.Value });
            }
            return dicInstance as IDictionary;
        }

        private static T UseGenericConverter<T>(this IHierarchy<DataPair> hierarchy)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(hierarchy.Instance.Value.ToString());
        }

        private static IEnumerable<object> ParseCollectionItem(this IEnumerable<IHierarchy<DataPair>> items, Type valueType)
        {
            var valueTypeInfo = valueType.GetTypeInfo();
            if (valueTypeInfo.IsPrimitive)
            {
                return items.Select(i => Decorator.Enclose(i).UseConvertibleFormatter());
            }

            if (valueType == typeof(Uri))
            {
                return items.Select(i => Decorator.Enclose(i).UseUriFormatter());
            }

            if (valueType == typeof(decimal))
            {
                return items.Select(i => Decorator.Enclose(i).UseDecimalFormatter()).Cast<object>();
            }

            if (valueType == typeof(string))
            {
                return items.Select(i => Decorator.Enclose(i).UseStringFormatter());
            }

            if (valueType == typeof(Guid))
            {
                return items.Select(i => Decorator.Enclose(i).UseGuidFormatter()).Cast<object>();
            }

            if (valueType == typeof(DateTime))
            {
                return items.Select(i => Decorator.Enclose(i).UseDateTimeFormatter()).Cast<object>();
            }

            if (valueType == typeof(TimeSpan))
            {
                return items.Select(i => Decorator.Enclose(i).UseTimeSpanFormatter()).Cast<object>();
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
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseConvertibleFormatter()));
            }

            if (valueType == typeof(Uri))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseUriFormatter()));
            }

            if (valueType == typeof(decimal))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseDecimalFormatter()));
            }

            if (valueType == typeof(string))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseStringFormatter()));
            }

            if (valueType == typeof(Guid))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseGuidFormatter()));
            }

            if (valueType == typeof(DateTime))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseDateTimeFormatter()));
            }

            if (valueType == typeof(TimeSpan))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseTimeSpanFormatter()));
            }

            return new Dictionary<object, object>();
        }
    }
}