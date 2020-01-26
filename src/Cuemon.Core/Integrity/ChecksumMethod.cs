namespace Cuemon.Integrity
{
    /// <summary>
    /// Specifies ways for a checksum to be computed.
    /// </summary>
    public enum ChecksumMethod
    {
        /// <summary>
        /// Indicates default behavior which is leaving the checksum unaltered.
        /// </summary>
        Default,
        /// <summary>
        /// Indicates that a checksum is combined from all given input and hence always will be available.
        /// </summary>
        Combined,
        /// <summary>
        /// Indicates that a checksum is generated from date-time inputs.
        /// </summary>
        Timestamp
    }
}