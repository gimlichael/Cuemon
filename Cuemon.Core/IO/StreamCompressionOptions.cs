using System.IO.Compression;

namespace Cuemon.IO
{
    public class StreamCompressionOptions : StreamCopyOptions
    {
        public StreamCompressionOptions()
        {
            Level = CompressionLevel.Optimal;
        }

        public CompressionLevel Level { get; set; }
    }
}