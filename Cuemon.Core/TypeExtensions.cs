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
        /// <exception cref="ArgumentNullException">
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


    }
}