using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Cuemon.Collections.Generic;
using Cuemon.Integrity;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods for generating different types of values or sequences of values.
    /// </summary>
    /// <seealso cref="Eradicate"/>
    public static class Generate
    {
        private static readonly Hash Fnv1A = HashFactory.CreateFnv64(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);

        private static readonly ThreadLocal<Random> LocalRandomizer = new ThreadLocal<Random>(() =>
        {
            var rnd = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rnd);
            var seed = BitConverter.ToInt32(rnd, 0);
            return new Random(seed);
        });

        /// <summary>
        /// Generates a sequence of <typeparamref name="T"/> within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the elements to return.</typeparam>
        /// <param name="count">The number of <typeparamref name="T"/> to generate.</param>
        /// <param name="generator">The function delegate that will resolve the object of <typeparamref name="T"/>; the parameter passed to the delegate represents the index (zero-based) of the element to return.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a range of <typeparamref name="T"/> elements.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        public static IEnumerable<T> RangeOf<T>(int count, Func<int, T> generator)
        {
            Validator.ThrowIfLowerThan(count, 0, nameof(count));
            for (var i = 0; i < count; i++) { yield return generator(i); }
        }

        /// <summary>
        /// Generates a random integer that is within a specified range.
        /// </summary>
        /// <param name="minimumInclusive">The inclusive lower bound of the random number returned.</param>
        /// <param name="maximumExclusive">The exclusive upper bound of the random number returned. <paramref name="maximumExclusive"/> must be greater than or equal to <paramref name="minimumInclusive"/>.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minimumInclusive"/> and less than <paramref name="maximumExclusive"/>; that is, the range of return values includes <paramref name="minimumInclusive"/> but not <paramref name="maximumExclusive"/>.
        /// If <paramref name="minimumInclusive"/> equals <paramref name="maximumExclusive"/>, <paramref name="minimumInclusive"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minimumInclusive" /> is greater than <paramref name="maximumExclusive"/>.
        /// </exception>
        public static int RandomNumber(int maximumExclusive = int.MaxValue, int minimumInclusive = 0)
        {
            Validator.ThrowIfGreaterThan(minimumInclusive, maximumExclusive, nameof(minimumInclusive));
            return LocalRandomizer.Value.Next(minimumInclusive, maximumExclusive);
        }

        /// <summary>
        /// Generates a string from the specified Unicode character repeated until the specified length.
        /// </summary>
        /// <param name="c">A Unicode character.</param>
        /// <param name="count">The number of times <paramref name="c"/> occurs.</param>
        /// <returns>A <see cref="string"/> filled with the specified <paramref name="c"/> until the specified <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than zero.
        /// </exception>
        public static string FixedString(char c, int count)
        {
            return new string(c, count);
        }

        /// <summary>
        /// Generates a random string with the specified length using values of <see cref="Alphanumeric.LettersAndNumbers"/>.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <returns>A random string from the values of <see cref="Alphanumeric.LettersAndNumbers"/>.</returns>
        public static string RandomString(int length)
        {
            return RandomString(length, Alphanumeric.LettersAndNumbers);
        }

        /// <summary>
        /// Generates a random string with the specified length from the provided values.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <param name="values">The values to use in the randomization process.</param>
        /// <returns>A random string from the values provided.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> cannot be null.
        /// </exception>
        public static string RandomString(int length, params string[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            var result = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var index = RandomNumber(values.Length);
                var indexLength = values[index].Length;
                result.Append(values[index][RandomNumber(indexLength)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Computes a suitable hash code from the variable number of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A variable number of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static int HashCode32(params IConvertible[] convertibles)
        {
            return HashCode32(Arguments.ToEnumerableOf(convertibles));
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static int HashCode32(IEnumerable<IConvertible> convertibles)
        {
            return Fnv1A.ComputeHash(convertibles).To(bytes => BitConverter.ToInt32(bytes, 0));
        }

        /// <summary>
        /// Computes a suitable hash code from the variable number of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A variable number of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static long HashCode64(params IConvertible[] convertibles)
        {
            return HashCode64(Arguments.ToEnumerableOf(convertibles));
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static long HashCode64(IEnumerable<IConvertible> convertibles)
        {
            return Fnv1A.ComputeHash(convertibles).To(bytes => BitConverter.ToInt64(bytes, 0));
        }
    }
}