namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Specifies a set of features to support on the <see cref="PagedCollection"/> object. This class cannot be inherited.
    /// </summary>
    public sealed class PagedSettings
    {
        private static int DefaultPageSizeValue = 25;

        /// <summary>
        /// Gets or sets the default page size of the <see cref="PagedSettings"/> class. Default is 25.
        /// </summary>
        /// <value>The default page size of the <see cref="PagedSettings"/> class.</value>
        public static int DefaultPageSize
        {
            get { return DefaultPageSizeValue; }
            set { DefaultPageSizeValue = value; }
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedSettings"/> class.
        /// </summary>
        public PagedSettings()
        {
            PageSize = DefaultPageSize;
            PageNumber = 1;
            SortOrderDirection = SortOrder.Unspecified;
            Data = new DataPairDictionary();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Generate.HashCode32(PageSize, PageNumber, (int)SortOrderDirection, Data.GetHashCode()) ^ Generate.HashCode32(string.Concat(SearchCriteria, SortOrderBy));
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether this instance was invoked with a search operation.
        /// </summary>
        /// <value><c>true</c> if this instance invoked with a search operation; otherwise, <c>false</c>.</value>
        public bool HasSearchCriteriaDefined
        {
            get { return !string.IsNullOrEmpty(SearchCriteria); }
        }

        /// <summary>
        /// Gets the search criteria of this instance.
        /// </summary>
        /// <value>The search criteria of this instance.</value>
        public string SearchCriteria { get; set; }

        /// <summary>
        /// Gets the order by sorting of this instance.
        /// </summary>
        /// <value>The order by sorting of this instance.</value>
        public string SortOrderBy { get; set; }

        /// <summary>
        /// Gets the direction of the order by sorting of this instance.
        /// </summary>
        /// <value>The direction of the order by sorting of this instance.</value>
        public SortOrder SortOrderDirection { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance was invoked with a sorting operation.
        /// </summary>
        /// <value><c>true</c> if this instance invoked with a sorting operation.; otherwise, <c>false</c>.</value>
        public bool HasSortOrderByDefined
        {
            get { return !string.IsNullOrEmpty(SortOrderBy); }
        }

        /// <summary>
        /// Gets or sets the number of elements to display on a page.
        /// </summary>
        /// <value>The number of elements to display on a page.</value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the one-based number of the page to iterate.
        /// </summary>
        /// <value>The one-based number of the page to iterate.</value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="DataPair"/> elements that provide information about arbitrary data.
        /// </summary>
        /// <value>The collection of <see cref="DataPair"/> elements that provide information about arbitrary data.</value>
        public DataPairDictionary Data { get; private set; }
        #endregion
    }
}