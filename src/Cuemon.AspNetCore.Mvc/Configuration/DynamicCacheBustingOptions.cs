using System;

namespace Cuemon.AspNetCore.Mvc.Configuration
{
    /// <summary>
    /// Specifies options that is related to <see cref="DynamicCacheBustingOptions"/> operations.
    /// </summary>
    /// <seealso cref="CacheBustingOptions" />
    public class DynamicCacheBustingOptions : CacheBustingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCacheBustingOptions"/> class.
        /// </summary>
        public DynamicCacheBustingOptions()
        {
            TimeToLive = TimeSpan.FromMinutes(20);
            PreferredLength = 8;
            PreferredCharacters = StringUtility.AlphanumericCharactersCaseSensitive;
        }

        /// <summary>
        /// Gets or sets the preferred length of <see cref="CacheBusting.Version"/>.
        /// </summary>
        /// <value>The preferred length of <see cref="CacheBusting.Version"/>.</value>
        public int PreferredLength { get; set; }

        /// <summary>
        /// Gets or sets the preferred characters of <see cref="CacheBusting.Version"/>.
        /// </summary>
        /// <value>The preferred characters of <see cref="CacheBusting.Version"/>.</value>
        public string PreferredCharacters { get; set; }

        /// <summary>
        /// Gets or sets the TTL of <see cref="CacheBusting.Version"/>.
        /// </summary>
        /// <value>The TTL of <see cref="CacheBusting.Version"/>.</value>
        public TimeSpan TimeToLive { get; set; }
    }
}