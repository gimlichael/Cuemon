using System;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Specifies a set of features to support on the <see cref="PagedCollection"/> object.
    /// </summary>
    public class PagedSettings : IEquatable<PagedSettings>
    {
        /// <summary>
        /// Gets or sets the default page size of the <see cref="PagedSettings"/> class. Default is 25.
        /// </summary>
        /// <value>The default page size of the <see cref="PagedSettings"/> class.</value>
        public static int DefaultPageSize { get; set; } = 25;

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

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(PagedSettings other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }
            return PageSize == other.PageSize && PageNumber.Equals(other.PageNumber) && SortOrderDirection.Equals(other.SortOrderDirection) && Data.GetHashCode().Equals(other.Data.GetHashCode()) && SearchCriteria.Equals(other.SearchCriteria) && SortOrderBy.Equals(other.SortOrderBy);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != this.GetType()) { return false; }
            return Equals((PagedSettings) obj);
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