namespace Cuemon
{
    /// <summary>
    /// Represents a part of a day.
    /// </summary>
    public class DayPart
    {
        internal DayPart(string name, TimeRange range)
        {
            Name = name;
            Range = range;
        }

        /// <summary>
        /// Gets the name of a <see cref="DayPart"/>.
        /// </summary>
        /// <value>The name of a <see cref="DayPart"/>.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the approximate range that this <see cref="DayPart"/> represents.
        /// </summary>
        /// <value>The approximate range that this <see cref="DayPart"/> represents.</value>
        public TimeRange Range { get; set; }
    }
}