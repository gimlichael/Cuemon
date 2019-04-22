using System;
using System.IO;
using System.Text;
using Cuemon.Xml;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="Stream"/> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Remove the XML namespace declarations from the specified <see cref="Stream"/> <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="stream"/> but with no namespace declarations.</returns>
        public static Stream RemoveXmlNamespaceDeclarations(this Stream stream, bool omitXmlDeclaration = false)
        {
            return XmlUtility.RemoveNamespaceDeclarations(stream, omitXmlDeclaration);
        }

        /// <summary>
        /// Remove the XML namespace declarations from the specified <see cref="Stream"/> <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="encoding">The text encoding to use.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="stream"/> but with no namespace declarations.</returns>
        public static Stream RemoveXmlNamespaceDeclarations(this Stream stream, bool omitXmlDeclaration, Encoding encoding)
        {
            return XmlUtility.RemoveNamespaceDeclarations(stream, omitXmlDeclaration, encoding);
        }
    }
}