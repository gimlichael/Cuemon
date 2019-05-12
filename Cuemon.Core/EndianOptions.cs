using System;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="BitConverter"/>.
    /// </summary>
    public class EndianOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndianOptions"/> class.
        /// </summary>
        public EndianOptions()
        {
            UseBigEndian = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use Big-endian instead of Little-endian.
        /// </summary>
        /// <value><c>true</c> if Big-endian should be used; otherwise, <c>false</c>.</value>
        public bool UseBigEndian { get; set; }
    }
}