using System.IO;
using System.Text;
using System.Xml;
using Cuemon.IO;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make XML <see cref="Encoding"/> operations easier to work with.
    /// </summary>
    public static class XmlEncodingUtility
    {
        /// <summary>
        /// Reads the <see cref="Encoding"/> from the specified XML <see cref="Stream"/>. If an encoding cannot be resolved, UTF-8 encoding is assumed for the <see cref="Encoding"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to resolve an <see cref="Encoding"/> object from.</param>
        /// <returns>An <see cref="Encoding"/> object equivalent to the encoding used in the <paramref name="value"/>, or <see cref="Encoding.UTF8"/> if unable to resolve the encoding.</returns>
        public static Encoding ReadEncoding(Stream value)
        {
            return ReadEncoding(value, Encoding.UTF8);
        }

        /// <summary>
        /// Reads the <see cref="Encoding"/> from the specified XML <see cref="Stream"/>. If an encoding cannot be resolved, <paramref name="defaultEncoding"/> encoding is assumed for the <see cref="Encoding"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to resolve an <see cref="Encoding"/> object from.</param>
        /// <param name="defaultEncoding">The preferred default <see cref="Encoding"/> to use if an encoding cannot be resolved automatically.</param>
        /// <returns>An <see cref="Encoding"/> object equivalent to the encoding used in the <paramref name="value"/>, or <paramref name="defaultEncoding"/> if unable to resolve the encoding.</returns>
        public static Encoding ReadEncoding(Stream value, Encoding defaultEncoding)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (!value.TryDetectUnicodeEncoding(out var encoding))
            {
                long startingPosition = -1;
                if (value.CanSeek)
                {
                    startingPosition = value.Position;
                    value.Position = 0;
                }

                XmlDocument document = new XmlDocument();
                document.Load(value);
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    XmlDeclaration declaration = (XmlDeclaration)document.FirstChild;
                    if (!string.IsNullOrEmpty(declaration.Encoding)) { encoding = Encoding.GetEncoding(declaration.Encoding); }
                }

                if (value.CanSeek) { value.Seek(startingPosition, SeekOrigin.Begin); }
            }
            return encoding ?? defaultEncoding;
        }
    }
}