using System;
using Cuemon.Text;

namespace Cuemon.Extensions.Text
{
    public static class StringExtensions
    {
        public static string ToAsciiEncodedString(this string @string, Action<EncodingOptions> setup = null)
        {
            return StringConverter.ChangeToAsciiEncoding(@string, setup);
        }
    }
}