using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cuemon.Text;

namespace Cuemon.Extensions.Web
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    /// <remarks>
    /// Kudos to the mono-project team for this class. I only modified some of the original code to fit into this class. For the original code, have a visit here for the source code: https://github.com/mono/mono/blob/master/mcs/class/System.Web/System.Web/HttpUtility.cs or here for the mono-project website: http://www.mono-project.com/.
    /// </remarks>
    public static class StringExtensions
    {
        /// <summary>
        /// Encodes a URL string.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>An URL encoded string.</returns>
        public static string UrlEncode(this string value, Action<EncodingOptions> setup = null)
        {
            if (value == null)  { return null;}
            if (value == string.Empty) { return string.Empty; }

            var options = Patterns.Configure(setup);
            var needEncode = false;
            var len = value.Length;
            for (var i = 0; i < len; i++)
            {
                var c = value[i];
                if ((c < '0') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || (c > 'z'))
                {
                    if (Infrastructure.NotEncoded(c)) { continue; }
                    needEncode = true;
                    break;
                }
            }

            if (!needEncode) { return value; }

            // avoided GetByteCount call
            var bytes = new byte[options.Encoding.GetMaxByteCount(value.Length)];
            var realLen = options.Encoding.GetBytes(value, 0, value.Length, bytes, 0);

            var encodedBytes = bytes.UrlEncode(0, realLen);
            return Encoding.ASCII.GetString(encodedBytes, 0, encodedBytes.Length);
        }

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a decoded string.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>An URL decoded string.</returns>
        public static string UrlDecode(this string value, Action<EncodingOptions> setup = null)
        {
            if (null == value) { return null; }
            if (value.IndexOf('%') == -1 && value.IndexOf('+') == -1) { return value; }

            var options = Patterns.Configure(setup);
            long len = value.Length;
            var bytes = new List<byte>();

            for (var i = 0; i < len; i++)
            {
                var ch = value[i];
                if (ch == '%' && i + 2 < len && value[i + 1] != '%')
                {
                    int xchar;
                    if (value[i + 1] == 'u' && i + 5 < len)
                    {
                        // unicode hex sequence
                        xchar = GetChar(value, i + 2, 4);
                        if (xchar != -1)
                        {
                            WriteCharBytes(bytes, (char)xchar, options.Encoding);
                            i += 5;
                        }
                        else
                        {
                            WriteCharBytes(bytes, '%', options.Encoding);
                        }
                    }
                    else if ((xchar = GetChar(value, i + 1, 2)) != -1)
                    {
                        WriteCharBytes(bytes, (char)xchar, options.Encoding);
                        i += 2;
                    }
                    else 
                    {
                        WriteCharBytes(bytes, '%', options.Encoding);
                    }
                    continue;
                }

                WriteCharBytes(bytes, ch == '+' ? ' ' : ch, options.Encoding);
            }

            var buf = bytes.ToArray();
            return options.Encoding.GetString(buf, 0, buf.Length);
        }

        private static void WriteCharBytes(IList buf, char ch, Encoding e)
        {
            if (ch > 255)
            {
                foreach (var b in e.GetBytes(new[] { ch })) { buf.Add(b); }
            }
            else
            { 
                buf.Add((byte)ch);
            }
        }

        private static int GetInt(byte b)
        {
            var c = (char)b;
            if (c >= '0' && c <= '9') { return c - '0'; }
            if (c >= 'a' && c <= 'f') { return c - 'a' + 10; }
            if (c >= 'A' && c <= 'F') { return c - 'A' + 10; }
            return -1;
        }
        
        private static int GetChar(string str, int offset, int length)
        {
            var val = 0;
            var end = length + offset;
            for (var i = offset; i < end; i++)
            {
                var c = str[i];
                if (c > 127) { return -1; }
                var current = GetInt((byte)c);
                if (current == -1) { return -1; }
                val = (val << 4) + current;
            }
            return val;
        }
    }
}