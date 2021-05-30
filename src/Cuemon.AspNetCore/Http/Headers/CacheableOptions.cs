using System;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Configuration options for <see cref="CacheableMiddleware"/>.
    /// </summary>
    public class CacheableOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheableOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="CacheableOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="CacheControl"/></term>
        ///         <description><code>
        ///             new CacheControlHeaderValue()
        ///             {
        ///                 Public = true,
        ///                 MustRevalidate = true,
        ///                 NoTransform = true,
        ///                 MaxAge = TimeSpan.FromDays(7)
        ///             };
        ///         </code></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Expires"/></term>
        ///         <description><c>new ExpiresHeaderValue(TimeSpan.FromDays(7));</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public CacheableOptions()
        {
            var expires = TimeSpan.FromDays(7);
            Validators = new List<ICacheableValidator>();
            CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MustRevalidate = true,
                NoTransform = true,
                MaxAge = expires
            };
            Expires = new ExpiresHeaderValue(expires);
        }

        /// <summary>
        /// Gets or sets the Cache-Control header associated with a HTTP response.
        /// </summary>
        /// <value>The Cache-Control header associated with a HTTP response.</value>
        public CacheControlHeaderValue CacheControl { get; set; }

        /// <summary>
        /// Gets or sets the Expires header associated with a HTTP response.
        /// </summary>
        /// <value>The Expires header associated with a HTTP response.</value>
        public ExpiresHeaderValue Expires { get; set; }

        /// <summary>
        /// Gets the validators that will be invoked one by one in <see cref="CacheableMiddleware.InvokeAsync"/>.
        /// </summary>
        /// <value>The validators that will be invoked by <see cref="CacheableMiddleware"/>.</value>
        public IList<ICacheableValidator> Validators { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has an assigned <see cref="CacheControl"/> value.
        /// </summary>
        /// <value><c>true</c> if this instance has a <see cref="CacheControl"/> assigned; otherwise, <c>false</c>.</value>
        public bool UseCacheControl => CacheControl != null;

        /// <summary>
        /// Gets a value indicating whether this instance has an assigned <see cref="Expires"/> value.
        /// </summary>
        /// <value><c>true</c> if this instance has an <see cref="Expires"/> assigned; otherwise, <c>false</c>.</value>
        public bool UseExpires => Expires != null;
    }
}
