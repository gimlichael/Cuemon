using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cuemon.Globalization
{
    /// <summary>
    /// This static class is designed to make <see cref="System.Globalization"/> operations easier to work with.
    /// </summary>
    public static class World
    {
        internal static readonly Lazy<IEnumerable<CultureInfo>> SpecificCultures = new(() =>
        {
            var cultures = new SortedList<string, CultureInfo>();
            var specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (var c in specificCultures)
            {
                cultures.Add(c.DisplayName, c);
            }
            return cultures.Values;
        });

        private static readonly Lazy<IEnumerable<RegionInfo>> SpecificRegions = new(() =>
        {
            var regions = new SortedList<string, RegionInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (var c in SpecificCultures.Value)
            {
                var region = new RegionInfo(c.Name);
                if (!regions.ContainsKey(region.EnglishName)) { regions.Add(region.EnglishName, region); }
            }
            return regions.Values;
        });

        /// <summary>
        /// Gets the by .NET specific regions of the world.
        /// </summary>
        /// <value>The .NET specific regions of the world.</value>
        public static IEnumerable<RegionInfo> Regions { get; } = SpecificRegions.Value;

        /// <summary>
        /// Resolves a sequence of related <see cref="CultureInfo"/> objects for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region to resolve a sequence of <see cref="CultureInfo"/> objects from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="CultureInfo"/> objects.</returns>
        public static IEnumerable<CultureInfo> GetCultures(RegionInfo region)
        {
            Validator.ThrowIfNull(region, nameof(region));
            return SpecificCultures.Value.Where(c => c.Name.EndsWith(region.TwoLetterISORegionName));
        }
    }
}