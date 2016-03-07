using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Represents a way of setting default values for the <see cref="PagedCollection{T}"/> class.
    /// </summary>
    public class PagedCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedCollection"/> class.
        /// </summary>
        protected PagedCollection()
        {
        }

        /// <summary>
        /// Retrieves all the elements in <paramref name="source"/> that match the conditions defined by the specified function delegate <paramref name="match"/>.
        /// </summary>
        /// <param name="source">The sequence to search and apply paging on.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="settings">The settings that specifies the conditions of the <paramref name="match"/> before applying paging to this instance.</param>
        /// <returns>A <see cref="PagedCollection{T}"/> that is the result of <paramref name="match"/>.</returns>
        /// <remarks>This search is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static PagedCollection<T> Search<T>(IEnumerable<T> source, Doer<T, PagedSettings, StringComparison, bool> match, PagedSettings settings)
        {
            return Search(source, match, settings, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retrieves all the elements in <paramref name="source"/> that match the conditions defined by the specified function delegate <paramref name="match"/>.
        /// </summary>
        /// <param name="source">The sequence to search and apply paging on.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="settings">The settings that specifies the conditions of the <paramref name="match"/> before applying paging to this instance.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A <see cref="PagedCollection{T}"/> that is the result of <paramref name="match"/>.</returns>
        public static PagedCollection<T> Search<T>(IEnumerable<T> source, Doer<T, PagedSettings, StringComparison, bool> match, PagedSettings settings, StringComparison comparison)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(match, nameof(match));
            Validator.ThrowIfNull(settings, nameof(settings));

            PagedCollection<T> pagedSource = source as PagedCollection<T>;
            return new PagedCollection<T>(new List<T>(EnumerableUtility.FindAll(pagedSource == null ? source : pagedSource.OriginalSource, match, settings, comparison)), settings);
        }

        /// <summary>
        /// Sort all the elements in <paramref name="source"/> by the <paramref name="settings"/> applied to the specified function delegate <paramref name="sorter"/>.
        /// </summary>
        /// <param name="source">The sequence to sort and apply paging on.</param>
        /// <param name="sorter">The function delegate that defines how the sorting is applied to the elements in <paramref name="source"/>.</param>
        /// <param name="settings">The settings that specifies the conditions of the <paramref name="sorter"/> before applying paging to this instance.</param>
        /// <returns>A <see cref="PagedCollection{T}"/> that is sorted by the specified <paramref name="sorter"/>.</returns>
        public static PagedCollection<T> Sort<T>(IEnumerable<T> source, Doer<IEnumerable<T>, PagedSettings, IEnumerable<T>> sorter, PagedSettings settings)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(sorter, nameof(sorter));
            Validator.ThrowIfNull(settings, nameof(settings));

            PagedCollection<T> pagedSource = source as PagedCollection<T>;
            return  new PagedCollection<T>(sorter(pagedSource == null ? source : pagedSource.OriginalSource, settings), settings);
        }
    }

    /// <summary>
    /// Represents an <see cref="IEnumerable{T}"/> paged data sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of this instance.</typeparam>
    public class PagedCollection<T> : PagedCollection, IEnumerable<T>
    {
        private int _lastHashCodeForSettings;
        private IList<T> _pageableCollection;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The source of the sequence.</param>
        /// <remarks>This will do a <see cref="EnumerableUtility.Count"/> on <paramref name="source"/>; so if your source is fragile consider providing this information in the overloaded constructor.</remarks>
        public PagedCollection(IEnumerable<T> source)
            : this(source, new PagedSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The source of the sequence.</param>
        /// <param name="settings">The <see cref="PagedSettings"/> that will be applied to this instance.</param>
        public PagedCollection(IEnumerable<T> source, PagedSettings settings)
            : this(source, settings, ValidateSource(source).Count())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedCollection{T}"/> class.
        /// </summary>
        /// <param name="selector">The function delegate that will define the <see cref="IEnumerable{T}"/> source sequence of this instance.</param>
        /// <param name="settings">The <see cref="PagedSettings"/> that will be applied to this instance.</param>
        /// <param name="counter">The function delegate that will define the <see cref="TotalElementCount"/> of this instance.</param>
        public PagedCollection(Doer<PagedSettings, IEnumerable<T>> selector, PagedSettings settings, Doer<PagedSettings, int> counter)
            : this(ValidateSelector(selector, settings), settings, ValidateCounter(counter, settings))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The source of the sequence.</param>
        /// <param name="settings">The <see cref="PagedSettings"/> that will be applied to this instance.</param>
        /// <param name="totalElementCount">The total number of elements in the <paramref name="source"/> sequence.</param>
        public PagedCollection(IEnumerable<T> source, PagedSettings settings, int totalElementCount)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(settings, nameof(settings));
            Validator.ThrowIfLowerThan(totalElementCount, 0, nameof(totalElementCount));
            OriginalSource = source;
            Settings = settings;
            TotalElementCount = totalElementCount;
        }

        private static IEnumerable<T> ValidateSelector(Doer<PagedSettings, IEnumerable<T>> selector, PagedSettings settings)
        {
            Validator.ThrowIfNull(selector, nameof(selector));
            Validator.ThrowIfNull(settings, nameof(settings));
            return selector(settings);
        }

        private static int ValidateCounter(Doer<PagedSettings, int> counter, PagedSettings settings)
        {
            Validator.ThrowIfNull(counter, nameof(counter));
            Validator.ThrowIfNull(settings, nameof(settings));
            return counter(settings);
        }

        private static IEnumerable<T> ValidateSource(IEnumerable<T> source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return source;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the settings that specifies the paging features of this instance.
        /// </summary>
        /// <value>The settings that specifies the paging features of this instance.</value>
        public PagedSettings Settings { get; private set; }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get { return PagedSource[index]; }
        }

        internal IEnumerable<T> OriginalSource { get; set; }

        private IList<T> PagedSource
        {
            get
            {
                int currentHashCodeForSettings = Settings.GetHashCode();
                if (_pageableCollection == null || _lastHashCodeForSettings != currentHashCodeForSettings)
                {
                    _pageableCollection = GetPageableCollection(OriginalSource);
                    _lastHashCodeForSettings = currentHashCodeForSettings;
                }
                return _pageableCollection;
            }
            set { _pageableCollection = value; }
        }

        /// <summary>
        /// Gets the number of elements on the current page.
        /// </summary>
        /// <value>The number of elements on the current page.</value>
        public int Count
        {
            get { return PagedSource.Count; }
        }

        /// <summary>
        /// Gets the total amount of pages for the elements in this sequence.
        /// </summary>
        /// <value>The total amount of pages for the elements in this sequence.</value>
        public int PageCount
        {
            get { return GetPageCount(); }
        }

        /// <summary>
        /// Gets the total number of elements in the sequence before paging is applied.
        /// </summary>
        /// <value>The total number of elements in the sequence before paging is applied.</value>
        public int TotalElementCount { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether this instance has a next paged data sequence.
        /// </summary>
        /// <value><c>true</c> if this instance has a next paged data sequence; otherwise, <c>false</c>.</value>
        public bool HasNextPage
        {
            get { return (Settings.PageNumber < PageCount); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a previous paged data sequence.
        /// </summary>
        /// <value><c>true</c> if this instance has a previous paged data sequence; otherwise, <c>false</c>.</value>
        public bool HasPreviousPage
        {
            get { return (Settings.PageNumber > 1 && Settings.PageNumber <= PageCount); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is on the first paged data sequence.
        /// </summary>
        /// <value><c>true</c> if this instance is on the first paged data sequence; otherwise, <c>false</c>.</value>
        public bool FirstPage { get { return Settings.PageNumber == 1; } }

        /// <summary>
        /// Gets a value indicating whether this instance is on the last paged data sequence.
        /// </summary>
        /// <value><c>true</c> if this instance is on the last paged data sequence; otherwise, <c>false</c>.</value>
        public bool LastPage { get { return Settings.PageNumber == PageCount; } }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the first number of the next page range relative to the specified <paramref name="page"/>.
        /// </summary>
        /// <param name="page">The page to calculate the first number of the next page range.</param>
        /// <returns>An <see cref="Int32"/> that represents the first number of the next page range.</returns>
        public int GetStartOfNextPageRange(int page)
        {
            return (((page * Settings.PageSize) - Settings.PageSize) + 1);
        }

        /// <summary>
        /// Gets the last number of the next page range relative to the specified <paramref name="page"/>.
        /// </summary>
        /// <param name="page">The page to calculate the last number of the next page range.</param>
        /// <returns>An <see cref="Int32"/> that represents the last number of the next page range.</returns>
        public int GetEndOfNextPageRange(int page)
        {
            int sonp = GetStartOfNextPageRange(page);
            int eonp = page >= PageCount ? ((page * Settings.PageSize) + (Count - Settings.PageSize)) : (page * Settings.PageSize);
            if (eonp < sonp) { eonp = TotalElementCount; }
            return eonp;
        }

        private List<T> GetPageableCollection(IEnumerable<T> source)
        {
            List<T> pagedSource = new List<T>();
            if (TotalElementCount > 0)
            {
                pagedSource.AddRange(Settings.PageNumber == 1
                    ? source.Take(Settings.PageSize)
                    : source.Skip((Settings.PageNumber - 1) * Settings.PageSize).Take(Settings.PageSize));

                // if this instance was invoked with the function delegates constructor, chances are that we have a controlled PagedSettings sequence bypassing the normal paging logic
                if (pagedSource.Count == 0 &&
                    (HasNextPage || HasPreviousPage)) { pagedSource.AddRange(source.Take(Settings.PageSize)); }
            }
            return pagedSource;
        }

        private int GetPageCount()
        {
            if (TotalElementCount == 0) { return 0; }
            double presult = TotalElementCount / Converter.FromObject<double>(Settings.PageSize);
            return Converter.FromObject<int>(Math.Ceiling(presult));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the paged collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the paged collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return PagedSource.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a paged collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the paged collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); 
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Count: {0}, PageCount: {1}, PageNumber: {2}, PageSize: {3}, TotalElementCount: {4}", Count,
                PageCount,
                Settings.PageNumber,
                Settings.PageSize,
                TotalElementCount);
        }

        #endregion
    }
}