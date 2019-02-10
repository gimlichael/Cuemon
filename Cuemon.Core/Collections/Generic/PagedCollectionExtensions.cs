using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This is an extension implementation that provides ways to enable paging on <see cref="IEnumerable{T}"/> sequences.
    /// </summary>
    public static class PagedCollectionExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        /// <remarks>The starting page is set to 1 and the page size is determined by <see cref="PagedSettings.DefaultPageSize"/>.</remarks>
        public static PagedCollection<T> ToPagedCollection<T>(this IEnumerable<T> source)
        {
            return ToPagedCollection(source, 1);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with starting <paramref name="pageNumber"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="pageNumber">The page number to start with.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        /// <remarks>The page size is determined by <see cref="PagedSettings.DefaultPageSize"/>.</remarks>
        public static PagedCollection<T> ToPagedCollection<T>(this IEnumerable<T> source, int pageNumber)
        {
            return ToPagedCollection(source, pageNumber, PagedSettings.DefaultPageSize);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with starting <paramref name="pageNumber"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="pageNumber">The page number to start with.</param>
        /// <param name="pageSize">The number of elements a page can contain.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        public static PagedCollection<T> ToPagedCollection<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            return ToPagedCollection(source, new PagedSettings() { PageNumber = pageNumber, PageSize = pageSize });
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with <paramref name="settings"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="settings">The settings that specifies the conditions of the converted <paramref name="source"/>.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/> initialized with <paramref name="settings"/>.</returns>
        public static PagedCollection<T> ToPagedCollection<T>(this IEnumerable<T> source, PagedSettings settings)
        {
            return new PagedCollection<T>(source, settings);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with starting <paramref name="pageNumber"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="pageNumber">The page number to start with.</param>
        /// <param name="pageSize">The number of elements a page can contain.</param>
        /// <param name="totalElementCount">The total number of elements in the <paramref name="source"/> sequence.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        public static PagedCollection<T> ToPagedCollection<T>(this IEnumerable<T> source, int pageNumber, int pageSize, int totalElementCount)
        {
            return ToPagedCollection(source, new PagedSettings() { PageNumber = pageNumber, PageSize = pageSize }, totalElementCount);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with <paramref name="settings"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="settings">The settings that specifies the conditions of the converted <paramref name="source"/>.</param>
        /// <param name="totalElementCount">The total number of elements in the <paramref name="source"/> sequence.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/> initialized with <paramref name="settings"/>.</returns>
        public static PagedCollection<T> ToPagedCollection<T>(this IEnumerable<T> source, PagedSettings settings, int totalElementCount)
        {
            return new PagedCollection<T>(source, settings, totalElementCount);
        }
    }
}
