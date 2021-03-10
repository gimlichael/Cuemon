using System;

namespace Cuemon
{
    /// <summary>
    /// Represent a table of both binary and metric prefixes for a <see cref="ByteUnit"/>. This class cannot be inherited from.
    /// </summary>
    /// <seealso cref="StorageCapacity" />
    public sealed class ByteStorageCapacity : StorageCapacity
    {
        /// <summary>
        /// Creates a new instance of <see cref="ByteStorageCapacity"/> initialized with <paramref name="bits"/>.
        /// </summary>
        /// <param name="bits">The <see cref="double"/> to convert.</param>
        /// <param name="setup">The <see cref="StorageCapacityOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitStorageCapacity"/> that is initialized with <paramref name="bits"/> / <see cref="ByteUnit.BitsPerByte"/>.</returns>
        public static ByteStorageCapacity FromBits(double bits, Action<StorageCapacityOptions> setup = null)
        {
            if (bits < ByteUnit.BitsPerByte) { bits = ByteUnit.BitsPerByte; }
            return FromBytes(bits / ByteUnit.BitsPerByte, setup);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ByteStorageCapacity"/> initialized with <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="double"/> to set the amount of byte for this table.</param>
        /// <param name="setup">The <see cref="StorageCapacityOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitStorageCapacity"/> that is equivalent to <paramref name="bytes"/>.</returns>
        public static ByteStorageCapacity FromBytes(double bytes, Action<StorageCapacityOptions> setup = null)
        {
            Validator.ThrowIfLowerThan(bytes, 0, nameof(bytes));
            return new ByteStorageCapacity(new ByteUnit(bytes, PrefixMultiple.None, Patterns.ConfigureExchange<StorageCapacityOptions, UnitFormatOptions>(setup)), setup);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteStorageCapacity"/> class.
        /// </summary>
        /// <param name="unit">The <see cref="ByteUnit"/> to convert.</param>
        /// <param name="setup">The <see cref="StorageCapacityOptions"/> which may be configured.</param>
        ByteStorageCapacity(ByteUnit unit, Action<StorageCapacityOptions> setup = null) : base(unit, setup)
        {
        }
    }
}