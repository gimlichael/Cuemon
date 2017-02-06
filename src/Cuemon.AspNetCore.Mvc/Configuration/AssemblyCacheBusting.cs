using System;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc.Configuration
{
    /// <summary>
    /// Provides cache-busting capabilities from an Assembly. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="CacheBusting" />
    public sealed class AssemblyCacheBusting : CacheBusting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCacheBusting"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="AssemblyCacheBustingOptions"/> which need to be configured.</param>
        public AssemblyCacheBusting(Action<AssemblyCacheBustingOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            Version = options.Assembly?.GetCacheValidator(options.ReadByteForByteChecksum).Checksum.ToHexadecimal().ToCasing(options.PreferredCasing);
        }

        /// <summary>
        /// Gets the version to be a part of the link you need cache-busting compatible.
        /// </summary>
        /// <value>The version to be a part of the link you need cache-busting compatible.</value>
        public override string Version { get; protected set; }
    }
}