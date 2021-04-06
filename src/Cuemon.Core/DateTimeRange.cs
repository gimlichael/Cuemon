using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// Represents a period of time between two <see cref="DateTime"/> values.
    /// </summary>
    public readonly struct DateTimeRange : IEquatable<DateTimeRange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
        /// </summary>
        /// <param name="start">The start date of a time range.</param>
        /// <param name="end">The end date of a time range.</param>
        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
            Duration = end.Subtract(start);
        }

        /// <summary>
        /// Gets the point of time where this time range begin.
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing the point of time where this time range begin.</value>
        public DateTime Start { get; }

        /// <summary>
        /// Gets the point of time where this time range end.
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing the point of time where this time range end.</value>
        public DateTime End { get; }

        /// <summary>
        /// Gets the duration between <see cref="Start"/> and <see cref="End"/>.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the duration between <see cref="Start"/> and <see cref="End"/>.</value>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DateTimeRange)) { return false; }
            return Equals((DateTimeRange)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(DateTimeRange other)
        {
            return (Start == other.Start && End == other.End);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateTimeRange"/> instances are equal.
        /// </summary>
        /// <param name="range1">The first time period to compare.</param>
        /// <param name="range2">The second time period to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="range1"/> and <paramref name="range2"/> are equal; otherwise, false. </returns>
        public static bool operator ==(DateTimeRange range1, DateTimeRange range2)
        {
            return range1.Equals(range2);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateTimeRange"/> instances are not equal.
        /// </summary>
        /// <param name="range1">The first time period to compare.</param>
        /// <param name="range2">The second time period to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="range1"/> and <paramref name="range2"/> are not equal; otherwise, false.</returns>
        public static bool operator !=(DateTimeRange range1, DateTimeRange range2)
        {
            return !range1.Equals(range2);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("s", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="format">A composite format string for the <see cref="DateTime"/> properties.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            FormattableString fs = $"A duration of {Duration.Days:D2}.{Duration.Hours:D2}:{Duration.Minutes:D2}:{Duration.Seconds:D2} between {Start.ToString(format, provider)} and {End.ToString(format, provider)}.";
            return fs.ToString(provider);
        }
    }
}