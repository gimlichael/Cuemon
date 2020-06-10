using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Globalization
{
    /// <summary>
    /// This static class is designed to make <see cref="System.Globalization"/> operations easier to work with.
    /// </summary>
    public static class World
    {
        private const int CultureTypesSpecificCultures = 2;
        private static readonly MethodInfo CultureInfoGetCultures = typeof(CultureInfo).GetMethod("GetCultures", new MemberReflection(excludeInheritancePath: true));

        internal static readonly Lazy<IEnumerable<CultureInfo>> SpecificCultures = new Lazy<IEnumerable<CultureInfo>>(() =>
        {
            var cultures = new SortedList<string, CultureInfo>();
            var specificCultures = CultureInfoGetCultures?.Invoke(null, new object[] { CultureTypesSpecificCultures }) as CultureInfo[];
            if (specificCultures != null)
            {
                foreach (var c in specificCultures)
                {
                    cultures.Add(c.DisplayName, c);
                }
                return cultures.Values;
            }

            using (var lfdSpecificCultures = Decorator.Enclose(typeof(World).Assembly).GetManifestResources("CultureInfo.SpecificCultures.dsv", ManifestResourceMatch.ContainsName).Values.Single())
            {
                using (var reader = new StreamReader(lfdSpecificCultures))
                {
                    string specificCulture;
                    while ((specificCulture = reader.ReadLine()) != null)
                    {
                        try
                        {
                            var c = new CultureInfo(specificCulture);
                            cultures.Add(c.DisplayName, c);
                        }
                        catch (CultureNotFoundException)
                        {
                            // ignored on systems not supporting the specificCulture
                        }
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