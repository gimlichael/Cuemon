using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make <see cref="TextReader"/> related conversions easier to work with.
    /// </summary>
    public static class TextReaderConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="TextReader"/> object.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>A <see cref="TextReader"/> initialized with <paramref name="value"/>.</returns>
        public static TextReader FromString(string value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return new StringReader(value);
        }
    }
}