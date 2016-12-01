using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
        public IHierarchy<T> this[int index] => this.NodeAt(index);

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
            Type instanceType = instance.GetType();
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

            Hierarchy<T> child = new Hierarchy<T>();
            child.Instance = instance;
            child.InstanceType = instanceType;
            child.Parent = this;
            child.Depth = Depth + 1;
            child.Index = CalculateIndex(this);
            child.IsNew = false;
            child.MemberReference = member;
            Children.Add(Children.Count, child);
            return child;
        }

        private static int CalculateIndex(Hierarchy<T> newItem)
        {
            IHierarchy<T> rootItem = newItem.Root();
            IEnumerable<IHierarchy<T>> allItems = rootItem.DescendantsAndSelf();
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
            StringBuilder path = new StringBuilder();
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
}