using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make object hierarchy operations easier to work with.
    /// </summary>
    public static class HierarchyUtility
    {
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