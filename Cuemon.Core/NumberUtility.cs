using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make number operations easier to work with.
    /// </summary>
    public static class NumberUtility
    {
        [ThreadStatic]
        private static Random _simpleRandomizerLocal;
        private static readonly Random SimpleRandomizerGlobal = new Random();
        private static readonly RandomNumberGenerator StrongRandomizerGlobal = RandomNumberGenerator.Create();

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="source">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(IEnumerable<int> source)
        {
            return IsCountableSequence(source.Select(CastAsIntegralSequence));
        }

        private static long CastAsIntegralSequence(int i)
        {
            return i;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="source">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(IEnumerable<long> source)
        {
            if (source == null) { return false; }
            var numbers = new List<long>(source);

            var x = numbers[0];
            var y = numbers[1];

            var difference = y - x;
            for (var i = 2; i < numbers.Count; i++)
            {
                x = numbers[i];
                y = numbers[i - 1];
                if ((x - y) != difference) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an even number.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is an even number; otherwise, <c>false</c>.</returns>
        public static bool IsEven(int value)
        {
            return ((value % 2) == 0);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an odd number.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is an odd number; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(int value)
        {
            return !IsEven(value);
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
        public static double RoundOff(double value, RoundOffAccuracy accuracy)
        {
            return Math.Round(value / (long)accuracy) * (long)accuracy;
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in kilobytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A kilobyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToKilobytes(long bytes)
        {
            return bytes / 1024f;
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in megabytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A megabyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToMegabytes(long bytes)
        {
            return BytesToKilobytes(bytes) / 1024f;
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in gigabytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A gigabyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToGigabytes(long bytes)
        {
            return BytesToMegabytes(bytes) / 1024f;
        }

        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent representation in terabytes.
        /// </summary>
        /// <param name="bytes">The bytes to be converted.</param>
        /// <returns>A terabyte representation equivalent to the specified <paramref name="bytes"/>.</returns>
        public static double BytesToTerabytes(long bytes)
        {
            return BytesToGigabytes(bytes) / 1024f;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a prime number.
        /// </summary>
        /// <param name="value">The positive integer to determine whether is a prime number.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a prime number; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> has a value smaller than 0.
        /// </exception>
        public static bool IsPrime(int value)
        {
            if (value < 0) { throw new ArgumentException("Value must have a value equal or higher than 0.", nameof(value)); }
            if ((value & 1) == 0) { return value == 2; }
            for (long i = 3; (i * i) <= value; i += 2)
            {
                if ((value % i) == 0) { return false; }
            }
            return value != 1;
        }

        /// <summary>
        /// Calculates the factorial of a positive integer <paramref name="n"/> denoted by n!.
        /// </summary>
        /// <param name="n">The positive integer to calculate a factorial number by.</param>
        /// <returns>The factorial number calculated from <paramref name="n"/>, or <see cref="double.PositiveInfinity"/> if <paramref name="n"/> is to high a value.</returns>
        public static double Factorial(double n)
        {
            if (n < 0) { throw new ArgumentException("n must have a value equal or higher than 0.", nameof(n)); }
            double total = 1;
            for (double i = 2; i <= n; ++i)
            {
                total *= i;
            }
            return total;
        }

        /// <summary>
        /// Gets the highest <see cref="int"/> value of the specified <see cref="int"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="int"/> values to parse for the highest value.</param>
        /// <returns>The highest <see cref="int"/> value of the specified <see cref="int"/> values.</returns>
        public static int GetHighestValue(params int[] values)
        {
            return values.Max();
        }

        /// <summary>
        /// Gets the highest <see cref="long"/> value of the specified <see cref="long"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="long"/> values to parse for the highest value.</param>
        /// <returns>The highest <see cref="long"/> value of the specified <see cref="long"/> values.</returns>
        public static long GetHighestValue(params long[] values)
        {
            return values.Max();
        }

        /// <summary>
        /// Gets the highest <see cref="double"/> value of the specified <see cref="double"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="double"/> values to parse for the highest value.</param>
        /// <returns>The highest <see cref="double"/> value of the specified <see cref="double"/> values.</returns>
        public static double GetHighestValue(params double[] values)
        {
            return values.Max();
        }

        /// <summary>
        /// Gets the highest <see cref="decimal"/> value of the specified <see cref="decimal"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="decimal"/> values to parse for the highest value.</param>
        /// <returns>The highest <see cref="decimal"/> value of the specified <see cref="decimal"/> values.</returns>
        public static decimal GetHighestValue(params decimal[] values)
        {
            return values.Max();
        }

        /// <summary>
        /// Gets the lowest <see cref="int"/> value of the specified <see cref="int"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="int"/> values to parse for the lowest value.</param>
        /// <returns>The lowest <see cref="int"/> value of the specified <see cref="int"/> values.</returns>
        public static int GetLowestValue(params int[] values)
        {
            return values.Min();
        }

        /// <summary>
        /// Gets the lowest <see cref="long"/> value of the specified <see cref="long"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="long"/> values to parse for the lowest value.</param>
        /// <returns>The lowest <see cref="long"/> value of the specified <see cref="long"/> values.</returns>
        public static long GetLowestValue(params long[] values)
        {
            return values.Min();
        }

        /// <summary>
        /// Gets the lowest <see cref="double"/> value of the specified <see cref="double"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="double"/> values to parse for the lowest value.</param>
        /// <returns>The lowest <see cref="double"/> value of the specified <see cref="double"/> values.</returns>
        public static double GetLowestValue(params double[] values)
        {
            return values.Min();
        }

        /// <summary>
        /// Gets the lowest <see cref="decimal"/> value of the specified <see cref="decimal"/> values.
        /// </summary>
        /// <param name="values">A variable number of <see cref="decimal"/> values to parse for the lowest value.</param>
        /// <returns>The lowest <see cref="decimal"/> value of the specified <see cref="decimal"/> values.</returns>
        public static decimal GetLowestValue(params decimal[] values)
        {
            return values.Min();
        }

        /// <summary>
        /// Generates a nonnegative random number.
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>.</returns>
        public static int GetRandomNumber()
        {
            return GetRandomNumber(int.MaxValue);
        }

        /// <summary>
        /// Generates a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
        /// <returns>A 32-bit signed integer greater than or equal to zero, and less than maxValue; that is, the range of return values ordinarily includes zero but not maxValue. However, if maxValue equals zero, maxValue is returned.</returns>
        public static int GetRandomNumber(int maxValue)
        {
            return GetRandomNumber(0, maxValue);
        }

        /// <summary>
        /// Generates a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned. </param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return GetRandomNumber(minValue, maxValue, RandomSeverity.Simple);
        }

        /// <summary>
        /// Generates a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned. </param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="severity">The severity of the random number being generated.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        public static int GetRandomNumber(int minValue, int maxValue, RandomSeverity severity)
        {
            Validator.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
            int seed;
            var localRandomizer = _simpleRandomizerLocal;
            switch (severity)
            {
                case RandomSeverity.Simple:
                    if (localRandomizer == null)
                    {
                        lock (SimpleRandomizerGlobal) { seed = SimpleRandomizerGlobal.Next(); }
                        _simpleRandomizerLocal = localRandomizer = new Random(seed);
                    }
                    break;
                case RandomSeverity.Strong:
                    if (localRandomizer == null)
                    {
                        var randomNumbers = new byte[4];
                        StrongRandomizerGlobal.GetBytes(randomNumbers);
                        seed = BitConverter.ToInt32(randomNumbers, 0);
                        _simpleRandomizerLocal = localRandomizer = new Random(seed);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity));
            }
            return localRandomizer.Next(minValue, maxValue);
        }
    }
}