namespace Cuemon
{
    /// <summary>
    /// Specifies the two standards for binary multiples and decimal multiples.
    /// </summary>
    public enum UnitPrefix
    {
        /// <summary>
        /// Defines the IEEE 1541 standard for binary prefix that refers strictly to powers of 2 (eg. one kibibit represents 1024 bits and not 1000 bits).
        /// </summary>
        Binary,
        /// <summary>
        /// Defines the International System of Units (SI) standard for metric prefixes that refers strictly to powers of 10 (eg. one kilobit represents 1000 bits and not 1024 bits).
        /// </summary>
        Decimal
    }
}