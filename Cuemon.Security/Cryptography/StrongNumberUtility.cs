using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This utility class is designed to make cryptography strong number operations easier to work with.
    /// </summary>
    public static class StrongNumberUtility
    {
        [ThreadStatic]
        private static Random _simpleRandomizerLocal;
        private static readonly RandomNumberGenerator StrongRandomizerGlobal = RandomNumberGenerator.Create();

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
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="minValue"/> is greater than <paramref name="maxValue"/>
        /// </exception>
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            if (minValue > maxValue) { throw new ArgumentOutOfRangeException(nameof(minValue)); }
            Random localRandomizer = _simpleRandomizerLocal;
            if (localRandomizer == null)
            {
                byte[] randomNumbers = new byte[4];
                StrongRandomizerGlobal.GetBytes(randomNumbers);
                var seed = BitConverter.ToInt32(randomNumbers, 0);
                _simpleRandomizerLocal = localRandomizer = new Random(seed);
            }
            return localRandomizer.Next(minValue, maxValue);
        }
    }
}