using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Globalization
{
    /// <summary>
    /// Extension methods for the <see cref="CultureInfo"/> class.
    /// </summary>
    public static class CultureInfoExtensions
    {
        private static readonly List<CultureInfo> EnrichedCultureInfos = new();

        /// <summary>
        /// Enriches the specified <paramref name="culture"/> with the original Windows variant.
        /// </summary>
        /// <param name="culture">The of <see cref="CultureInfo"/> to extend.</param>
        /// <returns>A <see cref="CultureInfo"/> object enriched with the original Windows variant.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="culture"/> is null.
        /// </exception>
        public static CultureInfo MergeWithOriginalFormatting(this CultureInfo culture)
        {
            Validator.ThrowIfNull(culture);
            return MergeWithOriginalFormatting(Arguments.ToEnumerableOf(culture)).SingleOrDefault();
        }

        /// <summary>
        /// Enriches the specified <paramref name="cultures"/> with the original Windows variant.
        /// </summary>
        /// <param name="cultures">The sequence of <see cref="CultureInfo"/> to extend.</param>
        /// <returns>A sequence of <see cref="CultureInfo"/> objects enriched with the original Windows variant.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cultures"/> is null.
        /// </exception>
        public static IEnumerable<CultureInfo> MergeWithOriginalFormatting(this IEnumerable<CultureInfo> cultures)
        {
            Validator.ThrowIfNull(cultures);

            var enrichedCultures = new List<CultureInfo>();
            foreach (var culture in cultures)
            {
                var enrichedCulture = EnrichedCultureInfos.Find(ci => ci.Name.Equals(culture.Name));
                if (enrichedCulture != null)
                {
                    enrichedCultures.Add(enrichedCulture);
                }
                else
                {
                    var surrogate = typeof(CultureInfoExtensions).GetEmbeddedResources($"{culture.Name}.bin", ManifestResourceMatch.ContainsName).SingleOrDefault();
                    var bf = new BinaryFormatter();
#pragma warning disable S5773 // Types allowed to be deserialized should be restricted
#pragma warning disable SYSLIB0011
                    var ms = new MemoryStream(surrogate.Value.DecompressGZip().ToByteArray());
                    var suggogateCulture = (CultureInfoSurrogate)bf.Deserialize(ms);
#pragma warning restore SYSLIB0011
#pragma warning restore S5773 // Types allowed to be deserialized should be restricted
                    Enrich(culture, suggogateCulture);
                    EnrichedCultureInfos.Add(culture);
                    enrichedCultures.Add(culture);
                }
            }
            return enrichedCultures;
        }

        private static void Enrich(CultureInfo culture, CultureInfoSurrogate surrogate)
        {
            culture.DateTimeFormat.AMDesignator = surrogate.DateTimeFormat.AMDesignator;
            culture.DateTimeFormat.CalendarWeekRule = surrogate.DateTimeFormat.CalendarWeekRule;
            culture.DateTimeFormat.DateSeparator = surrogate.DateTimeFormat.DateSeparator;
            culture.DateTimeFormat.FirstDayOfWeek = surrogate.DateTimeFormat.FirstDayOfWeek;
            culture.DateTimeFormat.FullDateTimePattern = surrogate.DateTimeFormat.FullDateTimePattern;
            culture.DateTimeFormat.LongDatePattern = surrogate.DateTimeFormat.LongDatePattern;
            culture.DateTimeFormat.MonthDayPattern = surrogate.DateTimeFormat.MonthDayPattern;
            culture.DateTimeFormat.LongTimePattern = surrogate.DateTimeFormat.LongTimePattern;
            culture.DateTimeFormat.PMDesignator = surrogate.DateTimeFormat.PMDesignator;
            culture.DateTimeFormat.ShortDatePattern = surrogate.DateTimeFormat.ShortDatePattern;
            culture.DateTimeFormat.ShortTimePattern = surrogate.DateTimeFormat.ShortTimePattern;
            culture.DateTimeFormat.TimeSeparator = surrogate.DateTimeFormat.TimeSeparator;
            culture.DateTimeFormat.YearMonthPattern = surrogate.DateTimeFormat.YearMonthPattern;

            culture.NumberFormat.CurrencyDecimalDigits = surrogate.NumberFormat.CurrencyDecimalDigits;
            culture.NumberFormat.CurrencyDecimalSeparator = surrogate.NumberFormat.CurrencyDecimalSeparator;
            culture.NumberFormat.CurrencyGroupSeparator = surrogate.NumberFormat.CurrencyGroupSeparator;
            culture.NumberFormat.CurrencyNegativePattern = surrogate.NumberFormat.CurrencyNegativePattern;
            culture.NumberFormat.CurrencyPositivePattern = surrogate.NumberFormat.CurrencyPositivePattern;
            culture.NumberFormat.CurrencySymbol = surrogate.NumberFormat.CurrencySymbol;
            culture.NumberFormat.DigitSubstitution = surrogate.NumberFormat.DigitSubstitution;
            culture.NumberFormat.NaNSymbol = surrogate.NumberFormat.NaNSymbol;
            culture.NumberFormat.NegativeInfinitySymbol = surrogate.NumberFormat.NegativeInfinitySymbol;
            culture.NumberFormat.NegativeSign = surrogate.NumberFormat.NegativeSign;
            culture.NumberFormat.NumberDecimalDigits = surrogate.NumberFormat.NumberDecimalDigits;
            culture.NumberFormat.NumberDecimalSeparator = surrogate.NumberFormat.NumberDecimalSeparator;
            culture.NumberFormat.NumberGroupSeparator = surrogate.NumberFormat.NumberGroupSeparator;
            culture.NumberFormat.NumberNegativePattern = surrogate.NumberFormat.NumberNegativePattern;
            culture.NumberFormat.PerMilleSymbol = surrogate.NumberFormat.PerMilleSymbol;
            culture.NumberFormat.PercentDecimalDigits = surrogate.NumberFormat.PercentDecimalDigits;
            culture.NumberFormat.PercentDecimalSeparator = surrogate.NumberFormat.PercentDecimalSeparator;
            culture.NumberFormat.PercentGroupSeparator = surrogate.NumberFormat.PercentGroupSeparator;
            culture.NumberFormat.PercentNegativePattern = surrogate.NumberFormat.PercentNegativePattern;
            culture.NumberFormat.PercentPositivePattern = surrogate.NumberFormat.PercentPositivePattern;
            culture.NumberFormat.PercentSymbol = surrogate.NumberFormat.PercentSymbol;
            culture.NumberFormat.PositiveInfinitySymbol = surrogate.NumberFormat.PositiveInfinitySymbol;
            culture.NumberFormat.PositiveSign = surrogate.NumberFormat.PositiveSign;
        }
    }
}
