using System;
using System.Numerics;

namespace Cuemon.Security
{
    /// <summary>
    /// Represents the base class from which all implementations of the Fowler–Noll–Vo non-cryptographic hashing algorithm must derive.
    /// </summary>
    public abstract class FowlerNollVoHash : Hash<FowlerNollVoOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FowlerNollVoHash"/> class.
        /// </summary>
        /// <param name="bits">The size in bits.</param>
        /// <param name="prime">The prime number of the algorithm.</param>
        /// <param name="offsetBasis">The initial value of the hash.</param>
        /// <param name="setup">The <see cref="FowlerNollVoOptions"/> which need to be configured.</param>
        protected FowlerNollVoHash(short bits, BigInteger prime, BigInteger offsetBasis, Action<FowlerNollVoOptions> setup) : base(setup)
        {
            Prime = prime;
            OffsetBasis = offsetBasis;
            Bits = BigInteger.Pow(2, bits);
        }

        /// <summary>
        /// Gets the prime number of the algorithm.
        /// </summary>
        /// <value>The prime number of the algorithm.</value>
        public BigInteger Prime { get; }

        /// <summary>
        /// Gets the offset basis used as the initial value of the hash.
        /// </summary>
        /// <value>The offset basis used as the initial value of the hash.</value>
        public BigInteger OffsetBasis { get; }

        /// <summary>
        /// Gets the size of the implementation in bits.
        /// </summary>
        /// <value>The size of the implementation in bits.</value>
        public BigInteger Bits { get; }

        /// <summary>
        /// Computes the hash value for the specified <see cref="T:byte[]" />.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]" /> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult" /> containing the computed hash code of the specified <paramref name="input" />.</returns>
        public override HashResult ComputeHash(byte[] input)
        {
            var hash = OffsetBasis;
            switch (Options.Algorithm)
            {
                case FowlerNollVoAlgorithm.Fnv1a:
                    foreach (var b in input)
                    {
                        hash ^= b;
                        hash *= Prime;
                        hash %= Bits;
                    }
                    break;
                default:
                    foreach (var b in input)
                    {
                        hash *= Prime;
                        hash ^= b;
                        hash %= Bits;
                    }
                    break;
            }
            var result = hash.ToByteArray();
            if (Condition.IsOdd(result.Length)) { Array.Resize(ref result, result.Length - 1); }
            result = Convertible.ReverseEndianness(result, o => o.ByteOrder = Options.ByteOrder);
            return new HashResult(result);
        }
    }
}