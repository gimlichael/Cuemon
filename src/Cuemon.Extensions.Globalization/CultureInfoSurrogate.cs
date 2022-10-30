using System;
using System.Runtime.Serialization;

namespace Cuemon.Extensions.Globalization
{
    [Serializable]
    internal class CultureInfoSurrogate  : ISerializable
    {
        public CultureInfoSurrogate()
        {
        }

        internal CultureInfoSurrogate(DateTimeFormatInfoSurrogate dateTimeFormat, NumberFormatInfoSurrogate numberFormat)
        {
            DateTimeFormat = dateTimeFormat;
            NumberFormat = numberFormat;
        }

        internal CultureInfoSurrogate(SerializationInfo info, StreamingContext context)
        {
            DateTimeFormat = (DateTimeFormatInfoSurrogate)info.GetValue(nameof(DateTimeFormat), typeof(DateTimeFormatInfoSurrogate));
            NumberFormat = (NumberFormatInfoSurrogate)info.GetValue(nameof(NumberFormat), typeof(NumberFormatInfoSurrogate));
        }

        internal DateTimeFormatInfoSurrogate DateTimeFormat { get; set; }

        internal NumberFormatInfoSurrogate NumberFormat { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(DateTimeFormat), DateTimeFormat);
            info.AddValue(nameof(NumberFormat), NumberFormat);
        }
    }
}
