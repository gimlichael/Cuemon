namespace Cuemon.Integrity
{
    /// <summary>
    /// Different models of the CRC algorithm family.
    /// </summary>
    public enum Crc64Model
    {
        /// <summary>
        /// CRC-64; also known as CRC-64/ECMA-182.
        /// </summary>
        Default,
        /// <summary>
        /// CRC-64/GO-ISO.
        /// </summary>
        GoIso,
        /// <summary>
        /// CRC-64/WE.
        /// </summary>
        We,
        /// <summary>
        /// CRC-64/XZ; also, mistakenly, known as: CRC-64/GO-ECMA.
        /// </summary>
        Xz
    }
}