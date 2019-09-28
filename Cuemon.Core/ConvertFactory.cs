using System;
using System.Linq;
using System.Reflection;
using Cuemon.ComponentModel;
using Cuemon.ComponentModel.Codecs;
using Cuemon.ComponentModel.Converters;
using Cuemon.ComponentModel.Decoders;
using Cuemon.ComponentModel.Encoders;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides access to factory methods for instantiating <see cref="IConverter"/>, <see cref="IEncoder"/>, <see cref="IDecoder"/>, <see cref="ICodec"/> and <see cref="IParser"/> implementations.
    /// </summary>
    public static class ConvertFactory
    {
        public static TConverter UseConverter<TConverter>() where TConverter : class, IConverter, new()
        {
            return Activator.CreateInstance<TConverter>();
        }

        /// <summary>
        /// Uses this instance.
        /// </summary>
        /// <typeparam name="TCodec">The type of the t encoder.</typeparam>
        /// <returns>TEncoder.</returns>
        /// <seealso cref="AsciiStringEncoder" />
        /// <seealso cref="DeflateStreamCodec"/>
        /// <seealso cref="GZipStreamCodec"/>
        /// <seealso cref="HexadecimalCodec" />
        /// <seealso cref="StringToByteArrayCodec" />
        /// <seealso cref="UrlProtocolRelativeCodec"/>
        public static TCodec UseCodec<TCodec>() where TCodec : class, ICodec, new()
        {
            return Activator.CreateInstance<TCodec>();
        }

        /// <summary>
        /// Uses the encoder.
        /// </summary>
        /// <typeparam name="TEncoder">The type of the t encoder.</typeparam>
        /// <returns>TEncoder.</returns>
        /// <seealso cref="StringEncoder"/>
        public static TEncoder UseEncoder<TEncoder>() where TEncoder : class, IEncoder, new()
        {
            return Activator.CreateInstance<TEncoder>();
        }

        public static TDecoder UseDecoder<TDecoder>() where TDecoder : class, IDecoder, new()
        {
            return Activator.CreateInstance<TDecoder>();
        }
    }
}