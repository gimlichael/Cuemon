namespace Cuemon.Text
{
    /// <summary>
    /// Provides access to factory methods that are tailored for parsing operations following the patterns defined in <see cref="IParser"/> and <see cref="ITypeParser"/>.
    /// </summary>
    public static class ParseFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="HexadecimalParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="HexadecimalParser"/>.</returns>
        public static HexadecimalParser FromHexadecimal()
        {
            return new HexadecimalParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="Base64Parser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="Base64Parser"/>.</returns>
        public static Base64Parser FromBase64()
        {
            return new Base64Parser();
        }

        /// <summary>
        /// Creates an instance of <see cref="UrlEncodedBase64Parser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="UrlEncodedBase64Parser"/>.</returns>
        public static UrlEncodedBase64Parser FromUrlEncodedBase64()
        {
            return new UrlEncodedBase64Parser();
        }

        /// <summary>
        /// Creates an instance of <see cref="BinaryDigitsParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="BinaryDigitsParser"/>.</returns>
        public static BinaryDigitsParser FromBinaryDigits()
        {
            return new BinaryDigitsParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="EnumParser"/>.
        /// </summary>
        /// <returns>An <see cref="ITypeParser{TOptions}"/> implementation of <see cref="EnumParser"/>.</returns>
        public static EnumParser FromEnum()
        {
            return new EnumParser();
        }
                /// <summary>
        /// Creates an instance of <see cref="GuidParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="GuidParser"/>.</returns>
        public static GuidParser FromGuid()
        {
            return new GuidParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="ComplexValueParser"/>.
        /// </summary>
        /// <returns>An <see cref="ITypeParser{TOptions}"/> implementation of <see cref="ComplexValueParser"/>.</returns>
        public static ComplexValueParser FromAnything()
        {
            return new ComplexValueParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="SimpleValueParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="SimpleValueParser"/>.</returns>
        public static SimpleValueParser FromSimpleValue()
        {
            return new SimpleValueParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="ProtocolRelativeUrlParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="ProtocolRelativeUrlParser"/>.</returns>
        public static ProtocolRelativeUrlParser FromProtocolRelativeUrl()
        {
            return new ProtocolRelativeUrlParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="SimpleUriParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="SimpleUriParser"/>.</returns>
        public static SimpleUriParser FromUri()
        {
            return new SimpleUriParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="UriSchemeParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="UriSchemeParser"/>.</returns>
        public static UriSchemeParser FromUriScheme()
        {
            return new UriSchemeParser();
        }
    }
}