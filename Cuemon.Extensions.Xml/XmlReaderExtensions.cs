using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Cuemon.Xml;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlReader"/> class.
    /// </summary>
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Creates and returns a sequence of chunked <see cref="XmlReader"/> objects with a maximum of the specified <paramref name="size"/> of XML node elements located on a depth of 1.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data to chunk into smaller <see cref="XmlReader"/> objects for a batch run or similar.</param>
        /// <param name="size">The amount of XML node elements allowed per <see cref="XmlReader"/> object. Default is 128 XML node element.</param>
        /// <param name="setup">The <see cref="XmlWriterSettings"/> which need to be configured.</param>
        /// <returns>An sequence of <see cref="XmlReader"/> objects that contains no more than the specified <paramref name="size"/> of XML node elements from the <paramref name="reader" /> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the <paramref name="reader"/> object has already been called.
        /// </exception>
        public static IEnumerable<XmlReader> Chunk(this XmlReader reader, int size = 128, Action<XmlWriterSettings> setup = null)
        {
            return XmlReaderUtility.Chunk(reader, size, setup);
        }

        /// <summary>
        /// Copies everything from the specified <paramref name="reader"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding an exact copy of the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public static Stream CopyToStream(this XmlReader reader, Action<XmlCopyOptions> setup = null)
        {
            return XmlReaderUtility.CopyToStream(reader, setup);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream CopyToStream(this XmlReader reader, Action<XmlWriter, XmlReader, DisposableOptions> copier, Action<XmlCopyOptions> setup = null)
        {
            return XmlReaderUtility.CopyToStream(reader, copier, setup);
        }
    }
}