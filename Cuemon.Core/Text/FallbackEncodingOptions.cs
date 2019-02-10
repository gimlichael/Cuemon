using System.Text;

namespace Cuemon.Text
{
    /// <summary>
    /// Specifies options that is related to <see cref="Encoding"/> operations.
    /// </summary>
    /// <seealso cref="EncodingOptions" />
    public class FallbackEncodingOptions : EncodingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackEncodingOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FallbackEncodingOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="EncoderFallback"/></term>
        ///         <description><c>EncoderFallback.ExceptionFallback</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="DecoderFallback"/></term>
        ///         <description><c>DecoderFallback.ExceptionFallback</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FallbackEncodingOptions()
        {
            EncoderFallback = EncoderFallback.ExceptionFallback;
            DecoderFallback = DecoderFallback.ExceptionFallback;
        }

        /// <summary>
        /// Gets or sets the object that provides an error-handling procedure when a character cannot be encoded.
        /// </summary>
        /// <value>The object that provides an error-handling procedure when a character cannot be encoded.</value>
        public EncoderFallback EncoderFallback { get; set; }

        /// <summary>
        /// Gets or sets the object that provides an error-handling procedure when a byte sequence cannot be decoded.
        /// </summary>
        /// <value>The object that provides an error-handling procedure when a byte sequence cannot be decoded.</value>
        public DecoderFallback DecoderFallback { get; set; }
    }
}