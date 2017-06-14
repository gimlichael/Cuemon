using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Xml;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make XML <see cref="Stream"/> related conversions easier to work with.
    /// </summary>
    public static class XmlStreamConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="exception"/> to an XML <see cref="Stream"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to convert into an XML <see cref="Stream"/>.</param>
        /// <returns>An XML <see cref="Stream"/> variant of the specified <paramref name="exception"/>.</returns>
        /// <remarks>The converted <paramref name="exception"/> defaults to using an instance of <see cref="UTF8Encoding"/> unless specified otherwise.</remarks>
        public static Stream FromException(Exception exception)
        {
            return FromException(exception, Encoding.UTF8);
        }

        /// <summary>
        /// Converts the specified <paramref name="exception"/> to an XML <see cref="Stream"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to convert into an XML <see cref="Stream"/>.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <returns>An XML <see cref="Stream"/> variant of the specified <paramref name="exception"/>.</returns>
        public static Stream FromException(Exception exception, Encoding encoding)
        {
            return FromException(exception, encoding, false);
        }

        /// <summary>
        /// Converts the specified <paramref name="exception"/> to an XML <see cref="Stream"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to convert into an XML <see cref="Stream"/>.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <param name="includeStackTrace">if set to <c>true</c> the stack trace of the exception (and possible user data) is included in the converted result.</param>
        /// <returns>An XML <see cref="Stream"/> variant of the specified <paramref name="exception"/>.</returns>
        public static Stream FromException(Exception exception, Encoding encoding, bool includeStackTrace)
        {
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (encoding == null) { throw new ArgumentNullException(nameof(encoding)); }
            MemoryStream tempOutput = null;
            MemoryStream output;
            try
            {
                tempOutput = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(o => o.Encoding = encoding)))
                {
                    WriteException(writer, exception, includeStackTrace);
                    writer.Flush();
                    tempOutput.Position = 0;
                    output = tempOutput;
                    tempOutput = null;
                }
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            output.Position = 0;
            return output;
        }

        internal static void WriteException(XmlWriter writer, Exception exception, bool includeStackTrace)
        {
            Type exceptionType = exception.GetType();
            writer.WriteStartElement(XmlUtility.SanitizeElementName(exceptionType.Name));
            writer.WriteAttributeString("namespace", exceptionType.Namespace);
            WriteExceptionCore(writer, exception, includeStackTrace);
            writer.WriteEndElement();
        }

        private static void WriteEndElement<T>(T counter, XmlWriter writer)
        {
            writer.WriteEndElement();
        }

        private static void WriteExceptionCore(XmlWriter writer, Exception exception, bool includeStackTrace)
        {
            if (!string.IsNullOrEmpty(exception.Source))
            {
                writer.WriteElementString("Source", exception.Source);
            }

            if (!string.IsNullOrEmpty(exception.Message))
            {
                writer.WriteElementString("Message", exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WriteStartElement("StackTrace");
                string[] lines = exception.StackTrace.Split(new[] { StringUtility.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    writer.WriteElementString("Frame", line.Trim());
                }
                writer.WriteEndElement();
            }

            if (exception.Data.Count > 0)
            {
                writer.WriteStartElement("Data");
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WriteStartElement(XmlUtility.SanitizeElementName(entry.Key.ToString()));
                    writer.WriteString(XmlUtility.SanitizeElementText(entry.Value.ToString()));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            WriteInnerExceptions(writer, exception, includeStackTrace);
        }

        private static void WriteInnerExceptions(XmlWriter writer, Exception exception, bool includeStackTrace)
        {
            var aggregated = exception as AggregateException;
            var innerExceptions = new List<Exception>();
            if (aggregated != null) { innerExceptions.AddRange(aggregated.InnerExceptions); }
            if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            if (innerExceptions.Count > 0)
            {
                int endElementsToWrite = 0;
                foreach (var inner in innerExceptions)
                {
                    Type exceptionType = inner.GetType();
                    writer.WriteStartElement(XmlUtility.SanitizeElementName(exceptionType.Name));
                    writer.WriteAttributeString("namespace", exceptionType.Namespace);
                    WriteExceptionCore(writer, inner, includeStackTrace);
                    endElementsToWrite++;
                }
                LoopUtility.For(endElementsToWrite, WriteEndElement, writer);
            }
        }

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