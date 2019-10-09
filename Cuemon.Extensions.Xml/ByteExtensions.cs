using System;
using System.Xml;
using Cuemon.ComponentModel;
using Cuemon.ComponentModel.Codecs;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> struct.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts the given <paramref name="value"/> to an <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:byte[]"/> to extend.</param>
        /// <param name="setup">The <see cref="XmlReaderSettings"/> which may be configured.</param>
        /// <returns>An <see cref="XmlReader"/> representation of <paramref name="value"/>.</returns>
        public static XmlReader ToXmlReader(this byte[] value, Action<XmlReaderSettings> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return ConvertFactory.UseCodec<StreamToByteArrayCodec>().Decode(value).ToXmlReader(setup: setup);
        }
    }
}