using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.Web
{
    /// <summary>
    /// This utility class is a light-weight implementation of the now deprecated HttpUtility class. Was made for backward compatibility in terms of <see cref="UrlEncode(string,System.Text.Encoding)"/> and <see cref="UrlDecode(string,System.Text.Encoding)"/>.
    /// </summary>
    /// <remarks>
    /// Kudos to the mono-project team for this class. I only modified some of the original code to fit into this class. For the original code, have a visit here for the source code: https://github.com/mono/mono/blob/master/mcs/class/System.Web/System.Web/HttpUtility.cs or here for the mono-project website: http://www.mono-project.com/.
    /// </remarks>
    public static class HttpUtility
    {
        private static readonly char[] HexadecimalCharactersUpperCase = StringUtility.HexadecimalCharacters.ToCharArray();
        private static readonly char[] HexadecimalCharactersLowerCase = StringUtility.HexadecimalCharacters.ToLowerInvariant().ToCharArray();

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a decoded string.
        /// </summary>
        /// <param name="str">The string to decode.</param>
        /// <returns>A decoded string.</returns>
        public static string UrlDecode(string str)
        {
            return UrlDecode(str, Encoding.UTF8);
        }

        /// <summary>
        /// Converts a URL-encoded string into a decoded string, using the specified encoding object.
        /// </summary>
        /// <param name="str">The string to decode.</param>
        /// <param name="e">The <see cref="Encoding"/> that specifies the decoding scheme.</param>
        /// <returns>A decoded string.</returns>
        public static string UrlDecode(string str, Encoding e)
        {
            if (null == str) { return null; }
            if (str.IndexOf('%') == -1 && str.IndexOf('+') == -1) { return str; }
            if (e == null) { e = Encoding.UTF8; }

            long len = str.Length;
            var bytes = new List<byte>();
            int xchar;
            char ch;

            for (int i = 0; i < len; i++)
            {
                ch = str[i];
                if (ch == '%' && i + 2 < len && str[i + 1] != '%')
                {
                    if (str[i + 1] == 'u' && i + 5 < len)
                    {
                        // unicode hex sequence
                        xchar = GetChar(str, i + 2, 4);
                        if (xchar != -1)
                        {
                            WriteCharBytes(bytes, (char)xchar, e);
                            i += 5;
                        }
                        else
                            WriteCharBytes(bytes, '%', e);
                    }
                    else if ((xchar = GetChar(str, i + 1, 2)) != -1)
                    {
                        WriteCharBytes(bytes, (char)xchar, e);
                        i += 2;
                    }
                    else {
                        WriteCharBytes(bytes, '%', e);
                    }
                    continue;
                }

                if (ch == '+')
                    WriteCharBytes(bytes, ' ', e);
                else
                    WriteCharBytes(bytes, ch, e);
            }

            byte[] buf = bytes.ToArray();
            bytes = null;
            return e.GetString(buf, 0, buf.Length);

        }

        private static void WriteCharBytes(IList buf, char ch, Encoding e)
        {
            if (ch > 255)
            {
                foreach (byte b in e.GetBytes(new char[] { ch }))
                    buf.Add(b);
            }
            else
                buf.Add((byte)ch);
        }

        private static int GetInt(byte b)
        {
            char c = (char)b;
            if (c >= '0' && c <= '9') { return c - '0'; }
            if (c >= 'a' && c <= 'f') { return c - 'a' + 10; }
            if (c >= 'A' && c <= 'F') { return c - 'A' + 10; }
            return -1;
        }
        
        private static int GetChar(string str, int offset, int length)
        {
            int val = 0;
            int end = length + offset;
            for (int i = offset; i < end; i++)
            {
                char c = str[i];
                if (c > 127) { return -1; }
                int current = GetInt((byte)c);
                if (current == -1) { return -1; }
                val = (val << 4) + current;
            }
            return val;
        }

        /// <summary>
        /// Converts a string into a URL-encoded array of bytes using the specified encoding object.
        /// </summary>
        /// <param name="str">The string to encode.</param>
        /// <returns>An encoded array of bytes.</returns>
        public static string UrlEncode(string str)
        {
            return UrlEncode(str, Encoding.UTF8);
        }

        /// <summary>
        /// Converts a string into a URL-encoded array of bytes using the specified encoding object.
        /// </summary>
        /// <param name="str">The string to encode.</param>
        /// <param name="e">The <see cref="Encoding"/> that specifies the encoding scheme.</param>
        /// <returns>An encoded array of bytes.</returns>
        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
                return null;

            if (str == String.Empty)
                return String.Empty;

            bool needEncode = false;
            int len = str.Length;
            for (int i = 0; i < len; i++)
            {
                char c = str[i];
                if ((c < '0') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || (c > 'z'))
                {
                    if (NotEncoded(c))
                        continue;

                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
                return str;

            // avoided GetByteCount call
            byte[] bytes = new byte[e.GetMaxByteCount(str.Length)];
            int realLen = e.GetBytes(str, 0, str.Length, bytes, 0);

            var encodedBytes = UrlEncodeToBytes(bytes, 0, realLen);
            return EncodingUtility.AsciiEncoding.GetString(encodedBytes, 0, encodedBytes.Length);
        }

        /// <summary>
        /// Converts an array of bytes into a URL-encoded array of bytes, starting at the specified position in the array and continuing for the specified number of bytes.
        /// </summary>
        /// <param name="bytes">The array of bytes to encode.</param>
        /// <param name="offset">The position in the byte array at which to begin encoding.</param>
        /// <param name="count">The number of bytes to encode.</param>
        /// <returns>An encoded array of bytes.</returns>
        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            return UrlEncodeToBytes(bytes, offset, count, false);
        }

        /// <summary>
        /// Converts an array of bytes into a URL-encoded array of bytes, starting at the specified position in the array and continuing for the specified number of bytes.
        /// </summary>
        /// <param name="bytes">The array of bytes to encode.</param>
        /// <param name="offset">The position in the byte array at which to begin encoding.</param>
        /// <param name="count">The number of bytes to encode.</param>
        /// <param name="preferUppercaseHexadecimalEncoding">When <c>true</c>, the <paramref name="bytes"/> is encoded using upper-case hexadecimal characters; otherwise lower-case hexadecimal characters.</param>
        /// <param name="encoding">The text encoding to use when writing the encoded bytes. The default is <see cref="UTF8Encoding"/>.</param>
        /// <returns>An encoded array of bytes.</returns>
        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count, bool preferUppercaseHexadecimalEncoding, Encoding encoding = null)
        {
            if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }

            int blen = bytes.Length;
            if (blen == 0) { return new byte[0]; } 

            if (offset < 0 || offset >= blen) { throw new ArgumentOutOfRangeException(nameof(offset)); }
            if (count < 0 || count > blen - offset) { throw new ArgumentOutOfRangeException(nameof(count)); }

            using (Stream result = StreamWriterUtility.CreateStream(UrlEncodeCharWriter, bytes, offset, count, preferUppercaseHexadecimalEncoding ? HexadecimalCharactersUpperCase : HexadecimalCharactersLowerCase, StreamWriterUtility.CreateSettings(encoding ?? new UTF8Encoding())))
            {
                return result.ToByteArray();
            }
        }

        private static void UrlEncodeCharWriter(StreamWriter writer, byte[] bytes, int offset, int count, char[] hexadecimalCharacters)
        {
            int end = offset + count;
            for (int i = offset; i < end; i++)
            {
                char c = (char)bytes[i];
                if (c > 255)
                {
                    int idx;
                    int j = c;

                    writer.Write('%');
                    writer.Write('u');
                    idx = j >> 12;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = (j >> 8) & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = (j >> 4) & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = j & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                    return;
                }

                if (c > ' ' && NotEncoded(c))
                {
                    writer.Write(c);
                    return;
                }
                if (c == ' ')
                {
                    writer.Write('+');
                    return;
                }
                if ((c < '0') ||
                    (c < 'A' && c > '9') ||
                    (c > 'Z' && c < 'a') ||
                    (c > 'z'))
                {
                    writer.Write('%');
                    int idx = ((int)c) >> 4;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = ((int)c) & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                }
                else
                {
                    writer.Write(c);
                }
            }
        }

        private static bool NotEncoded(char c)
        {
            return (c == '!' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_'
                );
        }
    }
}