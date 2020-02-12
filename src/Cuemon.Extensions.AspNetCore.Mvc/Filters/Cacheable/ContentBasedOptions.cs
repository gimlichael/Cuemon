using Cuemon.Integrity;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Specifies options that is related to the <see cref="ContentBasedObjectResult{T}" />.
    /// </summary>
    public class ContentBasedOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBasedOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ContentBasedOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="IsWeak"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ContentBasedOptions()
        {
            IsWeak = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a weak validation.
        /// </summary>
        /// <value><c>true</c> if this instance has a weak validation; otherwise, <c>false</c>.</value>
        public bool IsWeak { get; set; }

        /// <summary>
        /// Gets the strength of the checksum validation.
        /// </summary>
        /// <param name="checksum">The checksum to determine the strength of.</param>
        /// <returns>One of the <see cref="ChecksumStrength"/> values.</returns>
        public ChecksumStrength GetValidation(byte[] checksum) => checksum == null || checksum.Length == 0 
            ? ChecksumStrength.None 
            : IsWeak 
                ? ChecksumStrength.Weak 
                : ChecksumStrength.Strong;
    }
}