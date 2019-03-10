using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="Type"/> operations easier to work with.
    /// </summary>
    public static class TypeUtility
    {
        private static ConcurrentDictionary<string, bool> ComplexValueTypeLookup { get; } = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to compare with <paramref name="source"/>.</typeparam>
        /// <param name="source">The object to compare with <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is of <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool Is<T>(object source)
        {
            return (source is T);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is not of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to compare with <paramref name="source"/>.</typeparam>
        /// <param name="source">The object to compare with <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is not of <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool IsNot<T>(object source)
        {
            return !Is<T>(source);
        }


        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEqualityComparer(Type source)
        {
            return ContainsInterface(source, typeof(IEqualityComparer), typeof(IEqualityComparer<>));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparable(Type source)
        {
            return ContainsInterface(source, typeof(IComparable), typeof(IComparable<>));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsComparer(Type source)
        {
            return ContainsInterface(source, typeof(IComparer), typeof(IComparer<>));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerable(Type source)
        {
            return ContainsInterface(source, typeof(IEnumerable), typeof(IEnumerable<>));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/></param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>; otherwise, <c>false</c>.</returns>
        public static bool IsDictionary(Type source)
        {
            return ContainsInterface(source, typeof(IDictionary), typeof(IDictionary<,>), typeof(IReadOnlyDictionary<,>));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="source">The source type to check for implements of either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.; otherwise, <c>false</c>.</returns>
        public static bool IsKeyValuePair(Type source)
        {
            return ContainsType(source, typeof(KeyValuePair<,>), typeof(DictionaryEntry));
        }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(Type source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (!source.GetTypeInfo().IsValueType) { return false; }
            return Nullable.GetUnderlyingType(source) != null;
        }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(T source) { return false; }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(T? source) where T : struct { return true; }

        /// <summary>
        /// Gets a sequence of derived types from the <paramref name="source"/> an it's associated <see cref="Assembly"/>.
        /// </summary>
        /// <param name="source">The source type to locate derived types from.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the derived types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetDescendantOrSelfTypes(Type source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            return GetDescendantOrSelfTypes(source, source.GetTypeInfo().Assembly);
        }

        /// <summary>
        /// Gets a sequence of derived types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate derived types from.</param>
        /// <param name="assemblies">The assemblies to search for the <paramref name="source"/>.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the derived types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetDescendantOrSelfTypes(Type source, params Assembly[] assemblies)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (assemblies == null) { throw new ArgumentNullException(nameof(assemblies)); }
            List<Type> derivedTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> assemblyDerivedTypes = ReflectionUtility.GetAssemblyTypes(assembly, null, source);
                foreach (Type derivedType in assemblyDerivedTypes)
                {
                    derivedTypes.Add(derivedType);
                }
            }
            return derivedTypes;
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
        public static Type GetAncestorOrSelf(Type source, Type sourceBaseLimit)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (sourceBaseLimit == null) { throw new ArgumentNullException(nameof(sourceBaseLimit)); }
            if (source == sourceBaseLimit) { return source; }

            Type sourceBase = source.GetTypeInfo().BaseType;
            while (sourceBase != null)
            {
                TypeInfo sourceBaseInfo = sourceBase.GetTypeInfo();
                if (sourceBaseInfo.BaseType == sourceBaseLimit) { break; }
                sourceBase = sourceBaseInfo.BaseType;
            }
            return sourceBase;
        }

        /// <summary>
        /// Gets a sequence of ancestor-or-self types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate ancestor-or-self types from.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the ancestor-or-self types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetAncestorOrSelfTypes(Type source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            List<Type> parentTypes = new List<Type>();
            Type currentType = source;
            while (currentType != null)
            {
                parentTypes.Add(currentType);
                currentType = currentType.GetTypeInfo().BaseType;
            }
            return parentTypes;
        }

        /// <summary>
        /// Gets a sorted (base-to-derived) sequence of ancestor-and-descendant-or-self types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate ancestor-and-descendant-or-self types from.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the ancestor-and-descendant-or-self types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetAncestorAndDescendantsOrSelfTypes(Type source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            return GetAncestorAndDescendantsOrSelfTypes(source, source.GetTypeInfo().Assembly);
        }

        /// <summary>
        /// Gets a sorted (base-to-derived) sequence of ancestor-and-descendant-or-self types from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source type to locate ancestor-and-descendant-or-self types from.</param>
        /// <param name="assemblies">The assemblies to search for the <paramref name="source"/>.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> holding the ancestor-and-descendant-or-self types from the <paramref name="source"/>.</returns>
        public static IEnumerable<Type> GetAncestorAndDescendantsOrSelfTypes(Type source, params Assembly[] assemblies)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (assemblies == null) { throw new ArgumentNullException(nameof(assemblies)); }
            IEnumerable<Type> ancestorOrSelfTypes = GetAncestorOrSelfTypes(source);
            IEnumerable<Type> derivedOrSelfTypes = GetDescendantOrSelfTypes(source, assemblies);
            return derivedOrSelfTypes.Concat(ancestorOrSelfTypes).Distinct().OrderByDescending(new ReferenceComparer<Type>());
        }

        /// <summary>
        /// Determines whether the specified source contains one or more of the target types specified throughout this member's inheritance chain.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="targets">The target interface types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the target types specified throughout this member's inheritance chain; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsInterface(Type source, params Type[] targets)
        {
            return ContainsInterface(source, true, targets);
        }

        /// <summary>
        /// Determines whether the specified source contains one or more of the target types specified throughout this member's inheritance chain.
        /// </summary>
        /// <param name="source">The source object to match against.</param>
        /// <param name="targets">The target interface types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the target types specified throughout this member's inheritance chain; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsInterface(object source, params Type[] targets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return ContainsInterface(source.GetType(), targets);
        }

        /// <summary>
        /// Determines whether the specified source contains one or more of the target types specified.
        /// </summary>
        /// <param name="source">The source object to match against.</param>
        /// <param name="inherit">Specifies whether to search this member's inheritance chain to find the interfaces.</param>
        /// <param name="targets">The target interface types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the target types specified; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsInterface(object source, bool inherit, params Type[] targets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return ContainsInterface(source.GetType(), inherit, targets);
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
        public static bool ContainsInterface(Type source, bool inherit, params Type[] targets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (targets == null) throw new ArgumentNullException(nameof(targets));
            var sourceInterfaces = source.GetTypeInfo().IsInterface ? source.Yield().Concat(source.GetInterfaces()).ToList() : source.GetInterfaces().ToList();
            foreach (Type targetType in targets)
            {
                if (inherit) // search all inheritance chains
                {
                    foreach (Type interfaceType in sourceInterfaces)
                    {
                        if (interfaceType.GetTypeInfo().IsGenericType)
                        {
                            if (targetType == interfaceType.GetGenericTypeDefinition()) { return true; }
                            continue;
                        }
                        if (targetType == interfaceType) { return true; }
                    }
                }
                else // search this type only
                {
                    Type interfaceType = Converter.Parse(sourceInterfaces, InterfaceParser, targetType);
                    if (interfaceType != null)
                    {
                        if (interfaceType.GetTypeInfo().IsGenericType)
                        {
                            if (targetType == interfaceType.GetGenericTypeDefinition()) { return true; }
                        }
                        if (targetType == interfaceType) { return true; }
                    }
                }
            }
            return false;
        }

        private static Type InterfaceParser(IEnumerable<Type> types, Type targetType)
        {
            foreach (var i in types)
            {
                if (i.Name.Equals(targetType.Name, StringComparison.OrdinalIgnoreCase)) { return i; }
            }
            return null;
        }

        /// <summary>
        /// Determines whether the specified source object contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The source object to match against.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source object contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAttributeType(object source, params Type[] targets)
        {
            return ContainsAttributeType(source, false, targets);
        }

        /// <summary>
        /// Determines whether the specified source object contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The source object to match against.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="source"/>  inheritance chain to find the attributes; otherwise, <c>false</c>.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source object contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAttributeType(object source, bool inherit, params Type[] targets)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            return ContainsAttributeType(source.GetType(), inherit, targets);
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source type contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAttributeType(Type source, params Type[] targets)
        {
            return ContainsAttributeType(source, false, targets);
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
        public static bool ContainsAttributeType(Type source, bool inherit, params Type[] targets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (targets == null) throw new ArgumentNullException(nameof(targets));
            foreach (Type targetType in targets)
            {
                if (source.GetTypeInfo().GetCustomAttributes(targetType, inherit).Any()) { return true; }
            }

            foreach (MemberInfo member in source.GetMembers())
            {
                if (ContainsAttributeType(member, inherit, targets)) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The member to match against.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified member contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAttributeType(MemberInfo source, params Type[] targets)
        {
            return ContainsAttributeType(source, false, targets);
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
        public static bool ContainsAttributeType(MemberInfo source, bool inherit, params Type[] targets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (targets == null) throw new ArgumentNullException(nameof(targets));
            foreach (Type targetType in targets)
            {
                if (source.GetCustomAttributes(targetType, inherit)?.Any() ?? false) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified collection of source objects contains one or more of the specified target types.
        /// </summary>
        /// <param name="sources">The collection of source objects to match against.</param>
        /// <param name="targets">The target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection of source objects contains one more of the target types specified; otherwise, <c>false</c>.
        /// </returns>
        private static bool ContainsType(IEnumerable sources, params Type[] targets)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));
            foreach (object source in sources)
            {
                if (ContainsType(source.GetType(), targets)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified target types.
        /// </summary>
        /// <param name="source">The source type to match against.</param>
        /// <param name="targets">The target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the specified target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsType(Type source, params Type[] targets)
        {
            if (targets == null) throw new ArgumentNullException(nameof(targets));
            foreach (Type targetType in targets)
            {
                Type sourceTypeCopy = source;
                while (sourceTypeCopy != typeof(object) && sourceTypeCopy != null) // recursively loop through all inheritance types of the source
                {
                    if (sourceTypeCopy.GetTypeInfo().IsGenericType)
                    {
                        if (targetType == sourceTypeCopy.GetGenericTypeDefinition()) { return true; }
                    }
                    if (sourceTypeCopy == targetType) // we have a matching type as specified in parameter targetTypes
                    {
                        return true;
                    }
                    sourceTypeCopy = sourceTypeCopy.GetTypeInfo().BaseType; // get the inheriting type
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified source contains one or more of the specified target types.
        /// </summary>
        /// <param name="source">The source object to match against.</param>
        /// <param name="targets">The target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the specified target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsType(object source, params Type[] targets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return ContainsType(source.GetType(), targets);
        }

        /// <summary>
        /// Determines whether the specified source/collection of source object(s) contains one or more of the specified target types.
        /// </summary>
        /// <param name="source">The source object to match against.</param>
        /// <param name="treatSourceAsEnumerable">if set to <c>true</c> the source object is cast as an <see cref="IEnumerable"/> object, and the actual matching is now done against the source objects within the collection against the target types specified.</param>
        /// <param name="targets">The target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source contains one or more of the specified target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsType(object source, bool treatSourceAsEnumerable, params Type[] targets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return treatSourceAsEnumerable ? ContainsType((source as IEnumerable), targets) : ContainsType(source.GetType(), targets);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="sources"/>, as a whole, is determined a complex <see cref="Type"/>.
        /// </summary>
        /// <param name="sources">The <see cref="Type"/> (or types) to determine complexity for.</param>
        /// <returns><c>true</c> if specified <paramref name="sources"/>, as a whole, is a complex <see cref="Type"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources"/> is null.
        /// </exception>
	    public static bool IsComplex(params Type[] sources)
        {
            Validator.ThrowIfNull(sources, nameof(sources));
            bool result = true;
            foreach (var source in sources)
            {
                bool isPrimitive;
                if (!ComplexValueTypeLookup.TryGetValue(source.AssemblyQualifiedName, out isPrimitive))
                {
                    TypeInfo sourceInfo = source.GetTypeInfo();
                    if (sourceInfo.IsGenericType)
                    {
                        var generics = source.GetGenericArguments().ToList();
                        if (sourceInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            return IsComplex(generics[0]);
                        }
                        return IsComplex(generics.ToArray());
                    }
                    isPrimitive = sourceInfo.IsPrimitive;
                    isPrimitive |= sourceInfo.IsEnum;
                    isPrimitive |= sourceInfo.IsValueType && IsSimpleValueType(source);
                    isPrimitive |= source == typeof(string);
                    isPrimitive |= source == typeof(decimal);
                    ComplexValueTypeLookup.AddIfNotContainsKey(source.AssemblyQualifiedName, isPrimitive);
                }
                result &= isPrimitive;
            }
            return !result;
        }

        private static bool IsSimpleValueType(Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            bool simple = source.GetTypeInfo().IsPrimitive;
            if (!simple)
            {
                var constructors = source.GetConstructors(ReflectionUtility.BindingInstancePublic);
                var propertyNames = source.GetProperties(ReflectionUtility.BindingInstancePublic).Where(p => p.CanRead && !p.IsSpecialName).Select(p => p.Name).ToList();
                foreach (var constructor in constructors)
                {
                    var arguments = constructor.GetParameters()?.Select(p => p.Name).ToList();
                    if (arguments != null)
                    {
                        var match = arguments.Intersect(propertyNames, StringComparer.OrdinalIgnoreCase).ToList();
                        if (arguments.Count == match.Count)
                        {
                            return true;
                        }
                    }
                }

                var staticMethods = source.GetMethods(ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic).Where(info => info.ReturnType == source && info.IsStatic).ToList();
                foreach (var staticMethod in staticMethods)
                {
                    var parameters = staticMethod.GetParameters()?.Select(p => p.Name).ToList();
                    if (parameters != null)
                    {
                        var match = parameters.Intersect(propertyNames, StringComparer.OrdinalIgnoreCase).ToList();
                        if (parameters.Count == match.Count)
                        {
                            return true;
                        }
                    }
                }
            }
            return simple;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> has a default constructor.
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to determine is with default constructor.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a default constructor; otherwise, <c>false</c>.</returns>
        public static bool IsWithDefaultConstructor(Type source)
        {
            return !IsWithoutDefaultConstructor(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> does not have a default constructor.
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to determine is without a default constructor.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> does not have a default constructor; otherwise, <c>false</c>.</returns>
        public static bool IsWithoutDefaultConstructor(Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return (!source.GetTypeInfo().IsValueType || source.GetConstructor(Type.EmptyTypes) == null);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is an anonymous method (be that in a form of a delegate or lambda expression).
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to determine is an anonymous method.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is an anonymous method; otherwise, <c>false</c>.</returns>
        public static bool IsAnonymousMethod(Type source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return (source.HasAttributes(typeof(CompilerGeneratedAttribute)) && source.Name.StartsWith("<>"));
        }

        /// <summary>
        /// Gets the default value of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to retrieve its default value from.</param>
        /// <returns>The default value of <paramref name="type"/>.</returns>
        public static object GetDefaultValue(Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            if (type.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(type) == null) { return Activator.CreateInstance(type); }
            return null;
        }
    }
}