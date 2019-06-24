namespace Cuemon.Integrity
{
    /// <summary>
    /// Different models of the CRC algorithm family.
    /// </summary>
    public enum Crc32Model
    {
        /// <summary>
        /// CRC-32; also known as CRC-32/ISO-HDLC, CRC-32/ADCCP, CRC-32/V-42, CRC-32/XZ, PKZIP.
        /// </summary>
        Default,
        /// <summary>
        /// CRC-32/AUTOSAR.
        /// </summary>
        Autosar,
        /// <summary>
        /// CRC-32/BZIP2; also known as CRC-32/AAL5, CRC-32/DECT-B, B-CRC-32.
        /// </summary>
        Bzip2,
        /// <summary>
        /// CRC32-C; also known as CRC-32/ISCSI, CRC-32/BASE91-C, CRC-32/CASTAGNOLI, CRC-32/INTERLAKEN.
        /// </summary>
        C,
        /// <summary>
        /// CRC-32/CD-ROM-EDC.
        /// </summary>
        CdRomEdc,
        /// <summary>
        /// CRC-32D; also known as BASE91-D.
        /// </summary>
        D,
        /// <summary>
        /// CRC-32/MPEG-2.
        /// </summary>
        Mpeg2,
        /// <summary>
        /// CRC-32/POSIX; also known as CRC-32/CKSUM, CKSUM.
        /// </summary>
        Posix,
        /// <summary>
        /// CRC-32Q; also known as CRC-32/AIXM.
        /// </summary>
        Q,
        /// <summary>
        /// CRC-32/JAMCRC.
        /// </summary>
        Jamcrc,
        /// <summary>
        /// CRC-32/XFER.
        /// </summary>
        Xfer
    }
}