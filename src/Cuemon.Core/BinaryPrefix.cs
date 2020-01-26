using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Defines a binary unit prefix for multiples of measurement for data that refers strictly to powers of 2.
    /// Implements the <see cref="IPrefixMultiple" />
    /// </summary>
    /// <seealso cref="IPrefixMultiple" />
    /// <seealso cref="UnitPrefix"/>
    public struct BinaryPrefix : IPrefixMultiple
    {
        private static readonly Lazy<IEnumerable<BinaryPrefix>> LazyPrefixes = new Lazy<IEnumerable<BinaryPrefix>>(() =>
        {
            var list = new List<BinaryPrefix>()
            {
                Exbi,
                Gibi,
                Kibi,
                Mebi,
                Pebi,
                Tebi,
                Yobi,
                Zebi
            };
            return list;
        });

        /// <summary>
        /// Gets the binary-multiple prefix kibi (symbol 'Ki'), 2^10 = 1024.
        /// </summary>
        /// <value>The binary-multiple prefix kibi (symbol 'Ki').</value>
        public static BinaryPrefix Kibi => new BinaryPrefix("kibi", "Ki", 10);

        /// <summary>
        /// Gets the binary-multiple prefix mebi (symbol 'Mi'), 2^20 = 1048576.
        /// </summary>
        /// <value>The binary-multiple prefix mebi (symbol 'Mi').</value>
        public static BinaryPrefix Mebi => new BinaryPrefix("mebi", "Mi", 20);

        /// <summary>
        /// Gets the binary-multiple prefix gibi (symbol 'Gi'), 2^30 = 1073741824.
        /// </summary>
        /// <value>The binary-multiple prefix gibi (symbol 'Gi').</value>
        public static BinaryPrefix Gibi => new BinaryPrefix("gibi", "Gi", 30);

        /// <summary>
        /// Gets the binary-multiple prefix tebi (symbol 'Ti'), 2^40 = 1099511627776.
        /// </summary>
        /// <value>The binary-multiple prefix tebi (symbol 'Ti').</value>
        public static BinaryPrefix Tebi => new BinaryPrefix("tebi", "Ti", 40);

        /// <summary>
        /// Gets the binary-multiple prefix pebi (symbol 'Pi'), 2^50 = 1125899906842624.
        /// </summary>
        /// <value>The binary-multiple prefix pebi (symbol 'Pi').</value>
        public static BinaryPrefix Pebi => new BinaryPrefix("pebi", "Pi", 50);

        /// <summary>
        /// Gets the binary-multiple prefix exbi (symbol 'Ei'), 2^60 = 1152921504606846976.
        /// </summary>
        /// <value>The binary-multiple prefix exbi (symbol 'Ei').</value>
        public static BinaryPrefix Exbi => new BinaryPrefix("exbi", "Ei", 60);

        /// <summary>
        /// Gets the binary-multiple prefix zebi (symbol 'Zi'), 2^70 = 1180591620717411303424.
        /// </summary>
        /// <value>The binary-multiple prefix zebi (symbol 'Zi').</value>
        public static BinaryPrefix Zebi => new BinaryPrefix("zebi", "Zi", 70);

        /// <summary>
        /// Gets the binary-multiple prefix yobi (symbol 'Yi'), 2^80 = 1208925819614629174706176.
        /// </summary>
        /// <value>The binary-multiple prefix yobi (symbol 'Yi').</value>
        public static BinaryPrefix Yobi => new BinaryPrefix("yobi", "Yi", 80);

        /// <summary>
        /// Gets the complete sequence of multiples and submultiples binary prefixes as specified by Institute of Electrical and Electronics Engineers (IEEE).
        /// </summary>
        /// <value>The complete sequence of multiples and submultiples binary prefixes as specified by Institute of Electrical and Electronics Engineers (IEEE).</value>
        public static IEnumerable<BinaryPrefix> BinaryPrefixes => LazyPrefixes.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryPrefix"/> struct.
        /// </summary>
        /// <param name="name">The name of the binary prefix.</param>
        /// <param name="symbol">The symbol of the the binary prefix.</param>
        /// <param name="exponent">The number that specifies a power.</param>
        public BinaryPrefix(string name, string symbol, int exponent)
        {
            Name = name;
            Symbol = symbol;
            Multiplier = Math.Pow(2, exponent);
        }

        /// <summary>
        /// Gets the name of the binary prefix.
        /// </summary>
        /// <value>The name of the binary prefix.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the symbol of the binary prefix.
        /// </summary>
        /// <value>The symbol of the binary prefix.</value>
        public string Symbol { get; }

        /// <summary>
        /// Gets the binary prefix multiplier.
        /// </summary>
        /// <value>The binary prefix multiplier.</value>
        public double Multiplier { get; }

        /// <summary>
        /// Converts the unit base <paramref name="value"/> to a binary prefix value.
        /// </summary>
        /// <param name="value">The value of the base unit.</param>
        /// <returns>A <see cref="double"/> that represents a binary prefix value.</returns>
        public double ToPrefixValue(double value)
        {
            return value / Multiplier;
        }

        /// <summary>
        /// Converts the binary <paramref name="prefixValue"/> back to a unit base value.
        /// </summary>
        /// <param name="prefixValue">The value of the binary prefix.</param>
        /// <returns>A <see cref="double"/> that represents a unit base value.</returns>
        public double ToBaseValue(double prefixValue)
        {
            return prefixValue * Multiplier;
        }

        internal IPrefixUnit ApplyPrefix(IPrefixUnit unit, Action<UnitFormatOptions> setup = null)
        {
            Validator.ThrowIfNull(unit, nameof(unit));
            try
            {
                return (IPrefixUnit)Activator.CreateInstance(unit.GetType(), unit.UnitValue, this, setup);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return FormattableString.Invariant($"{Name} ({Symbol}) 2^{Math.Log(Multiplier, 2)}");
        }
    }
}