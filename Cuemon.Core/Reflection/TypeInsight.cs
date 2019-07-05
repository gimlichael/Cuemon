using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.Collections.Generic;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Provides a robust way of working with reflection related operations grouped together.
    /// </summary>
    public class TypeInsight
    {
        private static ConcurrentDictionary<string, bool> ComplexValueTypeLookup { get; } = new ConcurrentDictionary<string, bool>();
        private readonly Type _type;

        public static implicit operator TypeInsight(Type type)
        {
            return new TypeInsight(type);
        }

        public static implicit operator Type(TypeInsight ti)
        {
            return ti._type;
        }

        public static TypeInsight FromType<T>()
        {
            return new TypeInsight(typeof(T));
        }

        public static TypeInsight FromType(Type type)
        {
            return type;
        }

        public static TypeInsight FromInstance(object instance)
        {
            Validator.ThrowIfNull(instance, nameof(instance));
            return new TypeInsight(instance.GetType());
        }

        public TypeInsight(Type type)
        {
            Validator.ThrowIfNull(type, nameof(type));
            _type = type;
        }

        public PropertyInsight When(Func<Type, PropertyInfo> selector)
        {
            return When(selector, out _);
        }

        public PropertyInsight When(Func<Type, PropertyInfo> selector, out PropertyInfo pi)
        {
            Validator.ThrowIfNull(selector, nameof(selector));
            pi = selector(_type);
            return PropertyInsight.FromProperty(pi);
        }

        public MethodInsight When(Func<Type, MethodInfo> selector)
        {
            return When(selector, out _);
        }

        public MethodInsight When(Func<Type, MethodInfo> selector, out MethodInfo mi)
        {
            Validator.ThrowIfNull(selector, nameof(selector));
            mi = selector(_type);
            return MethodInsight.FromMethod(selector(_type));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the underlying <see cref="Type"/> of this instance is nullable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNullable()
        {
            if (!_type.IsValueType) { return false; }
            return Nullable.GetUnderlyingType(_type) != null;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance suggest an anonymous implementation (be that in a form of a type, delegate or lambda expression).
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance suggest an anonymous implementation; otherwise, <c>false</c>.</returns>
        /// <remarks>If you can avoid it, don't use this method. It is, to say the least, fragile.</remarks>
        public bool IsAnonymous()
        {
            return (_type.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) != null && _type.IsClass && _type.IsSealed && _type.BaseType == typeof(object));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance has a default constructor.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance has a default constructor; otherwise, <c>false</c>.</returns>
        public bool HasDefaultConstructor()
        {
            return (_type.GetTypeInfo().IsValueType || _type.GetConstructor(Type.EmptyTypes) != null);
        }


        /// <summary>
        /// Gets the default value of the underlying <see cref="Type"/> of this instance.
        /// </summary>
        /// <returns>The default value of underlying <see cref="Type"/> of this instance.</returns>
        /// <remarks>Usage is intented for <c>structs</c>.</remarks>
        public object GetDefaultValue()
        {
            if (_type.IsValueType && Nullable.GetUnderlyingType(_type) == null) { return Activator.CreateInstance(_type); }
            return null;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements one or more of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The interface types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the underlying <see cref="Type"/> of this instance implements one or more of the specified <paramref name="types"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool HasInterface(params Type[] types)
        {
            Validator.ThrowIfNull(types, nameof(types));
            var si = _type.IsInterface ? Arguments.Yield(_type).Concat(_type.GetInterfaces()).ToList() : _type.GetInterfaces().ToList();
            foreach (var ti in types.Where(t => t.IsInterface))
            {
                foreach (var i in si)
                {
                    if (i.IsGenericType) { if (ti == i.GetGenericTypeDefinition()) { return true; } }
                    if (ti == i) { return true; }
                }
            }
            return false;
        }


        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements one or more of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the underlying <see cref="Type"/> of this instance implements one or more of the specified <paramref name="types"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool HasType(params Type[] types)
        {
            Validator.ThrowIfNull(types, nameof(types));
            foreach (var tt in types)
            {
                var st = _type;
                while (st != typeof(object) && st != null)
                {
                    if (st.IsGenericType) { if (tt == st.GetGenericTypeDefinition()) { return true; } }
                    if (st == tt) { return true; }
                    st = st.GetTypeInfo().BaseType;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements one or more of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The attribute types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the underlying <see cref="Type"/> of this instance implements one or more of the specified <paramref name="types"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAttribute(params Type[] types)
        {
            foreach (var tt in types) { if (_type.GetCustomAttributes(tt, true).Any()) { return true; } }
            foreach (var m in _type.GetMembers())
            {
                foreach (var tt in types) { if (m.GetCustomAttributes(tt, true).Any()) { return true; } }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.; otherwise, <c>false</c>.</returns>
        public bool HasKeyValuePairContract()
        {
            return HasType(typeof(KeyValuePair<,>), typeof(DictionaryEntry));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public bool HasEqualityComparerContract()
        {
            return HasInterface(typeof(IEqualityComparer), typeof(IEqualityComparer<>));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>; otherwise, <c>false</c>.</returns>
        public bool HasComparableContract()
        {
            return HasInterface(typeof(IComparable), typeof(IComparable<>));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>; otherwise, <c>false</c>.</returns>
        public bool HasComparerContract()
        {
            return HasInterface(typeof(IComparer), typeof(IComparer<>));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>; otherwise, <c>false</c>.</returns>
        public bool HasEnumerableContract()
        {
            return HasInterface(typeof(IEnumerable), typeof(IEnumerable<>));
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>; otherwise, <c>false</c>.</returns>
        public bool HasDictionaryContract()
        {
            return HasInterface(typeof(IDictionary), typeof(IDictionary<,>), typeof(IReadOnlyDictionary<,>));
        }


        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance is considered complex in its nature.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance is considered complex in its nature; otherwise, <c>false</c>.</returns>
        public bool IsComplex()
        {
            return IsComplex(_type);
        }

        private bool IsComplex(params Type[] types)
        {
            Validator.ThrowIfNull(types, nameof(types));
            var result = true;
            foreach (var source in types)
            {
                if (!ComplexValueTypeLookup.TryGetValue(source.AssemblyQualifiedName ?? $"{source.Name}|{source.GUID:N}", out var isPrimitive))
                {
                    var sourceInfo = source.GetTypeInfo();
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
                    isPrimitive |= sourceInfo.IsValueType && IsSimple(source);
                    isPrimitive |= source == typeof(string);
                    isPrimitive |= source == typeof(decimal);
                    if (!ComplexValueTypeLookup.ContainsKey(sourceInfo.AssemblyQualifiedName ?? $"{source.Name}|{source.GUID:N}"))
                    {
                        ComplexValueTypeLookup.TryAdd(source.AssemblyQualifiedName ?? $"{source.Name}|{source.GUID:N}", isPrimitive);
                    }
                }
                result &= isPrimitive;
            }
            return !result;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="Type"/> of this instance is considered simple in its nature.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="Type"/> of this instance is considered simple in its nature; otherwise, <c>false</c>.</returns>
        public bool IsSimple()
        {
            return IsSimple(_type);
        }

        private bool IsSimple(Type source)
        {
            if (source == null) { source = _type; }
            var simple = source.GetTypeInfo().IsPrimitive;
            if (!simple)
            {
                var constructors = source.GetConstructors(new MemberReflection(true, true));
                var propertyNames = source.GetProperties(new MemberReflection(true, true)).Where(p => p.CanRead && !p.IsSpecialName).Select(p => p.Name).ToList();
                foreach (var constructor in constructors)
                {
                    var arguments = constructor.GetParameters().Select(p => p.Name).ToList();
                    var match = arguments.Intersect(propertyNames, StringComparer.OrdinalIgnoreCase).ToList();
                    if (arguments.Count == match.Count)
                    {
                        return true;
                    }
                }

                var staticMethods = source.GetMethods(new MemberReflection(excludeInheritancePath: true)).Where(info => info.ReturnType == source && info.IsStatic).ToList();
                foreach (var staticMethod in staticMethods)
                {
                    var parameters = staticMethod.GetParameters().Select(p => p.Name).ToList();
                    var match = parameters.Intersect(propertyNames, StringComparer.OrdinalIgnoreCase).ToList();
                    if (parameters.Count == match.Count)
                    {
                        return true;
                    }
                }
            }
            return simple;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> (which type must be the same as the underlying <see cref="Type"/> of this instance) has a circular reference.
        /// </summary>
        /// <param name="source">The source to check for circular reference.</param>
        /// <param name="maxDepth">The maximum depth to traverse of <paramref name="source"/>.</param>
        /// <param name="valueResolver">The function delegate that is invoked when a property can be read and is of same type as the underlying <see cref="Type"/> of this instance.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
        public bool HasCircularReference(object source, int maxDepth = 2, Func<object, PropertyInfo, object> valueResolver = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfLowerThanOrEqual(maxDepth, 0, nameof(maxDepth));
            if (source.GetType() != _type) { throw new InvalidOperationException("The specified object has a different type than the underlying type of this instance."); }
            if (valueResolver == null) { valueResolver = Infrastructure.DefaultPropertyValueResolver; }
            var hasCircularReference = false;
            var currentDepth = 0;
            var stack = new Stack<object>();
            stack.Push(source);
            while (stack.Count != 0 && currentDepth <= maxDepth)
            {
                var current = stack.Pop();
                foreach (var property in _type.GetProperties())
                {
                    if (property.CanRead && property.PropertyType == _type)
                    {
                        var propertyValue = valueResolver(current, property);
                        if (propertyValue != null)
                        {
                            stack.Push(propertyValue);
                            hasCircularReference = currentDepth == maxDepth;
                        }
                    }
                }
                currentDepth++;
            }
            return hasCircularReference;
        }

        /// <summary>
        /// Gets a collection (derived-to-self) of derived / descendant types from the underlying <see cref="Type"/> of this instance.
        /// </summary>
        /// <param name="assemblies">The assemblies to include in the search of derived types.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> that contains the derived types from the underlying <see cref="Type"/> of this instance.</returns>
        public IEnumerable<Type> DerivedTypes(params Assembly[] assemblies)
        {
            var ac = new List<Assembly>(assemblies ?? Enumerable.Empty<Assembly>());
            if (!ac.Contains(_type.Assembly)) { ac.Add(_type.Assembly); }
            var dt = new List<Type>();
            foreach (var a in ac)
            {
                dt.AddRange(AssemblyInsight.FromAssembly(a).GetTypes(typeFilter: _type));
            }
            return dt;
        }

        /// <summary>
        /// Gets a collection (self-to-inherited) of inherited / ancestor types from the underlying <see cref="Type"/> of this instance.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Type}"/> that contains the inherited types from the underlying <see cref="Type"/> of this instance.</returns>
        public IEnumerable<Type> InheritedTypes()
        {
            var pt = new List<Type>();
            var ct = _type;
            while (ct != null)
            {
                pt.Add(ct);
                ct = ct.GetTypeInfo().BaseType;
            }
            return pt;
        }


        /// <summary>
        /// Gets a sorted collection (derived-to-self-to-inherited) of inherited / ancestor and derived / descendant types from the underlying <see cref="Type"/> of this instance.
        /// </summary>
        /// <param name="assemblies">The assemblies to include in the search of derived types.</param>
        /// <returns>An <see cref="IEnumerable{Type}"/> that contains a sorted (base-to-derived) collection of inherited and derived types from the underlying <see cref="Type"/> of this instance.</returns>
        public IEnumerable<Type> DerivedAndInheritedTypes(params Assembly[] assemblies)
        {
            var ancestor = InheritedTypes();
            var descendant = DerivedTypes(assemblies);
            return descendant.Concat(ancestor).Distinct().OrderByDescending(t => t, new ReferenceComparer<Type>());
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return _type.ToString();
        }
    }
}