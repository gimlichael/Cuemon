﻿using System.Security.Cryptography;
using System.Text;
using Cuemon.Text;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Configuration options for <see cref="HashAlgorithm"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="HashOptions" />
    public sealed class StringHashOptions : HashOptions, IEncodingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringHashOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="StringHashOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="HashOptions.AlgorithmType"/></term>
        ///         <description><see cref="HashAlgorithmType.SHA256"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Encoding"/></term>
        ///         <description><c><see cref="System.Text.Encoding.Unicode"/></c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Preamble"/></term>
        ///         <description><see cref="PreambleSequence.Remove"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public StringHashOptions()
        {
            Encoding = Encoding.Unicode;
            Preamble = PreambleSequence.Remove;
        }

        /// <summary>
        /// Gets or sets the action to take in regards to encoding related preamble sequences.
        /// </summary>
        /// <value>A value that indicates whether to preserve or remove preamble sequences.</value>
        public PreambleSequence Preamble { get; set; }

        /// <summary>
        /// Gets or sets the encoding for the operation.
        /// </summary>
        /// <value>The encoding for the operation.</value>
        public Encoding Encoding { get; set; }
    }
}