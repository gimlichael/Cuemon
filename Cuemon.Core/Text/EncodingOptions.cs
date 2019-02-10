﻿using System.IO;
using System.Text;

namespace Cuemon.Text
{
    /// <summary>
    /// Specifies options that is related to <see cref="Encoding"/> operations.
    /// </summary>
    public class EncodingOptions
    {
        /// <summary>
        /// Gets or sets the default preamble action of <see cref="EncodingOptions"/>. Default is <see cref="PreambleSequence.Remove"/>.
        /// </summary>
        /// <value>The default preamble action to use in <see cref="Encoding"/> related operations.</value>
        /// <remarks>Changing this value should be thought through carefully as it can change the behavior you have come to expect. Consider using local adjustment instead.</remarks>
        public static PreambleSequence DefaultPreambleSequence { get; set; } = PreambleSequence.Remove;

        /// <summary>
        /// Gets or sets the default encoding of <see cref="EncodingOptions"/>. Default is <see cref="System.Text.Encoding.UTF8"/>.
        /// </summary>
        /// <value>The default encoding to use in <see cref="Encoding"/> related operations.</value>
        /// <remarks>Changing this value should be thought through carefully as it can change the behavior you have come to expect. Consider using local adjustment instead.</remarks>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EncodingOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Preamble"/></term>
        ///         <description><see cref="DefaultPreambleSequence"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Encoding"/></term>
        ///         <description><see cref="DefaultEncoding"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public EncodingOptions()
        {
            Preamble = DefaultPreambleSequence;
            Encoding = DefaultEncoding;
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

        /// <summary>
        /// Tries to detect an <see cref="System.Text.Encoding"/> object from the specified <paramref name="value"/>. If unsuccessful, the encoding of this instance is returned.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to parse for an <see cref="System.Text.Encoding"/>.</param>
        /// <returns>Either the detected encoding of <paramref name="value"/>  or the encoding of this instance.</returns>
        public Encoding DetectEncoding(Stream value)
        {
            Encoding result;
            if (EncodingUtility.TryParse(value, out result))
            {
                return result;
            }
            return Encoding;
        }

        /// <summary>
        /// Tries to detect an <see cref="System.Text.Encoding"/> object from the specified <paramref name="value"/>. If unsuccessful, the encoding of this instance is returned.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to parse for an <see cref="System.Text.Encoding"/>.</param>
        /// <returns>Either the detected encoding of <paramref name="value"/> or the encoding of this instance.</returns>
        public Encoding DetectEncoding(byte[] value)
        {
            Encoding result;
            if (EncodingUtility.TryParse(value, out result))
            {
                return result;
            }
            return Encoding;
        }
    }
}