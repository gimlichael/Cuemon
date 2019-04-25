using System.IO;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Configuration options for <see cref="KeyedHashAlgorithm"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="KeyedHashOptions" />
    public sealed class StreamKeyedHashOptions : KeyedHashOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamKeyedHashOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="StreamKeyedHashOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="KeyedHashOptions.AlgorithmType"/></term>
        ///         <description><see cref="HmacAlgorithmType.SHA1"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="LeaveOpen"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public StreamKeyedHashOptions()
        {
            LeaveOpen = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Stream"/> is being left open or to have it being closed and disposed.
        /// </summary>
        /// <value><c>true</c> if the <see cref="Stream"/> is being left open; otherwise, <c>false</c> to have it being closed and disposed of.</value>
        public bool LeaveOpen { get; set; }
    }
}