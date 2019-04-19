using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Converts the specified caller information into an instance of a <see cref="MethodBase"/> object.
        /// </summary>
        /// <param name="caller">The <see cref="Type"/> to conduct a search for <paramref name="memberName"/>.</param>
        /// <param name="memberName">The name of the member of <paramref name="caller"/>.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <param name="flags">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <c>null</c>.</returns>
        public static MethodBase ToMethodBase(this Type caller, Type[] types = null, [CallerMemberName] string memberName = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            return MethodBaseConverter.FromType(caller, types, memberName, flags);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <returns>A sanitized <see cref="String"/> representation of <paramref name="source"/>.</returns>
        /// <remarks>Only the simple name of the <paramref name="source"/> is returned, not the fully qualified name.</remarks>
        public static string ToFriendlyName(this Type source)
        {
            return StringConverter.FromType(source);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <paramref name="source"/>; otherwise, <c>false</c> for the simple name of <paramref name="source"/>.</param>
        /// <returns>A sanitized <see cref="String"/> representation of <paramref name="source"/>.</returns>
        public static string ToFriendlyName(this Type source, bool fullName)
        {
            return StringConverter.FromType(source, fullName);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <paramref name="source"/>; otherwise, <c>false</c> for the simple name of <paramref name="source"/>.</param>
        /// <param name="excludeGenericArguments">Specify <c>true</c> to exclude generic arguments from the result; otherwise <c>false</c> to include generic arguments should the <paramref name="source"/> be a generic type.</param>
        /// <returns>A sanitized <see cref="String"/> representation of <paramref name="source"/>.</returns>
        public static string ToFriendlyName(this Type source, bool fullName, bool excludeGenericArguments)
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
        /// Retrieves a collection that represents all the properties defined on a specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to exclude properties on <paramref name="type"/>.</typeparam>
        /// <param name="type">The type that contains the properties to include except those defined on <typeparamref name="T"/>.</param>
        /// <returns>A collection of properties for the specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.</returns>
        public static IEnumerable<PropertyInfo> GetRuntimePropertiesExceptOf<T>(this Type type)
        {
            var baseProperties = typeof(T).GetRuntimeProperties();
            var typeProperties = type.GetRuntimeProperties();
            return typeProperties.Except(baseProperties, DynamicEqualityComparer.Create<PropertyInfo>(pi => $"{pi.Name}-{pi.PropertyType.Name}".GetHashCode32(), (x, y) => x.Name == y.Name && x.PropertyType.Name == y.PropertyType.Name));
        }

        /// <summary>
        /// Converts the <see cref="Type"/> to its equivalent string representation.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns>A string that contains the fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly.</returns>
        public static string ToFullNameIncludingAssemblyName(this Type type)
        {
            return $"{type.FullName}, {type.GetTypeInfo().Assembly.GetName().Name}";
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