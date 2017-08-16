using System;
using System.Collections.Generic;
using System.Xml;

namespace Cuemon.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlReaderUtility"/> class.
    /// </summary>
    public static class XmlReaderUtilityExtensions
    {
        /// <summary>
        /// Creates and returns a chunked sequence of <see cref="XmlReader"/> objects with a maximum of 128 XML node elements located on a depth of 1.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data to chunk into smaller <see cref="XmlReader"/> objects for a batch run or similar.</param>
        /// <returns>An sequence of <see cref="XmlReader"/> objects that contains no more than 128 XML node elements from the <paramref name="reader" /> object.</returns>
        public static IEnumerable<XmlReader> Chunk(this XmlReader reader)
        {
            return XmlReaderUtility.Chunk(reader);
        }

        /// <summary>
        /// Creates and returns a chunked sequence of <see cref="XmlReader"/> objects with a maximum of the specified <paramref name="size"/> of XML node elements located on a depth of 1.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data to chunk into smaller <see cref="XmlReader"/> objects for a batch run or similar.</param>
        /// <param name="size">The amount of XML node elements allowed per <see cref="XmlReader"/> object. Default is 128 XML node element.</param>
        /// <param name="setup">The <see cref="XmlWriterSettings"/> which need to be configured.</param>
        /// <returns>An sequence of <see cref="XmlReader"/> objects that contains no more than the specified <paramref name="size"/> of XML node elements from the <paramref name="reader" /> object.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the <paramref name="reader"/> object has already been called.
        /// </exception>
        public static IEnumerable<XmlReader> Chunk(this XmlReader reader, int size, Action<XmlWriterSettings> setup = null)
        {
            return XmlReaderUtility.Chunk(reader, size, setup);
        }
    }
}