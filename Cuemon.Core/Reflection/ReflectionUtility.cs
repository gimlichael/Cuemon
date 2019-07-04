using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This utility class is designed to make <see cref="Reflection"/> operations easier to work with.
    /// </summary>
    public static class ReflectionUtility
    {
        private static readonly string CircularReferenceKey = "circularReference";
        private static readonly string IndexKey = "index";

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> has a circular reference.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to check for circular reference.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
        public static bool HasCircularReference<T>(T source) where T : class
        {
            return HasCircularReference(source, 2);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> has a circular reference.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to check for circular reference.</param>
        /// <param name="maxDepth">The maximum depth to traverse of <paramref name="source"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
        public static bool HasCircularReference<T>(T source, int maxDepth) where T : class
        {
            return HasCircularReference(source, maxDepth, DefaultPropertyIndexParametersResolver);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> has a circular reference.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to check for circular reference.</param>
        /// <param name="maxDepth">The maximum depth to traverse of <paramref name="source"/>.</param>
        /// <param name="propertyIndexParametersResolver">The function delegate that is invoked if a property has one or more index parameters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
        public static bool HasCircularReference<T>(T source, int maxDepth, Func<ParameterInfo[], object[]> propertyIndexParametersResolver) where T : class
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (maxDepth <= 0) { throw new ArgumentOutOfRangeException(nameof(maxDepth)); }
            var hasCircularReference = false;
            var currentDepth = 0;
            var stack = new Stack<T>();
            stack.Push(source);
            while (stack.Count != 0 && currentDepth <= maxDepth)
            {
                var current = stack.Pop();
                var currentType = current.GetType();
                foreach (var property in currentType.GetProperties())
                {
                    if (property.CanRead && property.PropertyType == currentType)
                    {
                        var propertyValue = (T)GetPropertyValue(current, property, propertyIndexParametersResolver);
                        stack.Push(propertyValue);
                        hasCircularReference = currentDepth == maxDepth;
                    }
                }
                currentDepth++;
            }
            return hasCircularReference;
        }

        /// <summary>
        /// Gets the property value of a specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source whose property value will be returned.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> to access it's value from.</param>
        /// <returns>The property value of the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="property"/> is null.
        /// </exception>
	    public static object GetPropertyValue(object source, PropertyInfo property)
        {
            return GetPropertyValue(source, property, DefaultPropertyIndexParametersResolver);
        }

        /// <summary>
        /// Gets the property value of a specified <paramref name="source"/> with check for the need of property index values initialized by the specified <paramref name="propertyIndexParametersResolver"/>.
        /// </summary>
        /// <param name="source">The source whose property value will be returned.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> to access it's value from.</param>
        /// <param name="propertyIndexParametersResolver">The function delegate that is invoked if a property has one or more index parameters.</param>
        /// <returns>The property value of the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="property"/> is null - or - <paramref name="propertyIndexParametersResolver"/> is null.
        /// </exception>
        public static object GetPropertyValue(object source, PropertyInfo property, Func<ParameterInfo[], object[]> propertyIndexParametersResolver)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            if (propertyIndexParametersResolver == null) { throw new ArgumentNullException(nameof(propertyIndexParametersResolver)); }
            if (!property.CanRead) { return null; }

            var indexParameters = property.GetIndexParameters();
            if (indexParameters.Length == 0)
            {
                try
                {
                    return property.GetValue(source, null);
                }
                catch (TargetInvocationException) // possible TargetInvocationException for InvalidOperation scenarios and the like of - ignore for now
                {
                }
            }

            var indexValues = propertyIndexParametersResolver(indexParameters);
            if (indexValues != null && indexValues.Length > 0)
            {
                try
                {
                    var currentAsEnumerable = source as IEnumerable;
                    if (currentAsEnumerable != null)
                    {
                        if (!currentAsEnumerable.Cast<object>().Any()) { return null; }
                    }
                    return property.GetValue(source, indexValues); // possible TargetInvocationException as we have no clue what the indexer is (except the assumed one)
                }
                catch (TargetInvocationException)
                {
                }
            }
            return null;
        }

        private static object[] DefaultPropertyIndexParametersResolver(ParameterInfo[] infos)
        {
            var resolvedParameters = new List<object>();
            for (var i = 0; i < infos.Length; i++)
            {
                // because we don't know the values to pass to an indexer we will try to do some assumptions on a "normal" indexer
                // however; this has it flaws: an indexer does not necessarily have an item on 0, 1, 2 etc., so must handle the possible
                // TargetInvocationException.
                // more info? check here: http://blog.nkadesign.com/2006/net-the-limits-of-using-reflection/comment-page-1/#comment-10813
                if (TypeInsight.FromType(infos[i].ParameterType).HasType(typeof(byte), typeof(short), typeof(int), typeof(long))) // check to see if we have a "normal" indexer
                {
                    resolvedParameters.Add(0);
                }
            }
            return resolvedParameters.ToArray();
        }

        /// <summary>
        /// Gets the tree structure of the specified <paramref name="source"/> wrapped in an <see cref="IHierarchy{T}"/> node representing a hierarchical structure.
        /// </summary>
        /// <param name="source">The source whose properties will be traversed while building the hierarchical structure.</param>
        /// <param name="setup">The <see cref="ObjectHierarchyOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node representing the entirety of a hierarchical structure from the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static IHierarchy<object> GetObjectHierarchy(object source, Action<ObjectHierarchyOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            var options = Patterns.Configure(setup);
            IDictionary<int, int> referenceSafeguards = new Dictionary<int, int>();
            var stack = new Stack<Wrapper<object>>();

            var index = 0;
            var maxCircularCalls = options.MaxCircularCalls;

            var current = new Wrapper<object>(source);
            current.Data.Add(IndexKey, index);
            stack.Push(current);

            var result = new Hierarchy<object>();
            result.Add(source);

            while (stack.Count != 0)
            {
                current = stack.Pop();
                var currentType = current.Instance.GetType();
                if (options.SkipPropertyType(currentType))
                {
                    if (index == 0) { continue; }
                    index++;
                    result[(int)current.Data[IndexKey]].Add(current.Instance, current.MemberReference);
                    continue;
                }

                foreach (var property in currentType.GetProperties(new MemberReflection(true, true)))
                {
                    if (options.SkipProperty(property)) { continue; }
                    if (!property.CanRead) { continue; }
                    var reflector = TypeInsight.FromType(currentType);
                    if (reflector.HasEnumerableContract())
                    {
                        if (property.GetIndexParameters().Length > 0) { continue; }
                        if (reflector.HasDictionaryContract())
                        {
                            if (property.Name == "Keys" || property.Name == "Values") { continue; }
                        }
                    }

                    var propertyValue = GetPropertyValue(current.Instance, property, options.PropertyIndexParametersResolver);
                    if (propertyValue == null) { continue; }
                    index++;
                    result[(int)current.Data[IndexKey]].Add(propertyValue, property);
                    if (TypeInsight.FromType(property.PropertyType).IsComplex())
                    {
                        var circularCalls = 0;
                        if (current.Data.ContainsKey(CircularReferenceKey))
                        {
                            circularCalls = (int)current.Data[CircularReferenceKey];
                        }
                        var safetyHashCode = propertyValue.GetHashCode();
                        int calls;
                        if (!referenceSafeguards.TryGetValue(safetyHashCode, out calls)) { referenceSafeguards.Add(safetyHashCode, 0); }
                        if (calls <= maxCircularCalls && result[index].Depth < options.MaxDepth)
                        {
                            referenceSafeguards[safetyHashCode]++;
                            var wrapper = new Wrapper<object>(propertyValue);
                            wrapper.Data.Add(IndexKey, index);
                            wrapper.Data.Add(CircularReferenceKey, circularCalls + 1);
                            stack.Push(wrapper);
                        }
                    }
                }
            }
            return result;
        }

    }
}