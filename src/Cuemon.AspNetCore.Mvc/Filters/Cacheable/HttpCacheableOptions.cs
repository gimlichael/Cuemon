using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Specifies options that is related to the <see cref="HttpCacheableFilter" />.
    /// </summary>
    public class HttpCacheableOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCacheableOptions"/> class.
        /// </summary>
        public HttpCacheableOptions()
        {
            Filters = new List<ICacheableAsyncResultFilter>();
        }

        /// <summary>
        /// Gets the filters that will be invoked one by one in <see cref="HttpCacheableFilter.OnResultExecutionAsync"/>.
        /// </summary>
        /// <value>The filters that will be invoked by <see cref="HttpCacheableFilter"/>.</value>
        public IList<ICacheableAsyncResultFilter> Filters { get; }
    }
}