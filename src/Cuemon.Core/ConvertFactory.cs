using System;
using Cuemon.ComponentModel;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides access to factory methods that are tailored for convert operations following the patterns defined in <see cref="IConverter{TInput,TResult}"/> and <see cref="ITypeConverter{TInput,TOptions}"/>.
    /// </summary>
    public static class ConvertFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="ObjectConverter"/>.
        /// </summary>
        /// <returns>An <see cref="ITypeConverter{TInput,TResult}"/> implementation of <see cref="ObjectConverter"/>.</returns>
        public static ObjectConverter FromObject()
        {
            return new ObjectConverter();
        }

        /// <summary>
        /// Uses this instance.
        /// </summary>
        /// <typeparam name="TCodec">The type of the t encoder.</typeparam>
        /// <returns>TEncoder.</returns>
        /// <seealso cref="AsciiStringEncoder" />
        /// <seealso cref="DeflateStreamCodec"/>
        /// <seealso cref="GZipStreamCodec"/>
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