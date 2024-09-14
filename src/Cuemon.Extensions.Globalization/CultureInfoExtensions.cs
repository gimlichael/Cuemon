using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.YamlDotNet.Formatters;
using Cuemon.Reflection;
using YamlDotNet.Serialization.NamingConventions;

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
        /// <remarks>Replaces the value on the public writable properties of <see cref="CultureInfo.DateTimeFormat"/> and <see cref="CultureInfo.NumberFormat"/> to favor NLS (https://learn.microsoft.com/en-us/windows/win32/intl/national-language-support) instead of ICU (http://site.icu-project.org/home).</remarks>
        public static CultureInfo UseNationalLanguageSupport(this CultureInfo culture)
        {
            Validator.ThrowIfNull(culture);
            return UseNationalLanguageSupport(Arguments.ToEnumerableOf(culture)).SingleOrDefault();
        }

        /// <summary>
        /// Enriches the specified <paramref name="cultures"/> with the original Windows variant.
        /// </summary>
        /// <param name="cultures">The sequence of <see cref="CultureInfo"/> to extend.</param>
        /// <returns>A sequence of <see cref="CultureInfo"/> objects enriched with the original Windows variant.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cultures"/> is null.
        /// </exception>
        /// <remarks>Replaces the value on the public writable properties of <see cref="CultureInfo.DateTimeFormat"/> and <see cref="CultureInfo.NumberFormat"/> to favor NLS (https://learn.microsoft.com/en-us/windows/win32/intl/national-language-support) instead of ICU (http://site.icu-project.org/home).</remarks>
        public static IEnumerable<CultureInfo> UseNationalLanguageSupport(this IEnumerable<CultureInfo> cultures)
        {
            Validator.ThrowIfNull(cultures);

            var enrichedCultures = new List<CultureInfo>();
            foreach (var culture in cultures)
            {
                var enrichedCulture = EnrichedCultureInfos.Find(ci => ci.Name.Equals(culture.Name, StringComparison.Ordinal));
                if (enrichedCulture != null)
                {
                    enrichedCultures.Add(enrichedCulture);
                }
                else
                {
                    var surrogate = typeof(CultureInfoExtensions).GetEmbeddedResources($"{culture.Name}.bin", ManifestResourceMatch.ContainsName).SingleOrDefault();
                    var ms = new MemoryStream(surrogate.Value.DecompressGZip().ToByteArray());
                    var suggogateCulture = YamlFormatter.DeserializeObject<CultureInfoSurrogate>(ms, o =>
                    {
                        o.Settings.NamingConvention = NullNamingConvention.Instance;
                        o.Settings.ReflectionRules = new MemberReflection();
                        o.Settings.IndentSequences = false;
                    });
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
            culture.DateTimeFormat.ShortestDayNames = surrogate.DateTimeFormat.ShortestDayNames;
            culture.DateTimeFormat.AbbreviatedDayNames = surrogate.DateTimeFormat.AbbreviatedDayNames;
            culture.DateTimeFormat.AbbreviatedMonthNames = surrogate.DateTimeFormat.AbbreviatedMonthNames;
            culture.DateTimeFormat.AbbreviatedMonthGenitiveNames = surrogate.DateTimeFormat.AbbreviatedMonthGenitiveNames;

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
