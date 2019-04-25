namespace Cuemon.Integrity
{
    /// <summary>
    /// Specifies the validation strength of a cache checksum.
    /// </summary>
    public enum ChecksumStrength
    {
        /// <summary>
        /// Indicates that no checksum was specified.
        /// </summary>
        None,
        /// <summary>
        /// Indicates that a weak, semantic equivalent checksum was specified.
        /// </summary>
        Weak,
        /// <summary>
        /// Indicates that a strong, byte-for-byte checksum was specified.
        /// </summary>
        Strong
    }
}