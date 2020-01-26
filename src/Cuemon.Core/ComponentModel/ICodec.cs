
namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a way to handle both encoding and decoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IDecoder" />
    /// </summary>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IDecoder" />
    /// <remarks>Codec is a portmanteau of encoding (or coding) and decoding.</remarks>
    public interface ICodec : IEncoder, IDecoder
    {
    }
}