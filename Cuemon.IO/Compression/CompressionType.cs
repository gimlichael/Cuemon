namespace Cuemon.IO.Compression
{
    /// <summary>
    /// Specifies the algorithm used for compression.
    /// </summary>
    public enum CompressionType
    {
        /// <summary>
        /// A fast and efficient compression using a combination of the LZ77 algorithm and Huffman coding.
        /// </summary>
        Deflate,
        /// <summary>
        /// A slower but otherwise identical compression to the <see cref="CompressionType.Deflate"/> with cyclic redundancy check value for data corruption detection.
        /// </summary>
        GZip
    }
}