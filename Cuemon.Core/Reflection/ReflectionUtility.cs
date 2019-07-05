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

                    var propertyValue = options.ValueResolver(current.Instance, property);
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