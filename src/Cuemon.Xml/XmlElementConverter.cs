using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.IO;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make <see cref="XmlElement"/> related conversions easier to work with.
    /// </summary>
    public class XmlElementConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="exception"/> to an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to convert into an <see cref="XmlElement"/>.</param>
        /// <returns>An <see cref="XmlElement"/> variant of the specified <paramref name="exception"/>.</returns>
        public static XmlElement FromException(Exception exception)
        {
            return FromException(exception, Encoding.Unicode);
        }

        /// <summary>
        /// Converts the specified <paramref name="exception"/> to an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to convert into an <see cref="XmlElement"/>.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <returns>An <see cref="XmlElement"/> variant of the specified <paramref name="exception"/>.</returns>
        public static XmlElement FromException(Exception exception, Encoding encoding)
        {
            return FromException(exception, encoding, false);
        }

        /// <summary>
        /// Converts the specified <paramref name="exception"/> to an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to convert into an <see cref="XmlElement"/>.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <param name="includeStackTrace">if set to <c>true</c> the stack trace of the exception (and possible user data) is included in the converted result.</param>
        /// <returns>An <see cref="XmlElement"/> variant of the specified <paramref name="exception"/>.</returns>
        public static XmlElement FromException(Exception exception, Encoding encoding, bool includeStackTrace)
        {
            using (Stream output = XmlStreamConverter.FromException(exception, encoding, includeStackTrace))
            {
                XmlDocument document = new XmlDocument();
                document.Load(output);
                return document.DocumentElement;
            }
        }
    }
}