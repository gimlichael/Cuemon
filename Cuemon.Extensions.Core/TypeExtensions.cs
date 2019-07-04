using System;
using System.Collections;
using System.Collections.Generic;
using Cuemon.ComponentModel.Converters;
using Cuemon.Reflection;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <paramref name="source"/>; otherwise, <c>false</c> for the simple name of <paramref name="source"/>.</param>
        /// <param name="excludeGenericArguments">Specify <c>true</c> to exclude generic arguments from the result; otherwise <c>false</c> to include generic arguments (should the <paramref name="source"/> be a generic <see cref="Type"/>).</param>
        /// <returns>A sanitized <see cref="string"/> representation of <paramref name="source"/>.</returns>
        public static string ToFriendlyName(this Type source, bool fullName = false, bool excludeGenericArguments = false)
        {
            return ConvertFactory.UseConverter<TypeToStringConverter>().ChangeType(source, o =>
            {
                o.ExcludeGenericArguments = excludeGenericArguments;
                o.FullName = fullName;
            });
        }

        /// <summary>
        /// Gets the underlying type code of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose underlying <see cref="TypeCode"/> to get.</param>
        /// <returns>The code of the underlying type, or Empty if <paramref name="type"/> is null.</returns>
        public static TypeCode AsCode(this Type type)
        {
            return ConvertFactory.UseConverter<TypeCodeConverter>().ChangeType(type);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEqualityComparer(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).HasEqualityComparerContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparable(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).HasComparableContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparer(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).HasComparerContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerable(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).HasEnumerableContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/></param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsDictionary(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).HasDictionaryContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.; otherwise, <c>false</c>.</returns>
        public static bool IsKeyValuePair(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).HasKeyValuePairContract();
        }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).IsNullable();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> suggest an anonymous implementation (be that in a form of a type, delegate or lambda expression).
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to determine is an anonymous type.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> suggest an anonymous implementation; otherwise, <c>false</c>.</returns>
        /// <remarks>If you can avoid it, don't use this method. It is, to say the least, fragile.</remarks>
        public static bool IsAnonymous(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).IsAnonymous();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a complex <see cref="Type"/>.
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to determine complexity for.</param>
        /// <returns><c>true</c> if specified <paramref name="source"/> is a complex <see cref="Type"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static bool IsComplex(this Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return TypeInsight.FromType(source).IsComplex();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a simple <see cref="Type"/>.
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to determine complexity for.</param>
        /// <returns><c>true</c> if specified <paramref name="source"/> is a simple <see cref="Type"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static bool IsSimple(this Type source)
        {
            return !IsComplex(source);
        }

        /// <summary>
        /// Gets the default value of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to retrieve its default value from.</param>
        /// <returns>The default value of <paramref name="type"/>.</returns>
        public static object GetDefaultValue(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).GetDefaultValue();
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified target types.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="targets">The target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the specified target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasTypes(this Type source, params Type[] targets)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(targets, nameof(targets));
            return TypeInsight.FromType(source).HasType(targets);
        }

        /// <summary>
        /// Determines whether the specified source contains one or more of the target types specified throughout this member's inheritance chain.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="targets">The target interface types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the target types specified throughout this member's inheritance chain; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasInterfaces(this Type source, params Type[] targets)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(targets, nameof(targets));
            return TypeInsight.FromType(source).HasInterface(targets);
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source type contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttributes(this Type source, params Type[] targets)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(targets, nameof(targets));
            return TypeInsight.FromType(source).HasAttribute(targets);
        }
    }
}