using System;
using Cuemon.ComponentModel.Codecs;
using Cuemon.ComponentModel.Converters;
using Cuemon.Text;

namespace Cuemon.ComponentModel.TypeConverters
{
    public class TextConverter : IConverter<string, char[], EncodingOptions>
    {
        public char[] ChangeType(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            return options.Encoding.GetChars(ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(input, setup));
        }
    }
}