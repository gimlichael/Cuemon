using System;
using System.Collections.Generic;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Represents the base class from which all implementations of the CRC (Cyclic Redundancy Check) checksum algorithm must derive.
    /// </summary>
    /// <remarks>Help and inspiration was gathered @ http://www.ross.net/crc/download/crc_v3.txt</remarks>
    public abstract class CyclicRedundancyCheck : Hash<CyclicRedundancyCheckOptions>
    {
        private readonly Lazy<List<ulong>> _lazyLookup;
        private readonly ulong _initialValue;
        private readonly ulong _finalXor;


        /// <summary>
        /// Initializes a new instance of the <see cref="CyclicRedundancyCheck"/> class.
        /// </summary>
        /// <param name="polynomial">This is a binary value that should be specified as a hexadecimal number.</param>
        /// <param name="initialValue">This parameter specifies the initial value of the register when the algorithm starts.</param>
        /// <param name="finalXor">This is an W-bit value that should be specified as a hexadecimal number.</param>
        /// <param name="setup">The <see cref="CyclicRedundancyCheckOptions"/> which need to be configured.</param>
        protected CyclicRedundancyCheck(ulong polynomial, ulong initialValue, ulong finalXor, Action<CyclicRedundancyCheckOptions> setup) : base(setup)
        {
            _lazyLookup = new Lazy<List<ulong>>(() => PolynomialTableInitializerCore(polynomial));
            _initialValue = initialValue;
            _finalXor = finalXor;
        }

        private List<ulong> PolynomialTableInitializerCore(ulong polynomial)
        {
            var table = new ulong[256];
            var index = byte.MinValue;
            for (;;)
            {
                var checksum = PolynomialIndexInitializer(index);
                for (byte b = 0; b < 8; b++)
                {
                    PolynomialSlotCalculator(ref checksum, polynomial);
                }
                table[index] = checksum;
                if (index == byte.MaxValue) { break; }
                index++;
            }
            return new List<ulong>(table);
        }

        /// <summary>
        /// Gets the lookup table containing the pre-computed polynomial values.
        /// </summary>
        /// <value>The lookup table containing the pre-computed polynomial values.</value>
        protected ulong[] LookupTable => _lazyLookup.Value.ToArray();

        /// <summary>
        /// Returns the initial value for the specified <paramref name="index"/> of the polynomial <see cref="LookupTable"/>.
        /// </summary>
        /// <param name="index">The index of the array of polynomial values ranging from 0 to 255.</param>
        /// <returns>The initial value for the specified <paramref name="index"/>.</returns>
        protected abstract ulong PolynomialIndexInitializer(byte index);

        /// <summary>
        /// Computes the polynomial <paramref name="checksum"/> value in steps of 8 (0-7) x 256 (0-255) giving a total of 2048 times.
        /// </summary>
        /// <param name="checksum">The checksum that is iterated 8 times (0-7) in 256 steps (0-255).</param>
        /// <param name="polynomial">The polynomial value.</param>
        protected abstract void PolynomialSlotCalculator(ref ulong checksum, ulong polynomial);

        /// <summary>
        /// Gets the CRC initial value of the register.
        /// </summary>
        /// <value>The CRC initial value of the register.</value>
        public ulong InitialValue => _initialValue;

        /// <summary>
        /// Gets the CRC final value that is XORed to the final register value.
        /// </summary>
        /// <value>The CRC final value that is XORed to the final register value.</value>
        public ulong FinalXor => _finalXor;
    }
}