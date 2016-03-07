using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Represents a way to expose a node of a hierarchical structure, including the node object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object represented in the hierarchical structure.</typeparam>
    public sealed class Hierarchy<T> : Wrapper<T>, IHierarchy<T>
    {
        private SortedList<int, IHierarchy<T>> _children;
        private IHierarchy<T> _parent;
        private int _depth = 0;
        private int _index = 0;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Hierarchy{T}" /> class.
        /// </summary>
        public Hierarchy()
        {
            this.IsNew = true;
        }
        #endregion

        #region Properties
        private bool IsNew { get; set; }

        /// <summary>
        /// Gets the current depth of the node in the hierarchical structure.
        /// </summary>
        /// <value>The current depth of the in the hierarchical structure.</value>
        public int Depth
        {
            get { return _depth; }
            private set { _depth = value; }
        }

        /// <summary>
        /// Gets the zero-based index of the current node that this hierarchical structure represents.
        /// </summary>
        /// <value>The zero-based index of the current node that this hierarchical structure represents.</value>
        public int Index
        {
            get { return _index; }
            private set { _index = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a parent.
        /// </summary>
        /// <value><c>true</c> if this instance has a parent; otherwise, <c>false</c>.</value>
        public bool HasParent
        {
            get { return (this.Parent != null); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has any children.
        /// </summary>
        /// <value><c>true</c> if this instance has any children; otherwise, <c>false</c>.</value>
        public bool HasChildren
        {
            get { return (this.Children.Count > 0); }
        }

        private SortedList<int, IHierarchy<T>> Children
        {
            get { return _children ?? (_children = new SortedList<int, IHierarchy<T>>()); }
        }

        private IHierarchy<T> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets the node at the specified index.
        /// </summary>
        /// <value>The node at the specified index.</value>
        public IHierarchy<T> this[int index]
        {
            get { return Hierarchy.NodeAt(this, index); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified instance to a node in the hierarchical structure representation.
        /// </summary>
        /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
        /// <returns>A reference to the newly added hierarchical node.</returns>
        public IHierarchy<T> Add(T instance)
        {
            Type instanceType = instance.GetType();
            return this.Add(instance, instanceType);
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
            return this.Add(instance, instanceType, null);
        }

        /// <summary>
        /// Adds the specified instance to a node in the hierarchical structure representation.
        /// </summary>
        /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
        /// <param name="member">The member from where <paramref name="instance"/> was referenced.</param>
        /// <returns>A reference to the newly added hierarchical node.</returns>
        public IHierarchy<T> Add(T instance, MemberInfo member)
        {
            return this.Add(instance, instance.GetType(), member);
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
            if (this.IsNew)
            {
                this.Depth = 0;
                this.Instance = instance;
                this.InstanceType = instanceType;
                this.IsNew = false;
                this.Index = 0;
                this.MemberReference = member;
                return this;
            }

            Hierarchy<T> child = new Hierarchy<T>();
            child.Instance = instance;
            child.InstanceType = instanceType;
            child.Parent = this;
            child.Depth = this.Depth + 1;
            child.Index = CalculateIndex(this);
            child.IsNew = false;
            child.MemberReference = member;
            this.Children.Add(this.Children.Count, child);
            return child;
        }

        private static int CalculateIndex(Hierarchy<T> newItem)
        {
            IHierarchy<T> rootItem = Hierarchy.AncestorsAndSelf(newItem).FirstOrDefault();
            IEnumerable<IHierarchy<T>> allItems = Hierarchy.DescendantsAndSelf(rootItem);
            return allItems.OfType<T>().Count();
        }

        /// <summary>
        /// Gets the hierarchical path of the node in the hierarchical structure.
        /// </summary>
        /// <value>A <see cref="string"/> that identifies the hierarchical path relative to the current node.</value>
        public string GetPath()
        {
            StringBuilder path = new StringBuilder();
            IHierarchy<T> current = this;
            while (current != null && current.Depth >= 0)
            {
                path.Insert(0, ".");
                path.Insert(0, current.InstanceType.Name);
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
            return this.Children.Values;
        }

        /// <summary>
        /// Gets the parent node of the current node in the hierarchical structure.
        /// </summary>
        /// <returns>The parent node of the current node in the hierarchical structure.</returns>
        public IHierarchy<T> GetParent()
        {
            return this.Parent;
        }
        #endregion
    }

    /// <summary>
    /// Provides static helper methods for a hierarchical structure based on <see cref="IHierarchy{T}"/>.
    /// </summary>
    public static class Hierarchy
    {
        #region Methods
        /// <summary>
        /// Returns the root node of the specified <paramref name="node"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node that the hierarchical structure represents.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node that represents the root of the specified <paramref name="node"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public static IHierarchy<T> Root<T>(IHierarchy<T> node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            return AncestorsAndSelf(node).FirstOrDefault();
        }

        /// <summary>
        /// Gets all ancestors (parent, grandparent, etc.) and self of the specified <paramref name="node"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node that the hierarchical structure represents.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to ancestors and self of the specified <paramref name="node"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> AncestorsAndSelf<T>(IHierarchy<T> node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            IList<IHierarchy<T>> result = new List<IHierarchy<T>>(HierarchyUtility.WhileSourceTraversalIsNotNull(node, HierarchyUtility.AncestorsAndSelf));
            return result.Count > 0 ? result.Reverse() : EnumerableUtility.Yield(node);
        }

        /// <summary>
        /// Gets all descendants (children, grandchildren, etc.) anf self of the current <paramref name="node"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node that the hierarchical structure represents.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the descendants and self of the specified <paramref name="node"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> DescendantsAndSelf<T>(IHierarchy<T> node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            return HierarchyUtility.WhileSourceTraversalHasElements(node, HierarchyUtility.DescendantsAndSelf);
        }

        /// <summary>
        /// Gets all siblings and self after the current <paramref name="node"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node that the hierarchical structure represents.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the siblings and self of the specified <paramref name="node"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> SiblingsAndSelf<T>(IHierarchy<T> node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            return SiblingsAndSelfAt(node, node.Depth);
        }

        /// <summary>
        /// Gets all siblings and self after the current <paramref name="node"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node that the hierarchical structure represents.</param>
        /// <param name="depth">The depth in the hierarchical structure from where to locate the siblings and self nodes.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the siblings and self of the specified <paramref name="node"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="depth"/> is less than zero.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> SiblingsAndSelfAt<T>(IHierarchy<T> node, int depth)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (depth < 0) { throw new ArgumentOutOfRangeException(nameof(depth)); }
            IHierarchy<T> root = AncestorsAndSelf(node).FirstOrDefault();
            IEnumerable<IHierarchy<T>> descendantsFromRoot = DescendantsAndSelf(root);
            foreach (IHierarchy<T> descendantItem in descendantsFromRoot)
            {
                if (descendantItem.Depth == depth) { yield return descendantItem; }
            }
        }

        /// <summary>
        /// Returns the node at the specified index of a hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node from which the flattening will begin.</param>
        /// <param name="index">The zero-based index at which a node should be retrieved in the hierarchical structure.</param>
        /// <returns>The node at the specified <paramref name="index"/> in the hierarchical structure.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero - or - <paramref name="index"/> exceeded the count of nodes in the hierarchical structure.
        /// </exception>
        public static IHierarchy<T> NodeAt<T>(IHierarchy<T> node, int index)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            if (index < 0) { throw new ArgumentOutOfRangeException(nameof(index)); }
            if (node.Index == index) { return node; }
            IEnumerable<IHierarchy<T>> allNodes = FlattenAll(node);
            foreach (IHierarchy<T> element in allNodes)
            {
                if (element.Index == index) { return element; }
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        /// <summary>
        /// Flattens the entirety of a hierarchical structure representation into an <see cref="IEnumerable{T}"/> sequence of nodes.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node from which the flattening will begin.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="IHierarchy{T}"/> all nodes represented by the hierarchical structure.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> FlattenAll<T>(IHierarchy<T> node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }
            IHierarchy<T> root = AncestorsAndSelf(node).FirstOrDefault();
            return DescendantsAndSelf(root);
        }
        #endregion
    }
}