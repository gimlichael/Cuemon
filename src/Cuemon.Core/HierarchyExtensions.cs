using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for a hierarchical structure based on <see cref="IHierarchy{T}"/>.
    /// </summary>
    public static class HierarchyExtensions
    {
        /// <summary>
        /// Returns the first node instance that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="node">The node to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IWrapper{T}.Instance"/>  that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.</returns>
        public static T FindFirstInstance<T>(this IHierarchy<T> node, Func<IHierarchy<T>, bool> match)
        {
            return FindInstance(node, match).FirstOrDefault();
        }

        /// <summary>
        /// Returns the only node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node instance is found; this method throws an exception if more than one node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="node">The node to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IWrapper{T}.Instance"/> node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node instance is found.</returns>
        public static T FindSingleInstance<T>(this IHierarchy<T> node, Func<IHierarchy<T>, bool> match)
        {
            return FindInstance(node, match).SingleOrDefault();
        }

        /// <summary>
        /// Retrieves all node instances that match the conditions defined by the function delegate <paramref name="match"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="node">The node to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all node instances that match the conditions defined by the specified predicate, if found.</returns>
        public static IEnumerable<T> FindInstance<T>(this IHierarchy<T> node, Func<IHierarchy<T>, bool> match)
        {
            return Find(node, match).Select(h => h.Instance);
        }

        /// <summary>
        /// Returns the first node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="node">The node to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.</returns>
        public static IHierarchy<T> FindFirst<T>(this IHierarchy<T> node, Func<IHierarchy<T>, bool> match)
        {
            return Find(node, match).FirstOrDefault();
        }

        /// <summary>
        /// Returns the only node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found; this method throws an exception if more than one node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="node">The node to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.</returns>
        public static IHierarchy<T> FindSingle<T>(this IHierarchy<T> node, Func<IHierarchy<T>, bool> match)
        {
            return Find(node, match).SingleOrDefault();
        }

        /// <summary>
        /// Retrieves all nodes that match the conditions defined by the function delegate <paramref name="match"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="node">The node to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all nodes that match the conditions defined by the specified predicate, if found.</returns>
        public static IEnumerable<IHierarchy<T>> Find<T>(this IHierarchy<T> node, Func<IHierarchy<T>, bool> match)
        {
            Validator.ThrowIfNull(node, nameof(node));
            Validator.ThrowIfNull(match, nameof(match));
            return DescendantsAndSelf(node).Where(match);
        }

        /// <summary>
        /// Replace the instance of the <paramref name="node"/> with a <paramref name="replacer"/> delegate.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="node">The node to replace with a new instance.</param>
        /// <param name="replacer">The delegate that will replace the wrapped instance of the <paramref name="node"/>.</param>
        public static void Replace<T>(this IHierarchy<T> node, Action<IHierarchy<T>, T> replacer)
        {
            Validator.ThrowIfNull(node, nameof(node));
            Validator.ThrowIfNull(replacer, nameof(replacer));
            replacer(node, node.Instance);
        }

        /// <summary>
        /// Replace all instances of the <paramref name="nodes"/> with a <paramref name="replacer"/> delegate.
        /// </summary>
        /// <typeparam name="T">The type of the instance that these nodes represents.</typeparam>
        /// <param name="nodes">The sequence of nodes to replace with a new instance.</param>
        /// <param name="replacer">The delegate that will replace all wrapped instances of the <paramref name="nodes"/>.</param>
        public static void ReplaceAll<T>(this IEnumerable<IHierarchy<T>> nodes, Action<IHierarchy<T>, T> replacer)
        {
            Validator.ThrowIfNull(nodes, nameof(nodes));
            Validator.ThrowIfNull(replacer, nameof(replacer));
            foreach (var node in nodes)
            {
                Replace(node, replacer);
            }
        }

        /// <summary>
        /// Returns the root node of the specified <paramref name="node"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="node"/> in the hierarchical structure.</typeparam>
        /// <param name="node">The node that the hierarchical structure represents.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node that represents the root of the specified <paramref name="node"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public static IHierarchy<T> Root<T>(this IHierarchy<T> node)
        {
            Validator.ThrowIfNull(node, nameof(node));
            return node.HasParent ? AncestorsAndSelf(node).FirstOrDefault() : node;
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
        public static IEnumerable<IHierarchy<T>> AncestorsAndSelf<T>(this IHierarchy<T> node)
        {
            Validator.ThrowIfNull(node, nameof(node));
            IList<IHierarchy<T>> result = new List<IHierarchy<T>>(HierarchyUtility.WhileSourceTraversalIsNotNull(node, HierarchyUtility.AncestorsAndSelf));
            return result.Count > 0 ? result.Reverse() : node.Yield();
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
        public static IEnumerable<IHierarchy<T>> DescendantsAndSelf<T>(this IHierarchy<T> node)
        {
            Validator.ThrowIfNull(node, nameof(node));
            return HierarchyUtility.WhileSourceTraversalHasElements(node, HierarchyUtility.DescendantsAndSelf).Reverse(); ;
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
        public static IEnumerable<IHierarchy<T>> SiblingsAndSelf<T>(this IHierarchy<T> node)
        {
            Validator.ThrowIfNull(node, nameof(node));
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
        public static IEnumerable<IHierarchy<T>> SiblingsAndSelfAt<T>(this IHierarchy<T> node, int depth)
        {
            Validator.ThrowIfNull(node, nameof(node));
            Validator.ThrowIfLowerThan(depth, 0, nameof(depth));
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
        public static IHierarchy<T> NodeAt<T>(this IHierarchy<T> node, int index)
        {
            Validator.ThrowIfNull(node, nameof(node));
            Validator.ThrowIfLowerThan(index, 0, nameof(index));
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
        public static IEnumerable<IHierarchy<T>> FlattenAll<T>(this IHierarchy<T> node)
        {
            Validator.ThrowIfNull(node, nameof(node));
            IHierarchy<T> root = AncestorsAndSelf(node).FirstOrDefault();
            return DescendantsAndSelf(root);
        }
    }
}