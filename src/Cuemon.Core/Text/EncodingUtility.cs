using System;
using System.IO;
using System.Text;

namespace Cuemon.Text
{
    /// <summary>
    /// This utility class is designed to make <see cref="Encoding"/> operations easier to work with.
    /// </summary>
    public static class EncodingUtility
    {
        /// <summary>
        /// Gets an encoding for the ASCII (7-bit) character set.
        /// </summary>
        /// <value>An encoding for the ASCII (7-bit) character set.</value>
        public static Encoding AsciiEncoding { get; } = Encoding.GetEncoding("us-ascii");

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="Stream"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="value"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="value"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(Stream value, out Encoding result)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (!value.CanSeek) // we are unable to read BOM - return default encoding for O/S
            {
                result = Encoding.UTF8;
                return false;
            }

            byte[] byteOrderMarks = new byte[] { 0, 0, 0, 0 };

            long startingPosition = value.Position;
            value.Position = 0;
            value.Read(byteOrderMarks, 0, 4); // only read the first 4 bytes
            value.Seek(startingPosition, SeekOrigin.Begin); // reset to original position}

            return TryParse(byteOrderMarks, out result);
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="value"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="value"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(byte[] value, out Encoding result)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            result = null;
            if (value.Length >= 4)
            {

                if (value[0] == 0xEF &&
                    value[1] == 0xBB &&
                    value[2] == 0xBF)
                {
                    result = Encoding.GetEncoding("UTF-8");
                }
                else if (value[0] == 0x00 &&
                         value[1] == 0x00 &&
                         value[2] == 0xFE &&
                         value[3] == 0xFF)
                {
                    result = Encoding.GetEncoding("UTF-32BE");
                }
                else if (value[0] == 0xFF &&
                         value[1] == 0xFE &&
                         value[2] == 0x00 &&
                         value[3] == 0x00)
                {
                    result = Encoding.GetEncoding("UTF-32");
                }
                else if (value[0] == 0xFE &&
                         value[1] == 0xFF)
                {
                    result = Encoding.GetEncoding("UNICODEFFFE");
                }
                else if (value[0] == 0xFF &&
                         value[1] == 0xFE)
                {
                    result = Encoding.GetEncoding("UTF-16");
                }
                else if (value[0] == 0x2B &&
                         value[1] == 0x2F &&
                         value[2] == 0x76 &&
                         (value[3] == 0x38 ||
                          value[3] == 0x39 ||
                          value[3] == 0x2B ||
                          value[3] == 0x2F))
                {
                    result = Encoding.GetEncoding("UTF-7");
                }
            }
            return (result != null);
        }
    }
}