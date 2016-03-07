using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="NumberUtility"/> class.
    /// </summary>
    public static class NumberUtilityExtension
    {
        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="source">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(this IEnumerable<int> source)
        {
            return NumberUtility.IsCountableSequence(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="source">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(this IEnumerable<long> source)
        {
            return NumberUtility.IsCountableSequence(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an even number.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is an even number; otherwise, <c>false</c>.</returns>
        public static bool IsEven(this int value)
        {
            return NumberUtility.IsEven(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an odd number.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is an odd number; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(this int value)
        {
            return NumberUtility.IsOdd(value);
        }

        /// <summary>
        /// Rounds a double-precision floating-point value to the nearest integral value closest to the specified <paramref name="accuracy"/>.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded.</param>
        /// <param name="accuracy">The accuracy to use in the rounding.</param>
        /// <returns>
        /// The integer value closest to the specified <paramref name="accuracy"/> of <paramref name="value"/>.<br/>
        /// Note that this method returns a <see cref="double"/> instead of an integral type.
        /// </returns>
        public static double RoundOff(this double value, RoundOffAccuracy accuracy)
        {
            return NumberUtility.RoundOff(value, accuracy);
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in kilobytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A kilobyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToKilobytes(this long bytes)
        {
            return NumberUtility.BytesToKilobytes(bytes);
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in megabytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A megabyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToMegabytes(this long bytes)
        {
            return NumberUtility.BytesToMegabytes(bytes);
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in gigabytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A gigabyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToGigabytes(this long bytes)
        {
            return NumberUtility.BytesToGigabytes(bytes);
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in terabytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A terabyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToTerabytes(this long bytes)
        {
            return NumberUtility.BytesToTerabytes(bytes);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a prime number.
        /// </summary>
        /// <param name="value">The positive integer to determine whether is a prime number.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a prime number; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="value"/> has a value smaller than 0.
        /// </exception>
        public static bool IsPrime(this int value)
        {
            return NumberUtility.IsPrime(value);
        }

        /// <summary>
        /// Calculates the factorial of a positive integer <paramref name="n"/> denoted by n!.
        /// </summary>
        /// <param name="n">The positive integer to calculate a factorial number by.</param>
        /// <returns>The factorial number calculated from <paramref name="n"/>, or <see cref="double.PositiveInfinity"/> if <paramref name="n"/> is to high a value.</returns>
        public static double Factorial(this double n)
        {
            return NumberUtility.Factorial(n);
        }
    }
}