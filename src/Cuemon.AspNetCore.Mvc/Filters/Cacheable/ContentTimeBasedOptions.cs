using System;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Specifies options that is related to the <see cref="ContentTimeBasedObjectResult{T}" />.
    /// </summary>
    public class ContentTimeBasedOptions
    {
        private readonly TimeBasedOptions _to = new TimeBasedOptions();
        private readonly ContentBasedOptions _io = new ContentBasedOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTimeBasedOptions"/> class.
        /// </summary>
        public ContentTimeBasedOptions()
        {
            IsWeak = _io.IsWeak;
            Modified = _to.Modified;
        }

        /// <summary>
        /// Gets or sets the modified date-time value of an object.
        /// </summary>
        /// <value>The modified date-time value of an object.</value>
        public DateTime? Modified { get; set; }

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
        public ChecksumStrength GetValidation(byte[] checksum)
        {
            return _io.GetValidation(checksum);
        }
    }
}