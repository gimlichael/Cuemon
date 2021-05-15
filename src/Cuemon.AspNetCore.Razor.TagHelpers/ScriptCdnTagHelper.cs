using System;
using Cuemon.AspNetCore.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    /// <summary>
    /// Provides an implementation targeting &lt;script&gt; elements that supports <see cref="ICacheBusting"/> versioning of a static script placed on a location with a CDN role. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ScriptTagHelper{TOptions}" />
    [HtmlTargetElement("cdn-script")]
    [Obsolete("This class is deprecated and will be renamed soon. New and more logical name will be: CdnScriptTagHelper.")]
    public sealed class ScriptCdnTagHelper : ScriptTagHelper<CdnTagHelperOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptCdnTagHelper"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="CdnTagHelperOptions" /> which need to be configured.</param>
        /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
        public ScriptCdnTagHelper(IOptions<CdnTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
        {
        }
    }
}
