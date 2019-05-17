using System;
using System.ComponentModel;
using Cuemon.ComponentModel.TypeConverters;
using Cuemon.Text;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{System.String}"/> interface.
    /// </summary>
    public static class StringTypeConverters
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> of a GUID to its equivalent <see cref="Guid"/> structure.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="string"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="Guid"/>.</param>
        /// <param name="setup">The <see cref="GuidStringOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Guid"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// The specified <paramref name="input"/> was not recognized to be a GUID.
        /// </exception>
        /// <seealso cref="GuidStringParser"/>
        /// <seealso cref="GuidStringOptions"/>
        public static Guid ToGuid(this IConversion<string> _, string input, Action<GuidStringOptions> setup = null)
        {
            return Parsers.FromGuidString.Parse(input, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="string"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <seealso cref="EncodedStringConverter"/>
        /// <seealso cref="EncodingOptions"/>
        public static byte[] ToByteArray(this IConversion<string> _, string input, Action<EncodingOptions> setup = null)
        {
            return ToByteArray<EncodedStringConverter, EncodingOptions>(_, input, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <typeparam name="TConverter">The type of the converter that is responsible for changing a <see cref="string"/> into a <see cref="T:byte[]"/>.</typeparam>
        /// <param name="_">The marker interface of a converter having <see cref="string"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="EncodedStringConverter"/>
        /// <seealso cref="BinaryDigitsStringConverter"/>
        /// <seealso cref="UrlEncodedBase64StringConverter"/>
        public static byte[] ToByteArray<TConverter>(this IConversion<string> _, string input) where TConverter : class, IConverter<string, byte[]>, new()
        {
            return Converter<string, byte[]>.UseConverter<TConverter>(input);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <typeparam name="TConverter">The type of the converter that is responsible for changing a <see cref="string"/> into a <see cref="T:byte[]"/>.</typeparam>
        /// <typeparam name="TOptions">The type of the options which may be configured.</typeparam>
        /// <param name="_">The marker interface of a converter having <see cref="string"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="EncodedStringConverter"/>
        public static byte[] ToByteArray<TConverter, TOptions>(this IConversion<string> _, string input, Action<TOptions> setup = null) 
            where TConverter : class, IConverter<string, byte[], TOptions>, new() 
            where TOptions : class, new()
        {
            return Converter<string, byte[], TOptions>.UseConverter<TConverter>(input, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> of a protocol-relative URL to its equivalent <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="string"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="Uri"/>.</param>
        /// <param name="setup">The <see cref="UrlProtocolRelativeStringOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null -or-
        /// <see cref="UrlProtocolRelativeStringOptions.RelativeReference"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters -or-
        /// <paramref name="input"/> did not start with <see cref="UrlProtocolRelativeStringOptions.RelativeReference"/> -or-
        /// <see cref="UrlProtocolRelativeStringOptions.RelativeReference"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <seealso cref="UrlProtocolRelativeStringConverter"/>
        /// <seealso cref="UrlProtocolRelativeStringOptions"/>
        public static Uri ToUri(this IConversion<string> _, string input, Action<UrlProtocolRelativeStringOptions> setup = null)
        {
            return Converter<string, Uri, UrlProtocolRelativeStringOptions>.UseConverter<UrlProtocolRelativeStringConverter>(input, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:char[]"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="string"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:char[]"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:char[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <seealso cref="EncodedStringConverter"/>
        /// <seealso cref="EncodingOptions"/>
        public static char[] ToCharArray(this IConversion<string> _, string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            return options.Encoding.GetChars(Converter.FromString.ToByteArray(input, setup));
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> of an URI scheme to its equivalent <see cref="UriScheme"/> enumeration.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="string"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="UriScheme"/>.</param>
        /// <returns>A <see cref="UriScheme"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="UriSchemeStringConverter"/>
        public static UriScheme ToUriScheme(this IConversion<string> _, string input)
        {
            return Converter<string, UriScheme>.UseConverter<UriSchemeStringConverter>(input);
        }
    }
}