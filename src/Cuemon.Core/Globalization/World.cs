using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cuemon.Reflection;

namespace Cuemon.Globalization
{
    /// <summary>
    /// This static class is designed to make <see cref="System.Globalization"/> operations easier to work with.
    /// </summary>
    public static class World
    {
        internal static readonly Lazy<IEnumerable<CultureInfo>> SpecificCultures = new Lazy<IEnumerable<CultureInfo>>(() =>
        {
            var cultures = new SortedList<string, CultureInfo>();
            using (var lfdSpecificCultures = ReflectionUtility.GetEmbeddedResource(typeof(World), "CultureInfo.SpecificCultures.dsv", ResourceMatch.ContainsName))
            {
                using (var reader = new StreamReader(lfdSpecificCultures))
                {
                    string specificCulture;
                    while ((specificCulture = reader.ReadLine()) != null)
                    {
                        var c = new CultureInfo(specificCulture);
                        cultures.Add(c.DisplayName, c);
                    }
                }
            }
            return cultures.Values;
        });

        private static readonly Lazy<IEnumerable<RegionInfo>> SpecificRegions = new Lazy<IEnumerable<RegionInfo>>(() =>
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
    }
}