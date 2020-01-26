using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Provides a roughly way to determine if a given part of day is either; Night, Morning, Forenoon, Afternoon og Evening.
    /// </summary>
    public static class DayParts
    {
        private static readonly DateTime Midnight = DateTime.UtcNow.Date;

        /// <summary>
        /// Gets the day part of a 24-hour period that approximates to Night.
        /// </summary>
        /// <value>The range of a 24-hour period that approximates to Night.</value>
        public static DayPart Night => new DayPart("Night", new TimeRange(Midnight, Midnight.AddHours(5)));

        /// <summary>
        /// Gets the day part of a 24-hour period that approximates to Morning.
        /// </summary>
        /// <value>The range of a 24-hour period that approximates to Morning.</value>
        public static DayPart Morning => new DayPart("Morning", new TimeRange(Night.Range.End, Midnight.AddHours(9)));

        /// <summary>
        /// Gets the day part of a 24-hour period that approximates to Forenoon.
        /// </summary>
        /// <value>The range of a 24-hour period that approximates to Forenoon.</value>
        public static DayPart Forenoon => new DayPart("Forenoon", new TimeRange(Morning.Range.End, Midnight.AddHours(12)));

        /// <summary>
        /// Gets the day part of a 24-hour period that approximates to Afternoon.
        /// </summary>
        /// <value>The range of a 24-hour period that approximates to Afternoon.</value>
        public static DayPart Afternoon => new DayPart("Afternoon", new TimeRange(Forenoon.Range.End, Midnight.AddHours(18)));

        /// <summary>
        /// Gets the day part of a 24-hour period that approximates to Evening.
        /// </summary>
        /// <value>The range of a 24-hour period that approximates to Evening.</value>
        public static DayPart Evening => new DayPart("Evening", new TimeRange(Afternoon.Range.End, Midnight.AddHours(24)));

        /// <summary>
        /// Gets the day parts of a 24-hour range of period.
        /// </summary>
        /// <value>The day parts of a 24-hour range of period.</value>
        public static IEnumerable<DayPart> All
        {
            get
            {
                yield return Night;
                yield return Morning;
                yield return Forenoon;
                yield return Afternoon;
                yield return Evening;
            }
        }
    }
}