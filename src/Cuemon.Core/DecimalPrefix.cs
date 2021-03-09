using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Defines a decimal (metric) unit prefix for multiples and submultiples of measurement that refers strictly to powers of 10. This class cannot be inherited.
    /// Implements the <see cref="IPrefixMultiple" />
    /// </summary>
    /// <seealso cref="IPrefixMultiple" />
    public sealed class DecimalPrefix : PrefixMultiple
    {
        private static readonly Lazy<IEnumerable<DecimalPrefix>> LazyPrefixes = new Lazy<IEnumerable<DecimalPrefix>>(() =>
        {
            var list = new List<DecimalPrefix>()
            {
                Atto,
                Centi,
                Deca,
                Deci,
                Exa,
                Femto,
                Giga,
                Hecto,
                Kilo,
                Mega,
                Micro,
                Milli,
                Nano,
                Peta,
                Pico,
                Tera,
                Yocto,
                Yotta,
                Zepto,
                Zetta
            };
            return list;
        });

        /// <summary>
        /// Gets the decimal-submultiple prefix yocto (symbol 'y'), 10^-24 = 0.000000000000000000000001.
        /// </summary>
        /// <value>The decimal-submultiple prefix yocto (symbol 'y').</value>
        public static DecimalPrefix Yocto => new DecimalPrefix("yocto", "y", -24);

        /// <summary>
        /// Gets the decimal-submultiple prefix zepto (symbol 'z'), 10^-21 = 0.000000000000000000001.
        /// </summary>
        /// <value>The decimal-submultiple prefix zepto (symbol 'z').</value>
        public static DecimalPrefix Zepto => new DecimalPrefix("zepto", "z", -21);

        /// <summary>
        /// Gets the decimal-submultiple prefix atto (symbol 'a'), 10^-18 = 0.000000000000000001.
        /// </summary>
        /// <value>The decimal-submultiple prefix atto (symbol 'a').</value>
        public static DecimalPrefix Atto => new DecimalPrefix("atto", "a", -18);

        /// <summary>
        /// Gets the decimal-submultiple prefix femto (symbol 'f'), 10^-15 = 0.000000000000001.
        /// </summary>
        /// <value>The decimal-submultiple prefix femto (symbol 'f').</value>
        public static DecimalPrefix Femto => new DecimalPrefix("femto", "f", -15);

        /// <summary>
        /// Gets the decimal-submultiple prefix pico (symbol 'f'), 10^-12 = 0.000000000001.
        /// </summary>
        /// <value>The decimal-submultiple prefix pico (symbol 'f').</value>
        public static DecimalPrefix Pico => new DecimalPrefix("pico", "p", -12);

        /// <summary>
        /// Gets the decimal-submultiple prefix nano (symbol 'n'), 10^-9 = 0.000000001.
        /// </summary>
        /// <value>The decimal-submultiple prefix nano (symbol 'n').</value>
        public static DecimalPrefix Nano => new DecimalPrefix("nano", "n", -9);

        /// <summary>
        /// Gets the decimal-submultiple prefix micro (symbol 'μ'), 10^-6 = 0.000001.
        /// </summary>
        /// <value>The decimal-submultiple prefix micro (symbol 'μ').</value>
        public static DecimalPrefix Micro => new DecimalPrefix("micro", "μ", -6);

        /// <summary>
        /// Gets the decimal-submultiple prefix milli (symbol 'μ'), 10^-3 = 0.001.
        /// </summary>
        /// <value>The decimal-submultiple prefix milli (symbol 'μ').</value>
        public static DecimalPrefix Milli => new DecimalPrefix("milli", "m", -3);

        /// <summary>
        /// Gets the decimal-submultiple prefix centi (symbol 'c'), 10^-2 = 0.01.
        /// </summary>
        /// <value>The decimal-submultiple prefix centi (symbol 'c').</value>
        public static DecimalPrefix Centi => new DecimalPrefix("centi", "c", -2);

        /// <summary>
        /// Gets the decimal-submultiple prefix deci (symbol 'd'), 10^-1 = 0.1.
        /// </summary>
        /// <value>The decimal-submultiple prefix deci (symbol 'd').</value>
        public static DecimalPrefix Deci => new DecimalPrefix("deci", "d", -1);

        /// <summary>
        /// Gets the decimal-multiple prefix deca (symbol 'da'), 10^1 = 10.
        /// </summary>
        /// <value>The decimal-multiple prefix deca (symbol 'da').</value>
        public static DecimalPrefix Deca => new DecimalPrefix("deca", "da", 1);

        /// <summary>
        /// Gets the decimal-multiple prefix hecto (symbol 'h'), 10^2 = 100.
        /// </summary>
        /// <value>The decimal-multiple prefix hecto (symbol 'h').</value>
        public static DecimalPrefix Hecto => new DecimalPrefix("hecto", "h", 2);

        /// <summary>
        /// Gets the decimal-multiple prefix kilo (symbol 'k'), 10^3 = 1000.
        /// </summary>
        /// <value>The decimal-multiple prefix kilo (symbol 'k').</value>
        public static DecimalPrefix Kilo => new DecimalPrefix("kilo", "k", 3);

        /// <summary>
        /// Gets the decimal-multiple prefix mega (symbol 'M'), 10^6 = 1000000.
        /// </summary>
        /// <value>The decimal-multiple prefix mega (symbol 'M').</value>
        public static DecimalPrefix Mega => new DecimalPrefix("mega", "M", 6);

        /// <summary>
        /// Gets the decimal-multiple prefix giga (symbol 'G'), 10^9 = 1000000000.
        /// </summary>
        /// <value>The decimal-multiple prefix giga (symbol 'G').</value>
        public static DecimalPrefix Giga => new DecimalPrefix("giga", "G", 9);

        /// <summary>
        /// Gets the decimal-multiple prefix tera (symbol 'T'), 10^12 = 1000000000000.
        /// </summary>
        /// <value>The decimal-multiple prefix tera (symbol 'T').</value>
        public static DecimalPrefix Tera => new DecimalPrefix("tera", "T", 12);

        /// <summary>
        /// Gets the decimal-multiple prefix peta (symbol 'P'), 10^15 = 1000000000000000.
        /// </summary>
        /// <value>The decimal-multiple prefix peta (symbol 'P').</value>
        public static DecimalPrefix Peta => new DecimalPrefix("peta", "P", 15);

        /// <summary>
        /// Gets the decimal-multiple prefix exa (symbol 'E'), 10^18 = 1000000000000000000.
        /// </summary>
        /// <value>The decimal-multiple prefix exa (symbol 'E').</value>
        public static DecimalPrefix Exa => new DecimalPrefix("exa", "E", 18);

        /// <summary>
        /// Gets the decimal-multiple prefix zetta (symbol 'Z'), 10^21 = 1000000000000000000000.
        /// </summary>
        /// <value>The decimal-multiple prefix zetta (symbol 'Z').</value>
        public static DecimalPrefix Zetta => new DecimalPrefix("zetta", "Z", 21);

        /// <summary>
        /// Gets the decimal-multiple prefix yotta (symbol 'Y'), 10^24 = 1000000000000000000000000.
        /// </summary>
        /// <value>The decimal-multiple prefix yotta (symbol 'Y').</value>
        public static DecimalPrefix Yotta => new DecimalPrefix("yotta", "Y", 24);

        /// <summary>
        /// Gets the complete sequence of multiples and submultiples metric prefixes as specified by International System of Units (SI).
        /// </summary>
        /// <value>The complete sequence of multiples and submultiples metric prefixes as specified by International System of Units (SI).</value>
        public static IEnumerable<DecimalPrefix> MetricPrefixes => LazyPrefixes.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalPrefix"/> struct.
        /// </summary>
        /// <param name="name">The name of the decimal prefix.</param>
        /// <param name="symbol">The symbol of the the decimal prefix.</param>
        /// <param name="exponent">The number that specifies a power.</param>
        public DecimalPrefix(string name, string symbol, double exponent) : base(name, symbol, 10, exponent)
        {
        }
    }
}