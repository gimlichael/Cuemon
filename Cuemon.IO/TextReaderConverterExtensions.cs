using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// This is an extension implementation of the <see cref="TextReaderConverter"/> class.
    /// </summary>
    public static class TextReaderConverterExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="TextReader"/> object.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>A <see cref="TextReader"/> initialized with <paramref name="value"/>.</returns>
        public static TextReader ToTextReader(this string value)
        {
            return TextReaderConverter.FromString(value);
        }
    }
}