using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make XML <see cref="Stream"/> related conversions easier to work with.
    /// </summary>
    public static class XmlStreamConverter
    {
        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from the resolved source encoding to the specified target encoding, preserving any preamble sequences.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from the resolved source encoding to the specified targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream stream, Encoding targetEncoding)
        {
            return ChangeEncoding(stream, targetEncoding, PreambleSequence.Keep);
        }

        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from the resolved encoding of <paramref name="stream"/> to the specified encoding.
        /// If an encoding cannot be resolved from <paramref name="stream"/>, UTF-8 encoding is assumed.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from the resolved source encoding to the specified targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream stream, Encoding targetEncoding, PreambleSequence sequence)
        {
            return ChangeEncoding(stream, XmlUtility.ReadEncoding(stream), o =>
            {
                o.Encoding = targetEncoding;
                o.Preamble = sequence;
            });
        }

        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from one <paramref name="encoding"/> to what is defined in <paramref name="setup"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="encoding">The source encoding format.</param>
        /// <param name="setup">The <see cref="XmlEncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from <paramref name="encoding"/> to what is defined in <paramref name="setup"/>.</returns>
        public static Stream ChangeEncoding(Stream stream, Encoding encoding, Action<XmlEncodingOptions> setup)
        {
            Validator.ThrowIfNull(stream, nameof(stream));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            Validator.ThrowIfNull(setup, nameof(setup));

            var options = Patterns.Configure(setup);
            if (encoding.Equals(options.Encoding)) { return stream; }

            long startingPosition = -1;
            if (stream.CanSeek)
            {
                startingPosition = stream.Position;
                stream.Position = 0;
            }

            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                var document = new XmlDocument();
                document.Load(stream);
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    var declaration = (XmlDeclaration)document.FirstChild;
                    declaration.Encoding = options.Encoding.WebName;
                }
                var settings = new XmlWriterSettings();
                settings.Encoding = options.Encoding;
                settings.Indent = true;
                settings.OmitXmlDeclaration = options.OmitXmlDeclaration;
                using (var writer = XmlWriter.Create(ms, settings))
                {
                    document.Save(writer);
                    writer.Flush();
                }

                if (stream.CanSeek) { stream.Seek(startingPosition, SeekOrigin.Begin); } // reset to original position
                ms.Position = 0;

                switch (options.Preamble)
                {
                    case PreambleSequence.Keep:
                        return ms;
                    case PreambleSequence.Remove:
                        var valueInBytes = ms.ToArray();
                        using (ms)
                        {
                            valueInBytes = ByteArrayUtility.RemovePreamble(valueInBytes, options.Encoding);
                        }
                        return new MemoryStream(valueInBytes);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options.Preamble));
                }
            });
        }
    }
}