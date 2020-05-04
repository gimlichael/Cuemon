﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Converts the name of the <paramref name="type"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <param name="setup">The <see cref="TypeNameOptions"/> which may be configured.</param>
        /// <returns>A sanitized <see cref="string"/> that represents the <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> cannot be null.
        /// </exception>
        public static string ToFriendlyName(this Type type, Action<TypeNameOptions> setup = null)
        {
            return TypeInsight.FromType(type).ToHumanReadableString(setup);
        }

        /// <summary>
        /// Gets the underlying type code of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns>The code of the underlying type, or Empty if <paramref name="type"/> is null.</returns>
        public static TypeCode ToTypeCode(this Type type)
        {
            return Type.GetTypeCode(type);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEqualityComparer(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).HasEqualityComparerContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparable(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).HasComparableContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparer(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).HasComparerContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerable(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).HasEnumerableContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsDictionary(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).HasDictionaryContract();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.; otherwise, <c>false</c>.</returns>
        public static bool IsKeyValuePair(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).HasKeyValuePairContract();
        }

        /// <summary>
        /// Determines whether the specified type is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).IsNullable();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> suggest an anonymous implementation (be that in a form of a type, delegate or lambda expression).
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> suggest an anonymous implementation; otherwise, <c>false</c>.</returns>
        /// <remarks>If you can avoid it, don't use this method. It is, to say the least, fragile.</remarks>
        public static bool IsAnonymous(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).IsAnonymous();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> is a complex <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if specified <paramref name="type"/> is a complex <see cref="Type"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is null.
        /// </exception>
        public static bool IsComplex(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).IsComplex();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> is a simple <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns><c>true</c> if specified <paramref name="type"/> is a simple <see cref="Type"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is null.
        /// </exception>
        public static bool IsSimple(this Type type)
        {
            return !IsComplex(type);
        }

        /// <summary>
        /// Gets the default value of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns>The default value of <paramref name="type"/>.</returns>
        public static object GetDefaultValue(this Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return TypeInsight.FromType(type).GetDefaultValue();
        }

        /// <summary>
        /// Determines whether the specified type type contains one or more of the specified target types.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <param name="targets">The target types to be matched against.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> contains one or more of the specified target types; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> cannot be null -or-
        /// <paramref name="targets"/> cannot be null.
        /// </exception>
        public static bool HasTypes(this Type type, params Type[] targets)
        {
            Validator.ThrowIfNull(type, nameof(type));
            return Decorator.Enclose(type).HasTypes(targets);
        }

        /// <summary>
        /// Determines whether the specified type contains one or more of the target types specified throughout this member's inheritance chain.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <param name="targets">The target interface types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type contains one or more of the target types specified throughout this member's inheritance chain; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasInterfaces(this Type type, params Type[] targets)
        {
            Validator.ThrowIfNull(type, nameof(type));
            Validator.ThrowIfNull(targets, nameof(targets));
            return TypeInsight.FromType(type).HasInterface(targets);
        }

        /// <summary>
        /// Determines whether the specified type type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type type contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttributes(this Type type, params Type[] targets)
        {
            Validator.ThrowIfNull(type, nameof(type));
            Validator.ThrowIfNull(targets, nameof(targets));
            return TypeInsight.FromType(type).HasAttribute(targets);
        }
    }
}