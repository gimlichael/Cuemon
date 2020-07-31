using System;
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

        private static T UseGenericConverter<T>(this IHierarchy<DataPair> hierarchy)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(hierarchy.Instance.Value.ToString());
        }
    }
}