using System;

namespace Cuemon
{
    /// <summary>
    /// Represent a table of both binary and metric prefixes for a <see cref="BitUnit"/>.
    /// Implements the <see cref="MultipleTable" />
    /// </summary>
    /// <seealso cref="MultipleTable" />
    public class BitMultipleTable : MultipleTable
    {
        /// <summary>
        /// Creates a new instance of <see cref="BitMultipleTable"/> initialized with <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="double"/> to convert.</param>
        /// <param name="setup">The <see cref="MultipleTableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitMultipleTable"/> that is initialized with <paramref name="bytes"/> * <see cref="ByteUnit.BitsPerByte"/> (ceiling).</returns>
        public static BitMultipleTable FromBytes(double bytes, Action<MultipleTableOptions> setup = null)
        {
            Validator.ThrowIfLowerThan(bytes, 0, nameof(bytes));
            return FromBits(Math.Ceiling(bytes * ByteUnit.BitsPerByte), setup);
        }

        /// <summary>
        /// Creates a new instance of <see cref="BitMultipleTable"/> initialized with <paramref name="bits"/>.
        /// </summary>
        /// <param name="bits">The <see cref="double"/> to set the amount of bit for this table.</param>
        /// <param name="setup">The <see cref="MultipleTableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="BitMultipleTable"/> that is equivalent to <paramref name="bits"/>.</returns>
        public static BitMultipleTable FromBits(double bits, Action<MultipleTableOptions> setup = null)
        {
            Validator.ThrowIfLowerThan(bits, 0, nameof(bits));
            return new BitMultipleTable(new BitUnit(bits, setup: Patterns.ConfigureExchange<MultipleTableOptions, UnitFormatOptions>(setup)), setup);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitMultipleTable"/> class.
        /// </summary>
        /// <param name="unit">The <see cref="BitUnit"/> to convert.</param>
        /// <param name="setup">The <see cref="MultipleTableOptions"/> which may be configured.</param>
        BitMultipleTable(BitUnit unit, Action<MultipleTableOptions> setup = null) : base(unit, setup)
        {
        }
    }
}