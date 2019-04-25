using System;
using System.Linq;
using System.Text;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make common <see cref="T:byte[]"/>  operations easier to work with.
    /// </summary>
    public static class ByteArrayUtility
	{
        /// <summary>
        /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="bytes"/>.
        /// If unsuccessful, the <paramref name="fallback"/> value is returned.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[]"/> to parse for an <see cref="Encoding"/>.</param>
        /// <param name="fallback">The <see cref="Encoding"/> to use when conversion is unsuccessful.</param>
        /// <returns>Either the detected encoding of <paramref name="bytes"/>  or the <paramref name="fallback"/> encoding.</returns>
        public static Encoding DetectUnicodeEncoding(byte[] bytes, Encoding fallback)
        {
            if (TryDetectUnicodeEncoding(bytes, out var result))
            {
                return result;
            }
            return fallback;
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[]"/> to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="bytes"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="bytes"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="bytes"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectUnicodeEncoding(byte[] bytes, out Encoding result)
        {
            Validator.ThrowIfNull(bytes, nameof(bytes));
            result = null;
            if (bytes.Length >= 4)
            {

                if (bytes[0] == 0xEF &&
                    bytes[1] == 0xBB &&
                    bytes[2] == 0xBF)
                {
                    result = Encoding.GetEncoding("UTF-8");
                }
                else if (bytes[0] == 0x00 &&
                         bytes[1] == 0x00 &&
                         bytes[2] == 0xFE &&
                         bytes[3] == 0xFF)
                {
                    result = Encoding.GetEncoding("UTF-32BE");
                }
                else if (bytes[0] == 0xFF &&
                         bytes[1] == 0xFE &&
                         bytes[2] == 0x00 &&
                         bytes[3] == 0x00)
                {
                    result = Encoding.GetEncoding("UTF-32");
                }
                else if (bytes[0] == 0xFE &&
                         bytes[1] == 0xFF)
                {
                    result = Encoding.GetEncoding("UNICODEFFFE");
                }
                else if (bytes[0] == 0xFF &&
                         bytes[1] == 0xFE)
                {
                    result = Encoding.GetEncoding("UTF-16");
                }
                else if (bytes[0] == 0x2B &&
                         bytes[1] == 0x2F &&
                         bytes[2] == 0x76 &&
                         (bytes[3] == 0x38 ||
                          bytes[3] == 0x39 ||
                          bytes[3] == 0x2B ||
                          bytes[3] == 0x2F))
                {
                    result = Encoding.GetEncoding("UTF-7");
                }
            }
            return (result != null);
        }

        /// <summary>
        /// Combines a variable number of byte arrays into one byte array.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[][]"/> to combine.</param>
        /// <returns>A variable number of <b>byte arrays</b> combined into one <b>byte array</b>.</returns>
        public static byte[] CombineByteArrays(params byte[][] bytes)
		{
            Validator.ThrowIfNull(bytes, nameof(bytes));
            return bytes.SelectMany(x => x).ToArray();
		}

        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">The input <see cref="T:byte[]"/>  to process.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <returns>A <see cref="byte"/> array without preamble information.</returns>
        public static byte[] RemovePreamble(byte[] bytes, Encoding encoding)
		{
            Validator.ThrowIfNull(bytes, nameof(bytes));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            if (bytes.Length <= 1) { return bytes; }
			var preamble = encoding.GetPreamble();
			if (preamble.Length == 0) { return bytes; }
			if ((preamble[0] == bytes[0] && preamble[1] == bytes[1]) || (preamble[0] == bytes[1] && preamble[1] == bytes[0]))
			{
				var bytesToRead = bytes.Length;
				bytesToRead -= preamble.Length;
				var bytesWithNoPreamble = new byte[bytesToRead];
				Array.Copy(bytes, preamble.Length, bytesWithNoPreamble, 0, bytesToRead);
				return bytesWithNoPreamble;
			}
			return bytes;
		}

        /// <summary>
        /// Removes trailing zero information (if any) from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[]"/> to process.</param>
        /// <returns>A <see cref="byte"/> array without trailing zeros.</returns>
        public static byte[] RemoveTrailingZeros(byte[] bytes)
		{
			if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }
			if (bytes.Length <= 1) { throw new ArgumentException("Size must be larger than 1.", nameof(bytes)); }
			var hasTrailingZeros = false;
			var marker = bytes.Length - 1;
			while (bytes[marker] == 0)
			{
				if (!hasTrailingZeros) { hasTrailingZeros = true; }
				marker--;
			}
			if (!hasTrailingZeros) { return bytes; }
			marker++;
			var output = new byte[marker];
			Array.Copy(bytes, output, marker);
			return output;
		}
	}
}