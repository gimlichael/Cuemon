using System;
using System.Globalization;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Represents a <see cref="DateTime"/> interval between two <see cref="DateTime"/> values.
    /// </summary>
    public readonly struct DateSpan : IEquatable<DateSpan>
    {
        private readonly DateTime _lower;
        private readonly DateTime _upper;
        private readonly Calendar _calendar;

        /// <summary>
		/// Initializes a new instance of the <see cref="DateSpan"/> structure with a default <see cref="DateTime"/> value set to <see cref="DateTime.Today"/>.
		/// </summary>
		/// <param name="start">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
		public DateSpan(DateTime start) : this(start, DateTime.Today)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateSpan"/> structure with a default <see cref="_calendar"/> value from the <see cref="CultureInfo.InvariantCulture"/> class.
        /// </summary>
        /// <param name="start">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        /// <param name="end">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        public DateSpan(DateTime start, DateTime end) : this(start, end, CultureInfo.InvariantCulture.Calendar)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateSpan"/> structure.
        /// </summary>
        /// <param name="start">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        /// <param name="end">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        /// <param name="calendar">The <see cref="Calendar"/> that applies to this <see cref="DateSpan"/>.</param>
        public DateSpan(DateTime start, DateTime end, Calendar calendar) : this()
        {
            Validator.ThrowIfNull(calendar, nameof(calendar));

            _lower = Arguments.ToEnumerableOf(start, end).Min();
            _upper = Arguments.ToEnumerableOf(start, end).Max();
            _calendar = calendar;

            var lower = _lower;
            var upper = _upper;

            var months = 0;
            var days = 0;

            var years = upper.Year == lower.Year ? 0 : upper.Year - lower.Year;
            var hours = upper.Hour == lower.Hour ? 0 : upper.Hour - lower.Hour;
            var minutes = upper.Minute == lower.Minute ? 0 : upper.Minute - lower.Minute;
            var seconds = upper.Second == lower.Second ? 0 : upper.Second - lower.Second;
            var milliseconds = upper.Millisecond == lower.Millisecond ? 0 : upper.Millisecond - lower.Millisecond;

            int daysPerYears;
            var y = lower.Year;
            do
            {
                daysPerYears = _calendar.GetDaysInYear(y);
                y++;
            } while (y < upper.Year);

            while (!lower.Year.Equals(upper.Year) || !lower.Month.Equals(upper.Month))
            {
                var daysPerMonth = _calendar.GetDaysInMonth(lower.Year, lower.Month);
                days += daysPerMonth;
                lower = lower.AddMonths(1);
                months++;
            }

            while (!lower.Day.Equals(upper.Day))
            {
                days++;
                lower = lower.AddDays(1);
            }

            var averageDaysPerMonth = months == 0 ? days : Convert.ToDouble(days) / Convert.ToDouble(months);
            var remainder = new TimeSpan(days, hours, minutes, seconds, milliseconds);

            Years = years;
            Months = months;
            Days = days;
            Hours = remainder.Hours;
            Minutes = remainder.Minutes;
            Seconds = remainder.Seconds;
            Milliseconds = remainder.Milliseconds;
            Ticks = remainder.Ticks;

            TotalYears = remainder.TotalDays / daysPerYears;
            TotalMonths = remainder.TotalDays / averageDaysPerMonth;
            TotalDays = remainder.TotalDays;
            TotalHours = remainder.TotalHours;
            TotalMinutes = remainder.TotalMinutes;
            TotalSeconds = remainder.TotalSeconds;
            TotalMilliseconds = remainder.TotalMilliseconds;
        }

        /// <summary>
		/// Calculates the number of weeks represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>Calculates the number of weeks represented by the current <see cref="DateSpan"/> structure.</value>
		public int GetWeeks()
        {
            var range = _upper.Subtract(_lower);
            var totalDays = 0;
            if (range.Days <= 7)
            {
                totalDays = _lower.DayOfWeek > _upper.DayOfWeek ? 2 : 1;
            }
            if (totalDays == 0) { totalDays = range.Days - 7 + (int)_lower.DayOfWeek; }
            int nextWeek = 0, weeks;
            for (weeks = 1; nextWeek < totalDays; weeks++) { nextWeek += 7; }
            return weeks;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Generate.HashCode32(_upper, _lower, _calendar.GetType().FullName);
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
            if (!(obj is DateSpan)) { return false; }
            return Equals((DateSpan)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(DateSpan other)
        {
            if ((_upper != other._upper) || (_calendar != other._calendar)) { return false; }
            return (_lower == other._lower);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateSpan"/> instances are equal.
        /// </summary>
        /// <param name="dateSpan1">The first date interval to compare.</param>
        /// <param name="dateSpan2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="dateSpan1"/> and <paramref name="dateSpan2"/> are equal; otherwise, false. </returns>
        public static bool operator ==(DateSpan dateSpan1, DateSpan dateSpan2)
        {
            return dateSpan1.Equals(dateSpan2);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateSpan"/> instances are not equal.
        /// </summary>
        /// <param name="dateSpan1">The first date interval to compare.</param>
        /// <param name="dateSpan2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="dateSpan1"/> and <paramref name="dateSpan2"/> are not equal; otherwise, false.</returns>
        public static bool operator !=(DateSpan dateSpan1, DateSpan dateSpan2)
        {
            return !dateSpan1.Equals(dateSpan2);
        }

        /// <summary>
        /// Constructs a new <see cref="DateSpan"/> object from a date and time interval specified in a string.
        /// </summary>
        /// <param name="start">A string that specifies the starting date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <returns>A <see cref="DateSpan"/> that corresponds to <paramref name="start"/> and <see cref="DateTime.Today"/> for the last part of the interval.</returns>
        public static DateSpan Parse(string start)
        {
            return Parse(start, DateTime.Today.ToString("s", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Constructs a new <see cref="DateSpan"/> object from a date and time interval specified in a string.
        /// </summary>
        /// <param name="start">A string that specifies the starting date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <param name="end">A string that specifies the ending date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <returns>A <see cref="DateSpan"/> that corresponds to <paramref name="start"/> and <paramref name="end"/> of the interval.</returns>
        public static DateSpan Parse(string start, string end)
        {
            return Parse(start, end, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Constructs a new <see cref="DateSpan"/> object from a date and time interval specified in a string.
        /// </summary>
        /// <param name="start">A string that specifies the starting date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <param name="end">A string that specifies the ending date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> to resolve a <see cref="_calendar"/> object from.</param>
        /// <returns>A <see cref="DateSpan"/> that corresponds to <paramref name="start"/> and <paramref name="end"/> of the interval.</returns>
        public static DateSpan Parse(string start, string end, CultureInfo culture)
        {
            return new DateSpan(DateTime.Parse(start, culture), DateTime.Parse(end, culture), culture.Calendar);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateSpan"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representation of the current <see cref="DateSpan"/> value. 
        /// </returns>
        /// <remarks>The returned string has the following format: y*:MM:dd:hh:mm:ss.f*, where y* is the actual calculated years and f* is the actual calculated milliseconds.</remarks>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1:D2}:{2:D2}:{3:D2}:{4:D2}:{5:D2}.{6}", Years, Months, Days, Hours, Minutes, Seconds, Milliseconds);
        }

        /// <summary>
        /// Gets the number of days represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of days represented by the current <see cref="DateSpan"/> structure.</value>
        public int Days { get; }

        /// <summary>
        /// Gets the total number of days represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of days represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalDays { get; }

        /// <summary>
        /// Gets the number of hours represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of hours represented by the current <see cref="DateSpan"/> structure.</value>
        public int Hours { get; }

        /// <summary>
        /// Gets the total number of hours represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of hours represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalHours { get; }

        /// <summary>
        /// Gets the number of milliseconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of milliseconds represented by the current <see cref="DateSpan"/> structure.</value>
        public int Milliseconds { get; }

        /// <summary>
        /// Gets the total number of milliseconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of milliseconds represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalMilliseconds { get; }

        /// <summary>
        /// Gets the number of minutes represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of minutes represented by the current <see cref="DateSpan"/> structure.</value>
        public int Minutes { get; }

        /// <summary>
        /// Gets the total number of minutes represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of minutes represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalMinutes { get; }

        /// <summary>
        /// Gets the number of months represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of months represented by the current <see cref="DateSpan"/> structure.</value>
        public int Months { get; }

        /// <summary>
        /// Gets the total number of months represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of months represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalMonths { get; }

        /// <summary>
        /// Gets the number of seconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of seconds represented by the current <see cref="DateSpan"/> structure.</value>
        public int Seconds { get; }

        /// <summary>
        /// Gets the total number of seconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of seconds represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalSeconds { get; }

        /// <summary>
        /// Gets the number of ticks represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of ticks represented by the current <see cref="DateSpan"/> structure.</value>
        public long Ticks { get; }

        /// <summary>
        /// Gets the number of years represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of years represented by the current <see cref="DateSpan"/> structure.</value>
        public int Years { get; }

        /// <summary>
        /// Gets the total number of years represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of years represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalYears { get; }
    }
}