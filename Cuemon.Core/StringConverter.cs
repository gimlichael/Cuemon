using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="string"/> related conversions easier to work with.
    /// </summary>
    public static class StringConverter
    {
        internal static readonly IDictionary<UriScheme, string> UriSchemeToStringLookupTable = UriSchemeConverter.StringToUriSchemeLookupTable.ToDictionary(pair => pair.Value, pair => pair.Key);

        /// <summary>
        /// Converts the specified <paramref name="value"/> of an <see cref="Uri"/> to its equivalent protocol-relative <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="Uri"/> to be converted.</param>
        /// <returns>A <see cref="string"/> that is a protocol-relative equivalent to <paramref name="value"/>.</returns>
        public static string ToProtocolRelativeUri(Uri value)
        {
            return ToProtocolRelativeUri(value, StringUtility.NetworkPathReference);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> of an <see cref="Uri"/> to its equivalent protocol-relative <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="Uri"/> to be converted.</param>
        /// <param name="relativeReference">The relative reference to prefix the result with. Default is <see cref="StringUtility.NetworkPathReference"/>.</param>
        /// <returns>A <see cref="string"/> that is a protocol-relative equivalent to <paramref name="value"/>.</returns>
        public static string ToProtocolRelativeUri(Uri value, string relativeReference)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfFalse(value.IsAbsoluteUri, nameof(value), "Uri must be absolute.");
            Validator.ThrowIfNullOrEmpty(relativeReference, nameof(relativeReference));
            var schemeLength = value.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.Unescaped).Length;
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", relativeReference, value.OriginalString.Remove(0, schemeLength));
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent binary representation.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>A binary <see cref="string"/> representation of the elements in <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string ToBinary(byte[] value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return string.Concat(value.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }

        /// <summary>
        /// Encodes a byte array into its equivalent string representation using base 64 digits, which is usable for transmission on the URL.
        /// </summary>
        /// <param name="value">The byte array to encode.</param>
        /// <returns>The string containing the encoded token if the byte array length is greater than one; otherwise, an empty string ("").</returns>
        /// <remarks>
        /// Source: http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-C
        /// </remarks>
        public static string ToUrlEncodedBase64(byte[] value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            string base64 = Convert.ToBase64String(value);
            base64 = base64.Split('=')[0];
            base64 = base64.Replace('+', '-');
            base64 = base64.Replace('/', '_');
            return base64;
        }

        /// <summary>
        /// Converts the specified string representation of an URI scheme to its <see cref="UriScheme"/> equivalent.
        /// </summary>
        /// <param name="uriScheme">A string containing an URI scheme to convert.</param>
        /// <returns>An <see cref="UriScheme"/> equivalent to the specified <paramref name="uriScheme"/> or <see cref="UriScheme.Undefined"/> if a conversion is not possible.</returns>
        public static string FromUriScheme(UriScheme uriScheme)
        {
            string result;
            if (!UriSchemeToStringLookupTable.TryGetValue(uriScheme, out result))
            {
                result = UriScheme.Undefined.ToString();
            }
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>A hexadecimal <see cref="string"/> representation of the elements in <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string ToHexadecimal(byte[] value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent hexadecimal representation.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A hexadecimal <see cref="string"/> representation of the characters in <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToHexadecimal(string value, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return ToHexadecimal(ByteConverter.FromString(value, setup));
        }

        /// <summary>
        /// Converts the specified hexadecimal <paramref name="hexadecimalValue"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="hexadecimalValue">The hexadecimal string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="string"/> representation of the hexadecimal characters in <paramref name="hexadecimalValue"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="hexadecimalValue"/> is null.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string FromHexadecimal(string hexadecimalValue, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(hexadecimalValue, nameof(hexadecimalValue));
            if (!NumberUtility.IsEven(hexadecimalValue.Length)) { throw new ArgumentException("The character length of a hexadecimal string must be an even number.", nameof(hexadecimalValue)); }

            List<byte> converted = new List<byte>();
            int stringLength = hexadecimalValue.Length / 2;
            using (StringReader reader = new StringReader(hexadecimalValue))
            {
                for (int i = 0; i < stringLength; i++)
                {
                    char firstChar = (char)reader.Read();
                    char secondChar = (char)reader.Read();
                    if (!Condition.IsHex(firstChar) || !Condition.IsHex(secondChar)) { throw new ArgumentException("One or more characters is not a valid hexadecimal value.", nameof(hexadecimalValue)); }
                    converted.Add(Convert.ToByte(new string(new[] { firstChar, secondChar }), 16));
                }
            }
            return FromBytes(converted.ToArray(), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a string using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="string"/> containing the results of decoding the specified sequence of bytes.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string FromBytes(byte[] value, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = setup.Configure();
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = options.DetectEncoding(value); }
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    value = ByteUtility.RemovePreamble(value, options.Encoding); // remove preamble from the resolved source encoding value
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options), "The specified argument was out of the range of valid values.");
            }
            return options.Encoding.GetString(value, 0, value.Length);
        }

        /// <summary>
        /// Converts the specified pascal-case representation <paramref name="value"/> to a human readable string.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to convert.</param>
        /// <param name="delimiter">The delimiter to use when converting PascalCasing to a human readable string. Default is space ( ).</param>
        /// <returns>A human readable string from the specified pascal-case representation <paramref name="value"/>.</returns>
        public static string FromPascalCasing(string value, string delimiter = " ")
        {
            Validator.ThrowIfNullOrEmpty(value, nameof(value));

            int processedCharacters = 0;
            StringBuilder result = new StringBuilder();
            foreach (char c in value)
            {
                processedCharacters++;
                if (Char.IsWhiteSpace(c)) { continue; }
                bool first = (processedCharacters == 1);
                bool last = (processedCharacters == value.Length);
                bool between = (!first && !last);
                if (Char.IsUpper(c))
                {
                    result.AppendFormat(between ? delimiter + "{0}" : "{0}", c);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a camel-case representation, using culture-independent casing rules.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to convert.</param>
        /// <returns>A camel-case representation of the specified <see cref="string"/> value.</returns>
        public static string ToCamelCasing(string value)
        {
            if (value.IsNullOrWhiteSpace()) { return value; }
            if (value.Length > 1) { return value.Substring(0, 1).ToLower() + value.Substring(1); }
            return value.ToLower();
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a pascal-case representation, using culture-independent casing rules.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to convert.</param>
        /// <returns>A pascal-case representation of the specified <see cref="string"/> value.</returns>
        public static string ToPascalCasing(string value)
        {
            if (value.IsNullOrWhiteSpace()) { return value; }
            return value.Substring(0, 1).ToUpper() + value.Substring(1);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an international Morse code representation.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to convert.</param>
        /// <returns>An international Morse code representation of the specified <see cref="string"/> value.</returns>
        /// <remarks>Any characters not supported by the international Morse code specifications is excluded from the result.</remarks>
        public static string ToMorseCode(string value)
        {
            return ToMorseCode(value, false);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an international Morse code representation.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to convert.</param>
        /// <param name="includeUnsupportedCharacters">if set to <c>true</c> characters not supported by the internal Morse code is left intact in the result for a general impression.</param>
        /// <returns>An international Morse code representation of the specified <paramref name="value"/> parameter.</returns>
        public static string ToMorseCode(string value, bool includeUnsupportedCharacters)
        {
            Validator.ThrowIfNullOrEmpty(value, nameof(value));
            StringBuilder morsecode = new StringBuilder(value.Length * 4);
            foreach (char character in value)
            {
                switch (character)
                {
                    case 'A':
                    case 'a':
                        morsecode.Append("· —");
                        break;
                    case 'B':
                    case 'b':
                        morsecode.Append("— · · ·");
                        break;
                    case 'C':
                    case 'c':
                        morsecode.Append("— · — ·");
                        break;
                    case 'D':
                    case 'd':
                        morsecode.Append("— · ·");
                        break;
                    case 'E':
                    case 'e':
                        morsecode.Append("·");
                        break;
                    case 'F':
                    case 'f':
                        morsecode.Append("· · — ·");
                        break;
                    case 'G':
                    case 'g':
                        morsecode.Append("— — ·");
                        break;
                    case 'H':
                    case 'h':
                        morsecode.Append("· · · ·");
                        break;
                    case 'I':
                    case 'i':
                        morsecode.Append("· ·");
                        break;
                    case 'J':
                    case 'j':
                        morsecode.Append("· — — —");
                        break;
                    case 'K':
                    case 'k':
                        morsecode.Append("— · —");
                        break;
                    case 'L':
                    case 'l':
                        morsecode.Append("· — · ·");
                        break;
                    case 'M':
                    case 'm':
                        morsecode.Append("— —");
                        break;
                    case 'N':
                    case 'n':
                        morsecode.Append("— ·");
                        break;
                    case 'O':
                    case 'o':
                        morsecode.Append("— — —");
                        break;
                    case 'P':
                    case 'p':
                        morsecode.Append("· — — ·");
                        break;
                    case 'Q':
                    case 'q':
                        morsecode.Append("— — · —");
                        break;
                    case 'R':
                    case 'r':
                        morsecode.Append("· — ·");
                        break;
                    case 'S':
                    case 's':
                        morsecode.Append("· · ·");
                        break;
                    case 'T':
                    case 't':
                        morsecode.Append("—");
                        break;
                    case 'U':
                    case 'u':
                        morsecode.Append("· · —");
                        break;
                    case 'V':
                    case 'v':
                        morsecode.Append("· · · —");
                        break;
                    case 'W':
                    case 'w':
                        morsecode.Append("· — —");
                        break;
                    case 'X':
                    case 'x':
                        morsecode.Append("— · · —");
                        break;
                    case 'Y':
                    case 'y':
                        morsecode.Append("— · — —");
                        break;
                    case 'Z':
                    case 'z':
                        morsecode.Append("— — · ·");
                        break;
                    case '1':
                        morsecode.Append("· — — — —");
                        break;
                    case '2':
                        morsecode.Append("· · — — —");
                        break;
                    case '3':
                        morsecode.Append("· · · — —");
                        break;
                    case '4':
                        morsecode.Append("· · · · —");
                        break;
                    case '5':
                        morsecode.Append("· · · · ·");
                        break;
                    case '6':
                        morsecode.Append("— · · · ·");
                        break;
                    case '7':
                        morsecode.Append("— — · · ·");
                        break;
                    case '8':
                        morsecode.Append("— — — · ·");
                        break;
                    case '9':
                        morsecode.Append("— — — — ·");
                        break;
                    case '0':
                        morsecode.Append("— — — — —");
                        break;
                    case '.':
                        morsecode.Append("· — · — · —");
                        break;
                    case ',':
                        morsecode.Append("— — · · — —");
                        break;
                    case '?':
                        morsecode.Append("· · — — · ·");
                        break;
                    case '\x27':
                        morsecode.Append("· — — — — ·");
                        break;
                    case '!':
                        morsecode.Append("— · — · — —");
                        break;
                    case '/':
                        morsecode.Append("— · · — ·");
                        break;
                    case '(':
                        morsecode.Append("— · — — ·");
                        break;
                    case ')':
                        morsecode.Append("— · — — · —");
                        break;
                    case '&':
                        morsecode.Append("· — · · ·");
                        break;
                    case ':':
                        morsecode.Append("— — — · · ·");
                        break;
                    case ';':
                        morsecode.Append("— · — · — ·");
                        break;
                    case '=':
                        morsecode.Append("— · · · —");
                        break;
                    case '+':
                        morsecode.Append("· — · — ·");
                        break;
                    case '-':
                        morsecode.Append("— · · · · —");
                        break;
                    case '_':
                        morsecode.Append("· · — — · —");
                        break;
                    case '"':
                        morsecode.Append("· — · · — ·");
                        break;
                    case '$':
                        morsecode.Append("· · · — · · —");
                        break;
                    case '@':
                        morsecode.Append("· — — · — ·");
                        break;
                    default:
                        if (includeUnsupportedCharacters) { morsecode.Append(character); }
                        break;
                }
            }
            return morsecode.ToString();
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="String"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="Char"/> sequence to convert.</param>
        /// <returns>A <see cref="String"/> equivalent to the specified <paramref name="value"/>.</returns>
        public static string FromChars(IEnumerable<char> value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var builder = new StringBuilder();
            foreach (var c in value) { builder.Append(c); }
            return builder.ToString();
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="string"/> sequence.
        /// </summary>
        /// <param name="value">The value to convert into a sequence.</param>
        /// <returns>A <see cref="string"/> sequence equivalent to the specified <paramref name="value"/>.</returns>
        public static IEnumerable<string> ToEnumerable(IEnumerable<char> value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return value.Select(c => new string(c, 1));
        }

        /// <summary>
        /// Renders the <paramref name="exception"/> to a human readable <see cref="string"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to render human readable.</param>
        /// <returns>A human readable <see cref="string"/> variant of the specified <paramref name="exception"/>.</returns>
        /// <remarks>The rendered <paramref name="exception"/> defaults to using an instance of <see cref="Encoding.Unicode"/> unless specified otherwise.</remarks>
        public static string FromException(Exception exception)
        {
            return FromException(exception, Encoding.Unicode);
        }

        /// <summary>
        /// Renders the <paramref name="exception"/> to a human readable <see cref="string"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to render human readable.</param>
        /// <param name="encoding">The encoding to use when rendering the <paramref name="exception"/>.</param>
        /// <returns>A human readable <see cref="string"/> variant of the specified <paramref name="exception"/>.</returns>
        public static string FromException(Exception exception, Encoding encoding)
        {
            return FromException(exception, encoding, false);
        }

        /// <summary>
        /// Renders the <paramref name="exception"/> to a human readable <see cref="string"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to render human readable.</param>
        /// <param name="encoding">The encoding to use when rendering the <paramref name="exception"/>.</param>
        /// <param name="includeStackTrace">if set to <c>true</c> the stack trace of the exception is included in the rendered result.</param>
        /// <returns>A human readable <see cref="string"/> variant of the specified <paramref name="exception"/>.</returns>
        public static string FromException(Exception exception, Encoding encoding, bool includeStackTrace)
        {
            Validator.ThrowIfNull(exception, nameof(exception));
            Validator.ThrowIfNull(encoding, nameof(encoding));

            MemoryStream output = null;
            MemoryStream tempOutput = null;
            string result;
            try
            {
                tempOutput = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(tempOutput, encoding))
                {
                    WriteException(writer, exception, includeStackTrace);
                    writer.Flush();
                    tempOutput.Position = 0;
                    output = tempOutput;
                    tempOutput = null;
                }
                result = FromBytes(output.ToArray(), options =>
                {
                    options.Encoding = encoding;
                    options.Preamble = PreambleSequence.Remove;
                });
                output = null;
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
                if (output != null) { output.Dispose(); }
            }
            return result;
        }

        private static void WriteException(TextWriter writer, Exception exception, bool includeStackTrace)
        {
            Type exceptionType = exception.GetType();
            writer.WriteLine("{0}.{1}{2}: {3}", exceptionType.Namespace, exceptionType.Name, string.IsNullOrEmpty(exception.Source) ? "" : " in " + exception.Source, exception.Message);
            WriteExceptionCore(writer, exception, includeStackTrace);
        }

        private static void WriteExceptionCore(TextWriter writer, Exception exception, bool includeStackTrace)
        {
            if (exception.StackTrace != null && includeStackTrace)
            {
                string[] lines = exception.StackTrace.Split(new[] { StringUtility.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    writer.WriteLine("   {0}", line.Trim());
                }
            }

            if (exception.Data.Count > 0)
            {
                writer.WriteLine("   Data");
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WriteLine("      {0}: {1}", entry.Key, entry.Value);
                }
            }

            WriteInnerExceptions(writer, exception, includeStackTrace);
        }

        private static void WriteInnerExceptions(TextWriter writer, Exception exception, bool includeStackTrace)
        {
            var innerExceptions = new List<Exception>(ExceptionUtility.Flatten(exception));
            if (innerExceptions.Count > 0)
            {
                foreach (var inner in innerExceptions)
                {
                    Type exceptionType = inner.GetType();
                    writer.WriteLine("{0}.{1}{2}: {3}", exceptionType.Namespace, exceptionType.Name, string.IsNullOrEmpty(inner.Source) ? "" : " in " + inner.Source, inner.Message);
                    WriteExceptionCore(writer, inner, includeStackTrace);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to represent.</param>
        /// <returns>A <see cref="System.String" /> that represents the specified <paramref name="instance"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="instance"/>, these rules applies: <br/>
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped<br/>
        /// 2: any public properties having index parameters is skipped<br/>
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden.<br/><br/>
        /// Note: do not call this method from an overridden ToString(..) method as a stackoverflow exception will occur.
        /// </remarks>
        public static string FromObject(object instance)
        {
            return FromObject(instance, false);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to represent.</param>
        /// <param name="bypassOverrideCheck">Specify <c>true</c> to bypass the check for if a ToString() method is overridden; otherwise, <c>false</c> to use default behavior, where an overridden method will return without further processing.</param>
        /// <returns>A <see cref="string" /> that represents the specified <paramref name="instance"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="instance"/>, these rules applies: <br/>
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped<br/>
        /// 2: any public properties having index parameters is skipped<br/>
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden.<br/><br/>
        /// Note: do not call this method from an overridden ToString(..) method without setting <paramref name="bypassOverrideCheck"/> to <c>true</c>; otherwise a stackoverflow exception will occur.
        /// </remarks>
        public static string FromObject(object instance, bool bypassOverrideCheck)
        {
            return FromObject(instance, bypassOverrideCheck, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to represent.</param>
        /// <param name="bypassOverrideCheck">Specify <c>true</c> to bypass the check for if a ToString() method is overriden; otherwise, <c>false</c> to use default behaviour, where an overriden method will return without further processing.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> that represents the specified <paramref name="instance"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="instance"/>, these rules applies: <br/>
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped<br/>
        /// 2: any public properties having index parameters is skipped<br/>
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden.<br/><br/>
        /// Note: do not call this method from an overridden ToString(..) method without setting <paramref name="bypassOverrideCheck"/> to <c>true</c>; otherwise a stackoverflow exception will occur.
        /// </remarks>
        public static string FromObject(object instance, bool bypassOverrideCheck, IFormatProvider provider)
        {
            return FromObject(instance, bypassOverrideCheck, provider, ", ");
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to represent.</param>
        /// <param name="bypassOverrideCheck">Specify <c>true</c> to bypass the check for if a ToString() method is overriden; otherwise, <c>false</c> to use default behaviour, where an overriden method will return without further processing.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="delimiter">The delimiter specification for when representing public properties of <paramref name="instance"/>.</param>
        /// <returns>A <see cref="System.String" /> that represents the specified <paramref name="instance"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="instance"/>, these rules applies: <br/>
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped<br/>
        /// 2: any public properties having index parameters is skipped<br/>
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden.<br/><br/>
        /// Note: do not call this method from an overridden ToString(..) method without setting <paramref name="bypassOverrideCheck"/> to <c>true</c>; otherwise a stackoverflow exception will occur.
        /// </remarks>
        public static string FromObject(object instance, bool bypassOverrideCheck, IFormatProvider provider, string delimiter)
        {
            return FromObject(instance, bypassOverrideCheck, provider, delimiter, DefaultPropertyConverter);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to represent.</param>
        /// <param name="bypassOverrideCheck">Specify <c>true</c> to bypass the check for if a ToString() method is overriden; otherwise, <c>false</c> to use default behaviour, where an overriden method will return without further processing.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="delimiter">The delimiter specification for when representing public properties of <paramref name="instance"/>.</param>
        /// <param name="propertyConverter">The function delegate that convert <see cref="PropertyInfo"/> objects to human-readable content.</param>
        /// <returns>A <see cref="System.String" /> that represents the specified <paramref name="instance"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="instance"/>, these rules applies: <br/>
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped<br/>
        /// 2: any public properties having index parameters is skipped<br/>
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden.<br/><br/>
        /// Note: do not call this method from an overridden ToString(..) method without setting <paramref name="bypassOverrideCheck"/> to <c>true</c>; otherwise a stackoverflow exception will occur.
        /// </remarks>
        public static string FromObject(object instance, bool bypassOverrideCheck, IFormatProvider provider, string delimiter, Func<PropertyInfo, object, IFormatProvider, string> propertyConverter)
        {
            return FromObject(instance, bypassOverrideCheck, provider, delimiter, propertyConverter, ReflectionUtility.GetProperties, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to represent.</param>
        /// <param name="bypassOverrideCheck">Specify <c>true</c> to bypass the check for if a ToString() method is overriden; otherwise, <c>false</c> to use default behaviour, where an overriden method will return without further processing.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="delimiter">The delimiter specification for when representing public properties of <paramref name="instance"/>.</param>
        /// <param name="propertyConverter">The function delegate that convert <see cref="PropertyInfo"/> objects to human-readable content.</param>
        /// <param name="propertiesReader">The function delegate that read <see cref="PropertyInfo"/> objects from the underlying <see cref="Type"/> of <paramref name="instance"/>.</param>
        /// <param name="propertiesReaderBindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search for <see cref="PropertyInfo"/> objects in the function delegate <paramref name="propertiesReader"/> is conducted.</param>
        /// <returns>A <see cref="System.String" /> that represents the specified <paramref name="instance"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="instance"/>, these rules applies: <br/>
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped<br/>
        /// 2: any public properties having index parameters is skipped<br/>
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden.<br/><br/>
        /// Note: do not call this method from an overridden ToString(..) method without setting <paramref name="bypassOverrideCheck"/> to <c>true</c>; otherwise a stackoverflow exception will occur.
        /// </remarks>
        public static string FromObject(object instance, bool bypassOverrideCheck, IFormatProvider provider, string delimiter, Func<PropertyInfo, object, IFormatProvider, string> propertyConverter, Func<Type, BindingFlags, IEnumerable<PropertyInfo>> propertiesReader, BindingFlags propertiesReaderBindingAttr)
        {
            Validator.ThrowIfNull(propertyConverter, nameof(propertyConverter));
            Validator.ThrowIfNull(propertiesReader, nameof(propertiesReader));

            if (instance == null) { return "<null>"; }

            if (!bypassOverrideCheck)
            {
                Func<string> toString = instance.ToString;
                var toStringMethodInfo = toString.GetMethodInfo();
                if (ReflectionUtility.IsOverride(toStringMethodInfo))
                {
                    var stringResult = toString();
                    return toStringMethodInfo.DeclaringType == typeof(bool) ? stringResult.ToLowerInvariant() : stringResult;
                }
            }

            Type instanceType = instance.GetType();
            StringBuilder instanceSignature = new StringBuilder(string.Format(provider, "{0}", FromType(instanceType, true)));
            IEnumerable<PropertyInfo> properties = propertiesReader(instanceType, propertiesReaderBindingAttr).Where(IndexParametersLengthIsZeroPredicate);
            instanceSignature.AppendFormat(" {{ {0} }}", ToDelimitedString(properties, delimiter, propertyConverter, instance, provider));
            return instanceSignature.ToString();
        }

        private static bool IndexParametersLengthIsZeroPredicate(PropertyInfo property)
        {
            return (property.GetIndexParameters().Length == 0);
        }

        private static string DefaultPropertyConverter(PropertyInfo property, object instance, IFormatProvider provider)
        {
            if (property.CanRead)
            {
                if (TypeUtility.IsComplex(property.PropertyType))
                {
                    return string.Format(provider, "{0}={1}", property.Name, FromType(property.PropertyType, true));
                }
                object instanceValue = ReflectionUtility.GetPropertyValue(instance, property);
                return string.Format(provider, "{0}={1}", property.Name, instanceValue ?? "<null>");
            }
            return string.Format(provider, "{0}=<no getter>", property.Name);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <returns>A sanitized <see cref="string"/> representation of <paramref name="source"/>.</returns>
        /// <remarks>Only the simple name of the <paramref name="source"/> is returned, not the fully qualified name.</remarks>
        public static string FromType(Type source)
        {
            return FromType(source, false);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <paramref name="source"/>; otherwise, <c>false</c> for the simple name of <paramref name="source"/>.</param>
        /// <returns>A sanitized <see cref="string"/> representation of <paramref name="source"/>.</returns>
        public static string FromType(Type source, bool fullName)
        {
            return FromType(source, fullName, false);
        }

        /// <summary>
        /// Converts the name of the <paramref name="source"/> with the intend to be understood by humans. 
        /// </summary>
        /// <param name="source">The type to sanitize the name from.</param>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <paramref name="source"/>; otherwise, <c>false</c> for the simple name of <paramref name="source"/>.</param>
        /// <param name="excludeGenericArguments">Specify <c>true</c> to exclude generic arguments from the result; otherwise <c>false</c> to include generic arguments should the <paramref name="source"/> be a generic type.</param>
        /// <returns>A sanitized <see cref="string"/> representation of <paramref name="source"/>.</returns>
        public static string FromType(Type source, bool fullName, bool excludeGenericArguments)
        {
            Validator.ThrowIfNull(source, nameof(source));

            string typeName = FromTypeConverter(source, fullName);
            if (!source.GetTypeInfo().IsGenericType) { return typeName; }

            Type[] parameters = source.GetGenericArguments();
            int indexOfGraveAccent = typeName.IndexOf('`');
            typeName = indexOfGraveAccent >= 0 ? typeName.Remove(indexOfGraveAccent) : typeName;
            return excludeGenericArguments ? typeName : string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", typeName, ToDelimitedString(parameters, ", ", FromTypeConverter, fullName));
        }

        private static readonly char[] InvalidCharacters = StringUtility.PunctuationMarks.Replace(".", "").ToCharArray();

        private static string FromTypeConverter(Type source, bool fullName)
        {
            if (source.IsAnonymousMethod())
            {
                var namespaceSegments = source.FullName.Split('.');
                var className = namespaceSegments.Last().Replace(source.Name, "").RemoveAll(InvalidCharacters);
                return fullName ? source.FullName.Replace(source.Name, "").RemoveAll(InvalidCharacters) : className;

            }
            return fullName ? source.FullName : source.Name;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a string using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The <see cref="System.IO.Stream"/> to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="string"/> containing the decoded result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string FromStream(Stream value, Action<EncodingOptions> setup = null)
        {
            return FromStream(value, setup, false);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a string using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The <see cref="System.IO.Stream"/> to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <see cref="string"/> containing the decoded result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string FromStream(Stream value, Action<EncodingOptions> setup, bool leaveStreamOpen)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = setup.Configure();
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = options.DetectEncoding(value); }
            if (options.Preamble < PreambleSequence.Keep || options.Preamble > PreambleSequence.Remove) { throw new ArgumentOutOfRangeException(nameof(setup), "The specified argument was out of the range of valid values."); }
            return FromBytes(ByteConverter.FromStream(value, leaveStreamOpen), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of comma delimited values.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <returns>A <see cref="string"/> of comma delimited values.</returns>
        public static string ToDelimitedString<T>(IEnumerable<T> source)
        {
            return ToDelimitedString(source, ",");
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(IEnumerable<TSource> source, string delimiter)
        {
            return ToDelimitedString(source, delimiter, DefaultConverter);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(IEnumerable<TSource> source, string delimiter, Func<TSource, string> converter)
        {
            return ToDelimitedString(source, delimiter, (string)null, converter);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T>(IEnumerable<TSource> source, string delimiter, Func<TSource, T, string> converter, T arg)
        {
            return ToDelimitedString(source, delimiter, (string)null, converter, arg);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2>(IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, string> converter, T1 arg1, T2 arg2)
        {
            return ToDelimitedString(source, delimiter, (string)null, converter, arg1, arg2);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3>(IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, T3, string> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            return ToDelimitedString(source, delimiter, (string)null, converter, arg1, arg2, arg3);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4>(IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, T3, T4, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return ToDelimitedString(source, delimiter, (string)null, converter, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, string delimiter, Func<TSource, T1, T2, T3, T4, T5, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return ToDelimitedString(source, delimiter, null, converter, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(IEnumerable<TSource> source, string delimiter, string format)
        {
            return ToDelimitedString(source, delimiter, format, DefaultConverter);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource>(IEnumerable<TSource> source, string delimiter, string format, Func<TSource, string> converter)
        {
            ValidateToDelimitedString(source, delimiter);
            var factory = FuncFactory.Create(converter, default(TSource));
            return ToDelimitedStringCore(factory, source, delimiter, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T>(IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T, string> converter, T arg)
        {
            ValidateToDelimitedString(source, delimiter);
            var factory = FuncFactory.Create(converter, default(TSource), arg);
            return ToDelimitedStringCore(factory, source, delimiter, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2>(IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, string> converter, T1 arg1, T2 arg2)
        {
            ValidateToDelimitedString(source, delimiter);
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2);
            return ToDelimitedStringCore(factory, source, delimiter, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3>(IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, T3, string> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            ValidateToDelimitedString(source, delimiter);
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3);
            return ToDelimitedStringCore(factory, source, delimiter, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4>(IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, T3, T4, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            ValidateToDelimitedString(source, delimiter);
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4);
            return ToDelimitedStringCore(factory, source, delimiter, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <param name="source">A collection of values to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="format">The desired format of the converted values.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a string representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        public static string ToDelimitedString<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, string delimiter, string format, Func<TSource, T1, T2, T3, T4, T5, string> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            ValidateToDelimitedString(source, delimiter);
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5);
            return ToDelimitedStringCore(factory, source, delimiter, format);
        }

        private static void ValidateToDelimitedString<TSource>(IEnumerable<TSource> source, string delimiter)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));
        }

        private static string ToDelimitedStringCore<TTuple, TSource>(FuncFactory<TTuple, string> factory, IEnumerable<TSource> source, string delimiter, string format) where TTuple : Template<TSource>
        {
            bool hasCustomFormat = !string.IsNullOrEmpty(format);
            if (hasCustomFormat)
            {
                int foundArguments;
                if (!StringUtility.ParseFormat(format, 1, out foundArguments)) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "You can only specify 1 formatting argument for the desired value. Actual result was {0} formatting arguments: \"{1}\".", foundArguments, format), nameof(format)); }
            }
            format = hasCustomFormat ? format + "{1}" : "{0}{1}";
            StringBuilder delimitedValues = new StringBuilder();
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    factory.GenericArguments.Arg1 = enumerator.Current;
                    delimitedValues.AppendFormat(format, factory.ExecuteMethod(), delimiter);
                }
            }
            return delimitedValues.Length > 0 ? delimitedValues.ToString(0, delimitedValues.Length - delimiter.Length) : delimitedValues.ToString();
        }

        private static string DefaultConverter<T>(T value)
        {
            return value.ToString();
        }
    }
}