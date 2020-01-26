using System;

namespace Cuemon
{
    /// <summary>
    /// Represent a table of both binary and metric prefixes for a <see cref="ByteUnit"/>.
    /// Implements the <see cref="MultipleTable" />
    /// </summary>
    /// <seealso cref="MultipleTable" />
    public class ByteMultipleTable : MultipleTable
    {
        /// <summary>
        /// Creates a new instance of <see cref="ByteMultipleTable"/> initialized with <paramref name="bits"/>.
        /// </summary>
        /// <param name="bits">The <see cref="double"/> to convert.</param>
        /// <param name="setup">The <see cref="MultipleTableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitMultipleTable"/> that is initialized with <paramref name="bits"/> / <see cref="ByteUnit.BitsPerByte"/>.</returns>
        public static ByteMultipleTable FromBits(double bits, Action<MultipleTableOptions> setup = null)
        {
            if (bits < ByteUnit.BitsPerByte) { bits = ByteUnit.BitsPerByte; }
            return FromBytes(bits / ByteUnit.BitsPerByte, setup);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ByteMultipleTable"/> initialized with <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="double"/> to set the amount of byte for this table.</param>
        /// <param name="setup">The <see cref="MultipleTableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitMultipleTable"/> that is equivalent to <paramref name="bytes"/>.</returns>
        public static ByteMultipleTable FromBytes(double bytes, Action<MultipleTableOptions> setup = null)
        {
            Validator.ThrowIfLowerThan(bytes, 0, nameof(bytes));
            return new ByteMultipleTable(new ByteUnit(bytes, setup: Patterns.ConfigureExchange<MultipleTableOptions, UnitFormatOptions>(setup)), setup);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteMultipleTable"/> class.
        /// </summary>
        /// <param name="unit">The <see cref="ByteUnit"/> to convert.</param>
        /// <param name="setup">The <see cref="MultipleTableOptions"/> which may be configured.</param>
        ByteMultipleTable(ByteUnit unit, Action<MultipleTableOptions> setup = null) : base(unit, setup)
        {
        }
    }
}