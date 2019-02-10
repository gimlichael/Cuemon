using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cuemon.Globalization
{
    /// <summary>
    /// This is an extension implementation of the <see cref="RegionInfo"/> class.
    /// </summary>
    public static class RegionInfoExtensions
    {
        /// <summary>
        /// Resolves a sequence of related <see cref="CultureInfo"/> objects for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region to resolve a sequence of <see cref="CultureInfo"/> objects from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="CultureInfo"/> objects.</returns>
        public static IEnumerable<CultureInfo> GetCultures(this RegionInfo region)
        {
            Validator.ThrowIfNull(region, nameof(region));
            return World.SpecificCultures.Value.Where(c => c.Name.EndsWith(region.TwoLetterISORegionName));
        }
    }
}