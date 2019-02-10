using System;
using System.Globalization;
using Cuemon.Collections.Generic;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Computes the CRC32 hash value for the input data using the implementation provided by the cyclic redundancy check class (CRC). This class cannot be inherited.
    /// </summary>
    public sealed class CyclicRedundancyCheck32 : CyclicRedundancyCheck
    {
        #region Contructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CyclicRedundancyCheck32"/> class.
        /// </summary>
        public CyclicRedundancyCheck32()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclicRedundancyCheck" /> class.
        /// </summary>
        /// <param name="representation">The CRC generator polynomial representation.</param>
        public CyclicRedundancyCheck32(PolynomialRepresentation representation) : base(representation)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the implementation of the polynomial representation details.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The <see cref="CyclicRedundancyCheck.Representation"/> property has an invalid value.
        /// </exception>
        protected override void InitializePolynomial()
        {
            switch (Representation)
            {
                case PolynomialRepresentation.Normal:
                    Polynomial = 0x04C11DB7;
                    break;
                case PolynomialRepresentation.Reversed:
                    Polynomial = 0xEDB88320;
                    break;
                case PolynomialRepresentation.ReversedReciprocal:
                    Polynomial = 0x82608EDB;
                    break;
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The specified CRC representation is invalid. Expected value must be one of the following: Normal, Reversed or ReversedReciprocal. Actually value was: {0}.", Representation));
            }
        }

        /// <summary>
        /// Initializes the implementation details of a <see cref="CyclicRedundancyCheck"/> related polynomial lookup table.
        /// </summary>
        /// <param name="currentBit">The current bit ranging from 0 to 7.</param>
        /// <param name="currentTableIndex">The current index of the associated polynomial <see cref="CyclicRedundancyCheck.LookupTable"/> ranging from 0 to 255.</param>
        /// <remarks>This method is - on first run - invoked 8 times per entry in the associated polynomial <see cref="CyclicRedundancyCheck.LookupTable"/>, given a total of 2048 times.</remarks>
        protected override void InitializePolynomialLookupTable(byte currentBit, ushort currentTableIndex)
        {
            bool hasIndex = ListUtility.HasIndex(currentTableIndex, LookupTable);
            if (!hasIndex) { LookupTable.Add(currentTableIndex); }
            long value = LookupTable[currentTableIndex];
            if ((value & 1) == 1)
            {
                value = (value >> 1) ^ Polynomial;
            }
            else
            {
                value >>= 1;
            }
            LookupTable[currentTableIndex] = value;
        }

        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            unchecked
            {
                for (int i = ibStart; i < cbSize; i++)
                {
                    HashCoreResult = ((HashCoreResult) >> 8) ^ LookupTable[(int)((array[i]) ^ ((HashCoreResult) & 0x000000FF))];
                }
            }
            HashCoreResult ^= DefaultSeed;
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            return new[] { (byte)((HashCoreResult >> 24) & 0xFF), 
                (byte)((HashCoreResult >> 16) & 0xFF), 
                (byte)((HashCoreResult >> 8) & 0xFF), 
                (byte)(HashCoreResult & 0xFF) };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the size, in bits, of the computed hash code.
        /// </summary>
        /// <value>The size of the hash.</value>
        /// <returns>The size, in bits, of the computed hash code.</returns>
        public override int HashSize
        {
            get { return 32; }
        }

        /// <summary>
        /// Gets the CRC default seed value.
        /// </summary>
        /// <value>The CRC default seed value.</value>
        public override long DefaultSeed
        {
            get { return 0xFFFFFFFF; }
        }
        #endregion
    }
}
