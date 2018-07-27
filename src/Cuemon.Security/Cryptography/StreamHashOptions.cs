using System.IO;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Specifies options that is related to <see cref="HashAlgorithm"/> operations. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="HashOptions" />
    public sealed class StreamHashOptions : HashOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamHashOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="StreamHashOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="HashOptions.AlgorithmType"/></term>
        ///         <description><see cref="HashAlgorithmType.MD5"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="LeaveStreamOpen"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public StreamHashOptions()
        {
            LeaveStreamOpen = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Stream"/> is being left open or to have it being closed and disposed.
        /// </summary>
        /// <value><c>true</c> if the <see cref="Stream"/> is being left open; otherwise, <c>false</c> to have it being closed and disposed of.</value>
        public bool LeaveStreamOpen { get; set; }
    }
}