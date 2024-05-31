using System;
using System.Collections.Specialized;
using Cuemon.Net.Collections.Specialized;

namespace Cuemon.Net
{
    /// <summary>
    /// Provides a collection of string values that is equivalent to a query string of an <see cref="Uri"/>.
    /// Implements the <see cref="NameValueCollection" />
    /// </summary>
    /// <seealso cref="NameValueCollection" />
    public class QueryStringCollection : NameValueCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStringCollection"/> class.
        /// </summary>
        public QueryStringCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStringCollection"/> class.
        /// </summary>
        /// <param name="qsc">The <see cref="QueryStringCollection"/> to copy to the new <see cref="QueryStringCollection"/> instance.</param>
        public QueryStringCollection(QueryStringCollection qsc) : base(qsc)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStringCollection"/> class.
        /// </summary>
        /// <param name="query">The query string of an <see cref="Uri"/>.</param>
        /// <param name="urlDecode">Specify <c>true</c> to decode the <paramref name="query"/> that has been encoded for transmission in a URL; otherwise, <c>false</c>.</param>
        public QueryStringCollection(string query, bool urlDecode = false)
        {
            Add(Decorator.Enclose(query, false).ToQueryString(urlDecode));
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Decorator.Enclose(this).ToString(FieldValueSeparator.Ampersand, false);
        }
    }
}