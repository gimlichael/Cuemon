using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Represents a way to expose a node of a hierarchical structure, including the node object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object represented in the hierarchical structure.</typeparam>
    public sealed class Hierarchy<T> : Wrapper<T>, IHierarchy<T>
    {
        private SortedList<int, IHierarchy<T>> _children;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Hierarchy{T}" /> class.
        /// </summary>
        public Hierarchy()
        {
            IsNew = true;
        }
        #endregion

        #region Properties
        private bool IsNew { get; set; }

        /// <summary>
        /// Gets the current depth of the node in the hierarchical structure.
        /// </summary>
        /// <value>The current depth of the in the hierarchical structure.</value>
        public int Depth { get; private set; }

        /// <summary>
        /// Gets the zero-based index of the current node that this hierarchical structure represents.
        /// </summary>
        /// <value>The zero-based index of the current node that this hierarchical structure represents.</value>
        public int Index { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has a parent.
        /// </summary>
        /// <value><c>true</c> if this instance has a parent; otherwise, <c>false</c>.</value>
        public bool HasParent => (Parent != null);

        /// <summary>
        /// Gets a value indicating whether this instance has any children.
        /// </summary>
        /// <value><c>true</c> if this instance has any children; otherwise, <c>false</c>.</value>
        public bool HasChildren => (Children.Count > 0);

        private SortedList<int, IHierarchy<T>> Children => _children ?? (_children = new SortedList<int, IHierarchy<T>>());

        private IHierarchy<T> Parent { get; set; }

        /// <summary>
        /// Gets the node at the specified index.
        /// </summary>
        /// <value>The node at the specified index.</value>
        public IHierarchy<T> this[int index] => Decorator.Enclose(this).NodeAt(index);

        #endregion

        #region Methods
        /// <summary>
        /// Allows for the instance on the current node to be replaced with a new <paramref name="instance" />.
        /// </summary>
        /// <param name="instance">The new instance to replace the original with.</param>
        public void Replace(T instance)
        {
            Replace(instance, instance.GetType());
        }

        /// <summary>
        /// Allows for the instance on the current node to be replaced with a new <paramref name="instance" />.
        /// </summary>
        /// <param name="instance">The new instance to replace the original with.</param>
        /// <param name="instanceType">The type of the new instance.</param>
        public void Replace(T instance, Type instanceType)
        {
            Validator.ThrowIfNull(instanceType, nameof(instanceType));
            Instance = instance;
            InstanceType = instanceType;
        }

        /// <summary>
        /// Adds the specified instance to a node in the hierarchical structure representation.
        /// </summary>
        /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
        /// <returns>A reference to the newly added hierarchical node.</returns>
        public IHierarchy<T> Add(T instance)
        {
            var instanceType = instance.GetType();
            return Add(instance, instanceType);
        }

        /// <summary>
        /// Adds the specified instance to a node in the hierarchical structure representation.
        /// </summary>
        /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
        /// <param name="instanceType">The type of <paramref name="instance"/>.</param>
        /// <returns>A reference to the newly added hierarchical node.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instanceType"/> is null.
        /// </exception>
        public IHierarchy<T> Add(T instance, Type instanceType)
        {
            return Add(instance, instanceType, null);
        }

        /// <summary>
        /// Adds the specified instance to a node in the hierarchical structure representation.
        /// </summary>
        /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
        /// <param name="member">The member from where <paramref name="instance"/> was referenced.</param>
        /// <returns>A reference to the newly added hierarchical node.</returns>
        public IHierarchy<T> Add(T instance, MemberInfo member)
        {
            return Add(instance, instance.GetType(), member);
        }

        /// <summary>
        /// Adds the specified instance to a node in the hierarchical structure representation.
        /// </summary>
        /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
        /// <param name="instanceType">The type of <paramref name="instance"/>.</param>
        /// <param name="member">The member from where <paramref name="instance"/> was referenced.</param>
        /// <returns>A reference to the newly added hierarchical node.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instanceType"/> is null.
        /// </exception>
        public IHierarchy<T> Add(T instance, Type instanceType, MemberInfo member)
        {
            if (instanceType == null) { throw new ArgumentNullException(nameof(instanceType)); }
            if (IsNew)
            {
                Depth = 0;
                Instance = instance;
                InstanceType = instanceType;
                IsNew = false;
                Index = 0;
                MemberReference = member;
                return this;
            }

            var child = new Hierarchy<T>
            {
                Instance = instance,
                InstanceType = instanceType,
                Parent = this,
                Depth = Depth + 1,
                Index = CalculateIndex(this),
                IsNew = false,
                MemberReference = member
            };
            Children.Add(Children.Count, child);
            return child;
        }

        private static int CalculateIndex(Hierarchy<T> newItem)
        {
            var rootItem = Decorator.Enclose(newItem).Root();
            var allItems = Decorator.Enclose(rootItem).DescendantsAndSelf();
            return allItems.Count();
        }

        /// <summary>
        /// Gets the hierarchical path of the node in the hierarchical structure.
        /// </summary>
        /// <returns>A <see cref="string" /> that identifies the hierarchical path relative to the current node.</returns>
        public string GetPath()
        {
            return GetPath(i => i.InstanceType.Name);
        }

        /// <summary>
        /// Gets the hierarchical path of the node in the hierarchical structure.
        /// </summary>
        /// <param name="pathResolver">The function delegate path resolver.</param>
        /// <returns>A <see cref="string" /> that identifies the hierarchical path relative to the current node.</returns>
        public string GetPath(Func<IHierarchy<T>, string> pathResolver)
        {
            var path = new StringBuilder();
            IHierarchy<T> current = this;
            while (current != null && current.Depth >= 0)
            {
                path.Insert(0, ".");
                path.Insert(0, pathResolver(current));
                current = current.GetParent();
            }
            return path.ToString(0, path.Length - 1);
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> sequence that represents all the child nodes of the current hierarchical node.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence that represents all the child nodes of the current hierarchical node.</returns>
        public IEnumerable<IHierarchy<T>> GetChildren()
        {
            return Children.Values;
        }

        /// <summary>
        /// Gets the parent node of the current node in the hierarchical structure.
        /// </summary>
        /// <returns>The parent node of the current node in the hierarchical structure.</returns>
        public IHierarchy<T> GetParent()
        {
            return Parent;
        }
        #endregion
    }

    /// <summary>
    /// Provides a set of static methods for hierarchy releated operations.
    /// </summary>
    public static class Hierarchy
    {
        private const string CircularReferenceKey = "circularReference";
        private const string IndexKey = "index";

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
                    if (Decorator.Enclose(currentType).HasEnumerableImplementation())
                    {
                        if (property.GetIndexParameters().Length > 0) { continue; }
                        if (Decorator.Enclose(currentType).HasDictionaryImplementation() && (property.Name == "Keys" || property.Name == "Values")) { continue; }
                    }

                    var propertyValue = options.ValueResolver(current.Instance, property);
                    if (propertyValue == null) { continue; }
                    index++;
                    result[(int)current.Data[IndexKey]].Add(propertyValue, property);
                    if (Decorator.Enclose(property.PropertyType).IsComplex())
                    {
                        var circularCalls = 0;
                        if (current.Data.ContainsKey(CircularReferenceKey))
                        {
                            circularCalls = (int)current.Data[CircularReferenceKey];
                        }
                        var safetyHashCode = propertyValue.GetHashCode();
                        if (!referenceSafeguards.TryGetValue(safetyHashCode, out var calls)) { referenceSafeguards.Add(safetyHashCode, 0); }
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

        /// <summary>
        /// Invokes the specified <paramref name="traversal"/> path of <paramref name="source"/> until obstructed by a null value.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/>.</typeparam>
        /// <param name="source">The source to travel until the <paramref name="traversal"/> path is obstructed by a null value.</param>
        /// <param name="traversal">The function delegate that is invoked until the traveled path is obstructed by a null value.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the traveled path of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> -or- <paramref name="traversal"/> is null.
        /// </exception>
        public static IEnumerable<TSource> WhileSourceTraversalIsNotNull<TSource>(TSource source, Func<TSource, TSource> traversal) where TSource : class
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (traversal == null) { throw new ArgumentNullException(nameof(traversal)); }
            var stack = new Stack<TSource>();
            stack.Push(traversal(source));
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                if (current == null) { yield break; }
                stack.Push(traversal(current));
                yield return current;
            }
        }

        /// <summary>
        /// Invokes the specified <paramref name="traversal"/> path of <paramref name="source"/> until obstructed by an empty sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/>.</typeparam>
        /// <param name="source">The source to travel until the <paramref name="traversal"/> path is obstructed by an empty sequence.</param>
        /// <param name="traversal">The function delegate that is invoked until the traveled path is obstructed by an empty sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the traveled path of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> -or- <paramref name="traversal"/> is null.
        /// </exception>
        public static IEnumerable<TSource> WhileSourceTraversalHasElements<TSource>(TSource source, Func<TSource, IEnumerable<TSource>> traversal) where TSource : class
        {
            var stack = new Stack<TSource>();
            stack.Push(source);
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                foreach (var element in traversal(current))
                {
                    stack.Push(element);
                }
                yield return current;
            }
        }

        internal static IHierarchy<TSource> AncestorsAndSelf<TSource>(IHierarchy<TSource> source)
        {
            return source.GetParent();
        }

        internal static IEnumerable<IHierarchy<TSource>> DescendantsAndSelf<TSource>(IHierarchy<TSource> source)
        {
            return source.GetChildren();
        }
    }
}