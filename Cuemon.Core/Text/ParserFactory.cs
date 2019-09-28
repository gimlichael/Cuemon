using System;
using System.Globalization;
using Cuemon.ComponentModel;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides access to factory methods that are tailored for parsing operations following the patterns defined in <see cref="IParser"/> and <see cref="ITypeParser"/>.
    /// </summary>
    public static class ParserFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="Base64Parser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="Base64Parser"/>.</returns>
        public static IParser<byte[]> CreateBase64Parser()
        {
            return new Base64Parser();
        }

        /// <summary>
        /// Creates an instance of <see cref="UrlEncodedBase64Parser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="UrlEncodedBase64Parser"/>.</returns>
        public static IParser<byte[]> CreateUrlEncodedBase64Parser()
        {
            return new UrlEncodedBase64Parser();
        }

        /// <summary>
        /// Creates an instance of <see cref="BinaryDigitsParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult}"/> implementation of <see cref="BinaryDigitsParser"/>.</returns>
        public static IParser<byte[]> CreateBinaryDigitsParser()
        {
            return new BinaryDigitsParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="EnumParser"/>.
        /// </summary>
        /// <returns>An <see cref="ITypeParser{TOptions}"/> implementation of <see cref="EnumParser"/>.</returns>
        public static ITypeParser<EnumOptions> CreateEnumParser()
        {
            return new EnumParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="GuidParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="GuidParser"/>.</returns>
        public static IParser<Guid, GuidOptions> CreateGuidParser()
        {
            return new GuidParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="ComplexValueParser"/>.
        /// </summary>
        /// <returns>An <see cref="ITypeParser{TOptions}"/> implementation of <see cref="ComplexValueParser"/>.</returns>
        public static ITypeParser<TypeConverterOptions> CreateComplexValueParser()
        {
            return new ComplexValueParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="SimpleValueParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="SimpleValueParser"/>.</returns>
        public static IParser<object, FormattingOptions<CultureInfo>> CreateSimpleValueParser()
        {
            return new SimpleValueParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="ProtocolRelativeUrlParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="ProtocolRelativeUrlParser"/>.</returns>
        public static IParser<Uri, ProtocolRelativeUrlOptions> CreateProtocolRelativeUrlParser()
        {
            return new ProtocolRelativeUrlParser();
        }

        /// <summary>
        /// Creates an instance of <see cref="SimpleUriParser"/>.
        /// </summary>
        /// <returns>An <see cref="IParser{TResult,TOptions}"/> implementation of <see cref="SimpleUriParser"/>.</returns>
        public static IParser<Uri, SimpleUriOptions> CreateUriParser()
        {
            return new SimpleUriParser();
        }
    }
}