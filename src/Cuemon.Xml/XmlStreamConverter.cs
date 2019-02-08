using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.IO;

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
        /// <param name="source">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from the resolved source encoding to the specified targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream source, Encoding targetEncoding)
        {
            return ChangeEncoding(source, targetEncoding, PreambleSequence.Keep);
        }

        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from the resolved encoding of <paramref name="source"/> to the specified encoding.
        /// If an encoding cannot be resolved from <paramref name="source"/>, UTF-8 encoding is assumed.
        /// </summary>
        /// <param name="source">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from the resolved source encoding to the specified targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream source, Encoding targetEncoding, PreambleSequence sequence)
        {
            return ChangeEncoding(source, XmlEncodingUtility.ReadEncoding(source), targetEncoding, sequence);
        }

        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from the resolved encoding of <paramref name="source"/> to the specified encoding.
        /// If an encoding cannot be resolved from <paramref name="source"/>, UTF-8 encoding is assumed.
        /// </summary>
        /// <param name="source">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from the resolved source encoding to the specified targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream source, Encoding targetEncoding, bool omitXmlDeclaration)
        {
            return ChangeEncoding(source, XmlEncodingUtility.ReadEncoding(source), targetEncoding, PreambleSequence.Keep, omitXmlDeclaration);
        }

        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from one encoding to another.
        /// </summary>
        /// <param name="source">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="sourceEncoding">The source encoding format.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from sourceEncoding to targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream source, Encoding sourceEncoding, Encoding targetEncoding, bool omitXmlDeclaration)
        {
            return ChangeEncoding(source, sourceEncoding, targetEncoding, PreambleSequence.Keep, omitXmlDeclaration);
        }

        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from one encoding to another.
        /// </summary>
        /// <param name="source">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="sourceEncoding">The source encoding format.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from sourceEncoding to targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream source, Encoding sourceEncoding, Encoding targetEncoding, PreambleSequence sequence)
        {
            return ChangeEncoding(source, sourceEncoding, targetEncoding, sequence, false);
        }

        /// <summary>
        /// Converts the entire XML <see cref="Stream"/> object from one encoding to another.
        /// </summary>
        /// <param name="source">The <see cref="Stream"/> to apply the conversion to.</param>
        /// <param name="sourceEncoding">The source encoding format.</param>
        /// <param name="targetEncoding">The target encoding format.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> object containing the results of converting bytes from sourceEncoding to targetEncoding.</returns>
        public static Stream ChangeEncoding(Stream source, Encoding sourceEncoding, Encoding targetEncoding, PreambleSequence sequence, bool omitXmlDeclaration)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (sourceEncoding == null) throw new ArgumentNullException(nameof(sourceEncoding));
            if (targetEncoding == null) throw new ArgumentNullException(nameof(targetEncoding));
            if (sourceEncoding.Equals(targetEncoding)) { return source; }

            long startingPosition = -1;
            if (source.CanSeek)
            {
                startingPosition = source.Position;
                source.Position = 0;
            }

            Stream stream;
            Stream tempStream = null;
            try
            {
                tempStream = new MemoryStream();
                XmlDocument document = new XmlDocument();
                document.Load(source);
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    XmlDeclaration declaration = (XmlDeclaration)document.FirstChild;
                    declaration.Encoding = targetEncoding.WebName;
                }
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = targetEncoding;
                settings.Indent = true;
                settings.OmitXmlDeclaration = omitXmlDeclaration;
                using (XmlWriter writer = XmlWriter.Create(tempStream, settings))
                {
                    document.Save(writer);
                    writer.Flush();
                }

                if (source.CanSeek) { source.Seek(startingPosition, SeekOrigin.Begin); } // reset to original position
                tempStream.Position = 0;

                switch (sequence)
                {
                    case PreambleSequence.Keep:
                        stream = tempStream;
                        tempStream = null;
                        break;
                    case PreambleSequence.Remove:
                        byte[] valueInBytes = ((MemoryStream)tempStream).ToArray();
                        using (tempStream)
                        {
                            valueInBytes = ByteUtility.RemovePreamble(valueInBytes, targetEncoding);
                        }
                        tempStream = StreamConverter.FromBytes(valueInBytes);
                        stream = tempStream;
                        tempStream = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sequence));
                }
            }
            finally
            {
                if (tempStream != null) { tempStream.Dispose(); }
            }
            return stream;
        }
    }
}