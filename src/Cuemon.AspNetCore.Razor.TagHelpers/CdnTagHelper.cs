using System;
using Cuemon.AspNetCore.Configuration;
using Cuemon.Configuration;
using Cuemon.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    /// <summary>
    /// Provides a base-class for CDN-related <see cref="TagHelper"/> implementation in Razor for ASP.NET Core.
    /// Implements the <see cref="TagHelper" />
    /// Implements the <see cref="IConfigurable{CdnTagHelperOptions}" />
    /// </summary>
    /// <seealso cref="TagHelper" />
    /// <seealso cref="IConfigurable{CdnTagHelperOptions}" />
    public abstract class CdnTagHelper : TagHelper, IConfigurable<CdnTagHelperOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CdnTagHelper"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="CdnTagHelperOptions" /> which need to be configured.</param>
        /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting"/> interface.</param>
        protected CdnTagHelper(IOptions<CdnTagHelperOptions> setup, ICacheBusting cacheBusting = null)
        {
            CacheBusting = cacheBusting;
            Options = setup.Value;
        }

        /// <summary>
        /// Gets the by constructor optional supplied object implementing the <see cref="ICacheBusting"/> interface.
        /// </summary>
        /// <value>The by constructor optional supplied object implementing the <see cref="ICacheBusting"/> interface.</value>
        protected ICacheBusting CacheBusting { get; }

        /// <summary>
        /// Gets a value indicating whether an object implementing the <see cref="ICacheBusting"/> interface is specified.
        /// </summary>
        /// <value><c>true</c> if an object implementing the <see cref="ICacheBusting"/> interface is specified; otherwise, <c>false</c>.</value>
        protected bool UseCacheBusting => CacheBusting != null;

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public CdnTagHelperOptions Options { get; }

        /// <summary>
        /// Gets the base URL of this instance.
        /// </summary>
        /// <returns>The base URL of this instance.</returns>
        protected virtual string GetBaseUrl()
        {
            var baseUrlIsNullOrEmpty = string.IsNullOrWhiteSpace(Options.BaseUrl);
            var baseUrlWithForwardingSlash = new Stem(Options.BaseUrl).AttachSuffix("/");
            switch (Options.Scheme)
            {
                case CdnUriScheme.None:
                    return baseUrlIsNullOrEmpty ? "" : FormattableString.Invariant($"{baseUrlWithForwardingSlash}");
                case CdnUriScheme.Http:
                    return baseUrlIsNullOrEmpty ? "" : FormattableString.Invariant($"{nameof(UriScheme.Http).ToLowerInvariant()}://{baseUrlWithForwardingSlash}");
                case CdnUriScheme.Https:
                    return baseUrlIsNullOrEmpty ? "" : FormattableString.Invariant($"{nameof(UriScheme.Https).ToLowerInvariant()}://{baseUrlWithForwardingSlash}");
                case CdnUriScheme.Relative:
                    return baseUrlIsNullOrEmpty ? "" : FormattableString.Invariant($"//{baseUrlWithForwardingSlash}");
                default:
                    return "";
            }
        }
    }
}