namespace Cuemon
{
    /// <summary>
    /// Specifies ways that a string must be represented in terms of naming style.
    /// </summary>
    public enum NamingStyle
    {
        /// <summary>
        /// Indicates the compound naming style string representation (eg. 1 Gigabyte / 0.93 Gibibyte).
        /// </summary>
        Compound,
        /// <summary>
        /// Indicates the symbol naming style string representation (eg. 1 GB / 0.93 GiB).
        /// </summary>
        Symbol
    }
}