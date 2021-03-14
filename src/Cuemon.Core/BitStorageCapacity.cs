using System;

namespace Cuemon
{
    /// <summary>
    /// Represent a table of both binary and metric prefixes for a <see cref="BitUnit"/>. This class cannot be inherited from.
    /// </summary>
    /// <seealso cref="StorageCapacity" />
    public sealed class BitStorageCapacity : StorageCapacity
    {
        /// <summary>
        /// Creates a new instance of <see cref="BitStorageCapacity"/> initialized with <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="double"/> to convert.</param>
        /// <param name="setup">The <see cref="StorageCapacityOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitStorageCapacity"/> that is initialized with <paramref name="bytes"/> * <see cref="ByteUnit.BitsPerByte"/> (ceiling).</returns>
        public static BitStorageCapacity FromBytes(double bytes, Action<StorageCapacityOptions> setup = null)
        {
            Validator.ThrowIfLowerThan(bytes, 0, nameof(bytes));
            return FromBits(Math.Ceiling(bytes * ByteUnit.BitsPerByte), setup);
        }

        /// <summary>
        /// Creates a new instance of <see cref="BitStorageCapacity"/> initialized with <paramref name="bits"/>.
        /// </summary>
        /// <param name="bits">The <see cref="double"/> to set the amount of bit for this table.</param>
        /// <param name="setup">The <see cref="StorageCapacityOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitStorageCapacity"/> that is equivalent to <paramref name="bits"/>.</returns>
        public static BitStorageCapacity FromBits(double bits, Action<StorageCapacityOptions> setup = null)
        {
            Validator.ThrowIfLowerThan(bits, 0, nameof(bits));
            return new BitStorageCapacity(new BitUnit(bits, PrefixMultiple.None, Patterns.ConfigureExchange<StorageCapacityOptions, UnitFormatOptions>(setup)), setup);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStorageCapacity"/> class.
        /// </summary>
        /// <param name="unit">The <see cref="BitUnit"/> to convert.</param>
        /// <param name="setup">The <see cref="StorageCapacityOptions"/> which may be configured.</param>
        BitStorageCapacity(BitUnit unit, Action<StorageCapacityOptions> setup = null) : base(unit, setup)
        {
        }
    }
}