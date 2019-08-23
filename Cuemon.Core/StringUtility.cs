using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.ComponentModel.Parsers;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="string"/> operations easier to work with.
    /// </summary>
    public static class StringUtility
    {
        #region Methods
        /// <summary>
        /// Parses whether the two specified values has a distinct difference from each other.
        /// </summary>
        /// <param name="definite">The value that specifies valid characters.</param>
        /// <param name="arbitrary">The value to distinctively compare with <paramref name="definite"/>.</param>
        /// <param name="difference">The distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/> or <see cref="string.Empty"/> if no difference.</param>
        /// <returns>
        /// 	<c>true</c> if there is a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>; otherwise <c>false</c>.
        /// </returns>
        public static bool ParseDistinctDifference(string definite, string arbitrary, out string difference)
        {
            if (definite == null) { definite = string.Empty; }
            if (arbitrary == null) { arbitrary = string.Empty; }
            difference = string.Concat(arbitrary.Distinct().Except(definite.Distinct()));
            return difference.Any();
        }
        #endregion
    }
}