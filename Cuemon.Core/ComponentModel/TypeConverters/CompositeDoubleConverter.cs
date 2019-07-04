using System;
using System.ComponentModel;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="double"/> to its equivalent <see cref="TimeSpan"/>.
    /// </summary>
    public class CompositeDoubleConverter : IConverter<double, TimeSpan, CompositeDoubleOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> combined with <see cref="TimeUnit"/> to its equivalent <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to be converted into a <see cref="TimeSpan"/>.</param>
        /// <param name="setup">The <see cref="CompositeDoubleOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="TimeSpan"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="input" /> is either lower than <see cref="long.MinValue"/> or greater than <see cref="long.MaxValue"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="TimeUnit"/>.
        /// </exception>
        public TimeSpan ChangeType(double input, Action<CompositeDoubleOptions> setup)
        {
            if (input == 0.0) { return TimeSpan.Zero; }
            var options = Patterns.Configure(setup);
            switch (options.TimeUnit)
            {
                case TimeUnit.Days:
                    return TimeSpan.FromDays(input);
                case TimeUnit.Hours:
                    return TimeSpan.FromHours(input);
                case TimeUnit.Minutes:
                    return TimeSpan.FromMinutes(input);
                case TimeUnit.Seconds:
                    return TimeSpan.FromSeconds(input);
                case TimeUnit.Milliseconds:
                    return TimeSpan.FromMilliseconds(input);
                case TimeUnit.Ticks:
                    if (input < long.MinValue || input > long.MaxValue) { throw new OverflowException(FormattableString.Invariant($"The specified input, {input}, having a time unit specified as Ticks cannot be less than {long.MinValue} or be greater than {long.MaxValue}.")); }
                    return TimeSpan.FromTicks((long)input);
                default:
                    throw new InvalidEnumArgumentException(nameof(options.TimeUnit), (int)options.TimeUnit, typeof(TimeUnit));
            }
        }
    }
}