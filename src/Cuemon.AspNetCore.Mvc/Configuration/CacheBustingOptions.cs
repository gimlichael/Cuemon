namespace Cuemon.AspNetCore.Mvc.Configuration
{
    /// <summary>
    /// Specifies options that is related to <see cref="CacheBusting"/> operations.
    /// </summary>
    public class CacheBustingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBustingOptions"/> class.
        /// </summary>
        public CacheBustingOptions()
        {
            PreferredCasing = CasingMethod.LowerCase;
        }

        /// <summary>
        /// Gets or sets the preferred casing to use on <see cref="CacheBusting.Version"/>.
        /// </summary>
        /// <value>The preferred casing to use on <see cref="CacheBusting.Version"/>.</value>
        public CasingMethod PreferredCasing { get; set; }
    }
}