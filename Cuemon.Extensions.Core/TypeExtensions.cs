using System;
using System.Collections;
using System.Collections.Generic;

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
            return StringConverter.FromType(source, fullName, excludeGenericArguments);
        }

        /// <summary>
        /// Gets the underlying type code of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose underlying <see cref="TypeCode"/> to get.</param>
        /// <returns>The code of the underlying type, or Empty if <paramref name="type"/> is null.</returns>
        public static TypeCode AsCode(this Type type)
        {
            return TypeCodeConverter.FromType(type);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEqualityComparer(this Type source)
        {
            return TypeUtility.IsEqualityComparer(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparable(this Type source)
        {
            return TypeUtility.IsComparable(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparer(this Type source)
        {
            return TypeUtility.IsComparer(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerable(this Type source)
        {
            return TypeUtility.IsEnumerable(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/></param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsDictionary(this Type source)
        {
            return TypeUtility.IsDictionary(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.; otherwise, <c>false</c>.</returns>
        public static bool IsKeyValuePair(this Type source)
        {
            return TypeUtility.IsKeyValuePair(source);
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
            return TypeUtility.IsNullable(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is an anonymous method (be that in a form of a delegate or lambda expression).
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to determine is an anonymous method.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is an anonymous method; otherwise, <c>false</c>.</returns>
        public static bool IsAnonymousMethod(this Type source)
        {
            return TypeUtility.IsAnonymousMethod(source);
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
            return TypeUtility.IsComplex(source);
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
            return TypeUtility.GetDefaultValue(type);
        }
    }
}