namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Condition"/> class.
    /// </summary>
    public static class ConditionExtensions
    {
        /// <summary>
        /// Determines whether there is a set difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="_">The <see cref="Condition"/> to extend.</param>
        /// <param name="first">The value where characters that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="difference">The set difference between <paramref name="second"/> and <paramref name="first"/> or <see cref="string.Empty"/> if no difference.</param>
        /// <returns>
        /// 	<c>true</c> if there is a set difference between <paramref name="second"/> and <paramref name="first"/>; otherwise <c>false</c>.
        /// </returns>
        public static bool HasDifference(this Condition _, string first, string second, out string difference)
        {
            difference = first.Difference(second);
            return difference.Length != 0;
        }
    }
}
