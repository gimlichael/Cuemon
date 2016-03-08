using System;
using System.Globalization;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon
{
	/// <summary>
	/// Represents a <see cref="DateTime"/> interval between two <see cref="DateTime"/> values.
	/// </summary>
	public struct DateSpan : IEquatable<DateSpan>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DateSpan"/> structure with a default <see cref="DateTime"/> value set to <see cref="DateTime.Today"/>.
		/// </summary>
		/// <param name="start">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
		public DateSpan(DateTime start) : this(start, DateTime.Today)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DateSpan"/> structure with a default <see cref="Calendar"/> value from the <see cref="CultureInfo.InvariantCulture"/> class.
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
            if (calendar == null) { throw new ArgumentNullException(nameof(calendar)); }

            Highest = EnumerableConverter.FromArray(start, end).Max();
            Lowest = EnumerableConverter.FromArray(start, end).Min();
            Calendar = calendar;

            int totalMonths, deltaMonths;
            GetMonths(out deltaMonths, out totalMonths);

			TimeSpan timespan = (Highest - Lowest);
			Hours = timespan.Hours;
			TotalHours = (long)timespan.TotalHours;
			Milliseconds = timespan.Milliseconds;
			TotalMilliseconds = (long)timespan.TotalMilliseconds;
			Minutes = timespan.Minutes;
			TotalMinutes = (long)timespan.TotalMinutes;
		    Months = deltaMonths;
            TotalMonths = totalMonths;
		    Days = Highest.Day;
			TotalDays = (int)Math.Floor(timespan.TotalDays);
			Seconds = timespan.Seconds;
			TotalSeconds = (long)timespan.TotalSeconds;
			Ticks = timespan.Ticks;
			Years = GetYears(Highest, Lowest);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Calculates the number of weeks represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>Calculates the number of weeks represented by the current <see cref="DateSpan"/> structure.</value>
		public int GetWeeks()
		{
			TimeSpan range = Highest.Subtract(Lowest);
			int totalDays = 0;
			if (range.Days <= 7)
			{
				totalDays = Lowest.DayOfWeek > Highest.DayOfWeek ? 2 : 1;
			}
			if (totalDays == 0) { totalDays = range.Days - 7 + (int)Lowest.DayOfWeek; }
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
			return Highest.GetHashCode() ^ Lowest.GetHashCode() ^ Calendar.GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
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
			if ((Highest != other.Highest) || (Calendar != other.Calendar)) { return false; }
			return (Lowest == other.Lowest);
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
        /// <param name="culture">A <see cref="CultureInfo"/> to resolve a <see cref="Calendar"/> object from.</param>
        /// <returns>A <see cref="DateSpan"/> that corresponds to <paramref name="start"/> and <paramref name="end"/> of the interval.</returns>
        public static DateSpan Parse(string start, string end, CultureInfo culture)
		{
			return new DateSpan(DateTime.Parse(start, culture), DateTime.Parse(end, culture), culture.Calendar);
		}

        private void GetMonths(out int deltaMonths, out int totalMonths)
		{
			totalMonths = 0;
            deltaMonths = Highest.Month;
			for (int year = Lowest.Year; year < Highest.Year; year++)
			{
				totalMonths += Calendar.GetMonthsInYear(year);
			}
            totalMonths += deltaMonths;
		}

		private int GetYears(DateTime start, DateTime end)
		{
			int years = start.Year - end.Year;
			if (start.DayOfYear < end.DayOfYear)
			{
				years--;
			}
			else if (start.DayOfYear == end.DayOfYear)
			{
				if (start.TimeOfDay < end.TimeOfDay)
				{
					years--;
				}
			}
			return years;
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
		#endregion

		#region Properties
		/// <summary>
		/// Gets the calendar represented by the current <see cref="DateSpan"/> structure. 
		/// The default value is taken from the <see cref="CultureInfo.InvariantCulture"/> class.
		/// </summary>
		/// <value>A <see cref="Calendar"/> that represents the current <see cref="DateSpan"/> structure.</value>
		private Calendar Calendar { get; set; }

		private DateTime Highest { get; set; }

		private DateTime Lowest { get; set; }

		/// <summary>
		/// Gets the number of days represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of days represented by the current <see cref="DateSpan"/> structure.</value>
		public int Days { get; private set; }

		/// <summary>
		/// Gets the total number of days represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The total number of days represented by the current <see cref="DateSpan"/> structure.</value>
		public int TotalDays { get; private set; }

		/// <summary>
		/// Gets the number of hours represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of hours represented by the current <see cref="DateSpan"/> structure.</value>
		public int Hours { get; private set; }

		/// <summary>
		/// Gets the total number of hours represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The total number of hours represented by the current <see cref="DateSpan"/> structure.</value>
		public long TotalHours { get; private set; }

		/// <summary>
		/// Gets the number of milliseconds represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of milliseconds represented by the current <see cref="DateSpan"/> structure.</value>
		public int Milliseconds { get; private set; }

		/// <summary>
		/// Gets the total number of milliseconds represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The total number of milliseconds represented by the current <see cref="DateSpan"/> structure.</value>
		public long TotalMilliseconds { get; private set; }

		/// <summary>
		/// Gets the number of minutes represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of minutes represented by the current <see cref="DateSpan"/> structure.</value>
		public int Minutes { get; private set; }

		/// <summary>
		/// Gets the total number of minutes represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The total number of minutes represented by the current <see cref="DateSpan"/> structure.</value>
		public long TotalMinutes { get; private set; }

		/// <summary>
		/// Gets the number of months represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of months represented by the current <see cref="DateSpan"/> structure.</value>
		public int Months { get; private set; }

		/// <summary>
		/// Gets the total number of months represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The total number of months represented by the current <see cref="DateSpan"/> structure.</value>
		public int TotalMonths { get; private set; }

		/// <summary>
		/// Gets the number of seconds represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of seconds represented by the current <see cref="DateSpan"/> structure.</value>
		public int Seconds { get; private set; }

		/// <summary>
		/// Gets the total number of seconds represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The total number of seconds represented by the current <see cref="DateSpan"/> structure.</value>
		public long TotalSeconds { get; private set; }

		/// <summary>
		/// Gets the number of ticks represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of ticks represented by the current <see cref="DateSpan"/> structure.</value>
		public long Ticks { get; private set; }

		/// <summary>
		/// Gets the number of years represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>The number of years represented by the current <see cref="DateSpan"/> structure.</value>
		public int Years { get; private set; }
		#endregion
	}
}