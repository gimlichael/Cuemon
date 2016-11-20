using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="TypeUtility"/> class.
    /// </summary>
    public static class TypeUtilityExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to compare with <paramref name="source"/>.</typeparam>
        /// <param name="source">The object to compare with <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is of <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool Is<T>(this object source)
        {
            return TypeUtility.Is<T>(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is not of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to compare with <paramref name="source"/>.</typeparam>
        /// <param name="source">The object to compare with <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is not of <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool IsNot<T>(this object source)
        {
            return TypeUtility.IsNot<T>(source);
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
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(this T source) { return TypeUtility.IsNullable(source); }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(this T? source) where T : struct { return TypeUtility.IsNullable(source); }

        /// <summary>
        /// Gets a sequence of derived types from the <paramref name="source"/> an it's associated <see cref="Assembly"/>.
        /// </summary>
        /// <param name="source">The source type to locate derived types from.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the derived types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetDescendantsAndSelf(this Type source)
        {
            return TypeUtility.GetDescendantOrSelfTypes(source);
        }

        /// <summary>
        /// Gets a sequence of derived types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate derived types from.</param>
        /// <param name="assemblies">The assemblies to search for the <paramref name="source"/>.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the derived types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetDescendantsAndSelf(this Type source, params Assembly[] assemblies)
        {
            return TypeUtility.GetDescendantOrSelfTypes(source, assemblies);
        }

        /// <summary>
        /// Gets the ancestor-or-self <see cref="Type"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to traverse.</param>
        /// <param name="sourceBaseLimit">The base limit of <paramref name="source"/>.</param>
        /// <returns>The ancestor-or-self type from the specified <paramref name="source"/> that is derived or equal to <paramref name="sourceBaseLimit"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> - or - <paramref name="sourceBaseLimit"/> is null.
        /// </exception>
        public static Type GetAncestorsAndSelf(this Type source, Type sourceBaseLimit)
        {
            return TypeUtility.GetAncestorOrSelf(source, sourceBaseLimit);
        }

        /// <summary>
        /// Gets a sequence of ancestor-or-self types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate ancestor-or-self types from.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the ancestor-or-self types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetAncestorsAndSelf(this Type source)
        {
            return TypeUtility.GetAncestorOrSelfTypes(source);
        }

        /// <summary>
        /// Gets a sorted (base-to-derived) sequence of ancestor-and-descendant-or-self types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate ancestor-and-descendant-or-self types from.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the ancestor-and-descendant-or-self types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetAncestorsAndDescendantsAndSelf(this Type source)
        {
            return TypeUtility.GetAncestorAndDescendantsOrSelfTypes(source);
        }

        /// <summary>
        /// Gets a sorted (base-to-derived) sequence of ancestor-and-descendant-or-self types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate ancestor-and-descendant-or-self types from.</param>
        /// <param name="assemblies">The assemblies to search for the <paramref name="source"/>.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the ancestor-and-descendant-or-self types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetAncestorsAndDescendantsAndSelf(this Type source, params Assembly[] assemblies)
        {
            return TypeUtility.GetAncestorAndDescendantsOrSelfTypes(source, assemblies);
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
            return TypeUtility.ContainsInterface(source, targets);
        }

        /// <summary>
        /// Determines whether the specified source contains one or more of the target types specified.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="inherit">Specifies whether to search this member's inheritance chain to find the interfaces.</param>
        /// <param name="targets">The target interface types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the target types specified; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasInterfaces(this Type source, bool inherit, params Type[] targets)
        {
            return TypeUtility.ContainsInterface(source, inherit, targets);
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
            return TypeUtility.ContainsAttributeType(source, targets);
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="source"/> inheritance chain to find the attributes; otherwise, <c>false</c>.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source type contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttributes(this Type source, bool inherit, params Type[] targets)
        {
            return TypeUtility.ContainsAttributeType(source, inherit, targets);
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The member to match against.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified member contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttributes(this MemberInfo source, params Type[] targets)
        {
            return TypeUtility.ContainsAttributeType(source, targets);
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The member to match against.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="source"/> inheritance chain to find the attributes; otherwise, <c>false</c>.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified member contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttributes(this MemberInfo source, bool inherit, params Type[] targets)
        {
            return TypeUtility.ContainsAttributeType(source, inherit, targets);
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
            return TypeUtility.ContainsType(source, targets);
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