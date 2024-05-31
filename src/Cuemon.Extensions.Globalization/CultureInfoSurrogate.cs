namespace Cuemon.Extensions.Globalization
{
    internal class CultureInfoSurrogate
    {
        internal CultureInfoSurrogate()
        {
        }

        internal CultureInfoSurrogate(DateTimeFormatInfoSurrogate dateTimeFormat, NumberFormatInfoSurrogate numberFormat)
        {
            DateTimeFormat = dateTimeFormat;
            NumberFormat = numberFormat;
        }

        internal DateTimeFormatInfoSurrogate DateTimeFormat { get; set; }

        internal NumberFormatInfoSurrogate NumberFormat { get; set; }
    }
}
