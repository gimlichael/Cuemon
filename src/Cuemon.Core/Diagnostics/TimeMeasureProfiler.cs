using System;
using System.Diagnostics;
using System.Text;

namespace Cuemon.Diagnostics
{
    /// <summary>
    /// Represents a profiler that is optimized for time measuring operations.
    /// </summary>
    /// <seealso cref="Profiler" />
    public class TimeMeasureProfiler : Profiler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasureProfiler"/> class.
        /// </summary>
        internal TimeMeasureProfiler()
        {
            Timer = new Stopwatch();
        }

        internal Stopwatch Timer { get; set; }

        /// <summary>
        /// Gets the total elapsed time measured by this profiler.
        /// </summary>
        /// <value>A read-only <see cref="TimeSpan"/> representing the total elapsed time measured by this profiler.</value>
        public TimeSpan Elapsed => Timer.Elapsed;

        /// <summary>
        /// Gets a value indicating whether this time measuring profiler is still running.
        /// </summary>
        /// <value><c>true</c> if this time measuring profiler is still running; otherwise, <c>false</c>.</value>
        public bool IsRunning => Timer.IsRunning;

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("{0} took {1:D2}:{2:D2}:{3:D2}.{4:D3}{5:D3}{6:D3} to execute.".FormatWith(Member,
                Elapsed.Hours,
                Elapsed.Minutes,
                Elapsed.Seconds,
                Elapsed.Milliseconds,
                Elapsed.GetMicroseconds(),
                Elapsed.GetNanoseconds()));
            if (Data.Count > 0)
            {
                result.Append(" Parameters: { ");
                result.Append(Data.ToDelimitedString(", ", pair => "{0}={1}".FormatWith(pair.Key, StringConverter.FromObject(pair.Value))));
                result.Append(" }");
            }
            return result.ToString();
        }
    }

    /// <summary>
    /// Represents a profiler that is optimized for time measuring operations that provides a return value. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <seealso cref="TimeMeasureProfiler" />
    public sealed class TimeMeasureProfiler<TResult> : TimeMeasureProfiler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasureProfiler{TResult}"/> class.
        /// </summary>
        internal TimeMeasureProfiler()
        {
        }

        /// <summary>
        /// Gets or sets the result of a time measuring operation that returned a value.
        /// </summary>
        /// <value>The result of a time measuring operation that returned a value.</value>
        public TResult Result { get; set; }
    }
}