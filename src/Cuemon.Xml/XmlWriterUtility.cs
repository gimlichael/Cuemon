using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make <see cref="XmlWriter"/> related operations easier to work with.
    /// </summary>
    public static class XmlWriterUtility
    {
        /// <summary>
        /// Copies everything from the specified <paramref name="reader"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <returns>A <see cref="Stream"/> holding an exact copy of the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public static Stream Copy(XmlReader reader)
        {
            return Copy(reader, CreateSettings());
        }

        /// <summary>
        /// Copies everything from the specified <paramref name="reader"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied doing the copying process.</param>
        /// <returns>A <see cref="Stream"/> holding an exact copy of the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public static Stream Copy(XmlReader reader, XmlWriterSettings settings)
        {
            return Copy(reader, settings, false);
        }

        /// <summary>
        /// Copies everything from the specified <paramref name="reader"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied doing the copying process.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <paramref name="reader"/> is being left open; otherwise <paramref name="reader"/> is disposed of.</param>
        /// <returns>A <see cref="Stream"/> holding an exact copy of the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public static Stream Copy(XmlReader reader, XmlWriterSettings settings, bool leaveStreamOpen)
        {
            return Copy(reader, settings, leaveStreamOpen, DefaultCopier);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream Copy(XmlReader reader, Action<XmlWriter, XmlReader> copier)
        {
            return Copy(reader, CreateSettings(), copier);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy(XmlReader reader, XmlWriterSettings settings, Action<XmlWriter, XmlReader> copier)
        {
            return Copy(reader, settings, false, copier);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <paramref name="reader"/> is being left open; otherwise <paramref name="reader"/> is disposed of.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy(XmlReader reader, XmlWriterSettings settings, bool leaveStreamOpen, Action<XmlWriter, XmlReader> copier)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            if (copier == null) { throw new ArgumentNullException(nameof(copier)); }
            try
            {
                return CreateXml(copier, reader, settings);
            }
            finally
            {
                if (!leaveStreamOpen) { reader.Dispose(); }
            }
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream Copy<T>(XmlReader reader, Action<XmlWriter, XmlReader, T> copier, T arg)
        {
            return Copy(reader, CreateSettings(), copier, arg);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T>(XmlReader reader, XmlWriterSettings settings, Action<XmlWriter, XmlReader, T> copier, T arg)
        {
            return Copy(reader, settings, false, copier, arg);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <paramref name="reader"/> is being left open; otherwise <paramref name="reader"/> is disposed of.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T>(XmlReader reader, XmlWriterSettings settings, bool leaveStreamOpen, Action<XmlWriter, XmlReader, T> copier, T arg)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            if (copier == null) { throw new ArgumentNullException(nameof(copier)); }
            try
            {
                return CreateXml(copier, reader, arg, settings);
            }
            finally
            {
                if (!leaveStreamOpen) { reader.Dispose(); }
            }
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream Copy<T1, T2>(XmlReader reader, Action<XmlWriter, XmlReader, T1, T2> copier, T1 arg1, T2 arg2)
        {
            return Copy(reader, CreateSettings(), copier, arg1, arg2);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2>(XmlReader reader, XmlWriterSettings settings, Action<XmlWriter, XmlReader, T1, T2> copier, T1 arg1, T2 arg2)
        {
            return Copy(reader, settings, false, copier, arg1, arg2);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <paramref name="reader"/> is being left open; otherwise <paramref name="reader"/> is disposed of.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2>(XmlReader reader, XmlWriterSettings settings, bool leaveStreamOpen, Action<XmlWriter, XmlReader, T1, T2> copier, T1 arg1, T2 arg2)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            if (copier == null) { throw new ArgumentNullException(nameof(copier)); }
            try
            {
                return CreateXml(copier, reader, arg1, arg2, settings);
            }
            finally
            {
                if (!leaveStreamOpen) { reader.Dispose(); }
            }
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream Copy<T1, T2, T3>(XmlReader reader, Action<XmlWriter, XmlReader, T1, T2, T3> copier, T1 arg1, T2 arg2, T3 arg3)
        {
            return Copy(reader, CreateSettings(), copier, arg1, arg2, arg3);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3>(XmlReader reader, XmlWriterSettings settings, Action<XmlWriter, XmlReader, T1, T2, T3> copier, T1 arg1, T2 arg2, T3 arg3)
        {
            return Copy(reader, settings, false, copier, arg1, arg2, arg3);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <paramref name="reader"/> is being left open; otherwise <paramref name="reader"/> is disposed of.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3>(XmlReader reader, XmlWriterSettings settings, bool leaveStreamOpen, Action<XmlWriter, XmlReader, T1, T2, T3> copier, T1 arg1, T2 arg2, T3 arg3)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            if (copier == null) { throw new ArgumentNullException(nameof(copier)); }
            try
            {
                return CreateXml(copier, reader, arg1, arg2, arg3, settings);
            }
            finally
            {
                if (!leaveStreamOpen) { reader.Dispose(); }
            }
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream Copy<T1, T2, T3, T4>(XmlReader reader, Action<XmlWriter, XmlReader, T1, T2, T3, T4> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return Copy(reader, CreateSettings(), copier, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3, T4>(XmlReader reader, XmlWriterSettings settings, Action<XmlWriter, XmlReader, T1, T2, T3, T4> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return Copy(reader, settings, false, copier, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <paramref name="reader"/> is being left open; otherwise <paramref name="reader"/> is disposed of.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3, T4>(XmlReader reader, XmlWriterSettings settings, bool leaveStreamOpen, Action<XmlWriter, XmlReader, T1, T2, T3, T4> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            if (copier == null) { throw new ArgumentNullException(nameof(copier)); }
            try
            {
                return CreateXml(copier, reader, arg1, arg2, arg3, arg4, settings);
            }
            finally
            {
                if (!leaveStreamOpen) { reader.Dispose(); }
            }
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream Copy<T1, T2, T3, T4, T5>(XmlReader reader, Action<XmlWriter, XmlReader, T1, T2, T3, T4, T5> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return Copy(reader, CreateSettings(), copier, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3, T4, T5>(XmlReader reader, XmlWriterSettings settings, Action<XmlWriter, XmlReader, T1, T2, T3, T4, T5> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return Copy(reader, settings, false, copier, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="copier"/>.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <paramref name="reader"/> is being left open; otherwise <paramref name="reader"/> is disposed of.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="copier"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="settings"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3, T4, T5>(XmlReader reader, XmlWriterSettings settings, bool leaveStreamOpen, Action<XmlWriter, XmlReader, T1, T2, T3, T4, T5> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            if (copier == null) { throw new ArgumentNullException(nameof(copier)); }
            try
            {
                return CreateXml(copier, reader, arg1, arg2, arg3, arg4, arg5, settings);
            }
            finally
            {
                if (!leaveStreamOpen) { reader.Dispose(); }
            }
        }

        private static void DefaultCopier(XmlWriter writer, XmlReader reader)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.CDATA:
                        writer.WriteCData(reader.Value);
                        break;
                    case XmlNodeType.Comment:
                        writer.WriteComment(reader.Value);
                        break;
                    case XmlNodeType.DocumentType:
                        writer.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
                        break;
                    case XmlNodeType.Element:
                        writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                        writer.WriteAttributes(reader, true);
                        if (reader.IsEmptyElement) { writer.WriteEndElement(); }
                        break;
                    case XmlNodeType.EndElement:
                        writer.WriteFullEndElement();
                        break;
                    case XmlNodeType.Attribute:
                    case XmlNodeType.Document:
                    case XmlNodeType.DocumentFragment:
                    case XmlNodeType.EndEntity:
                    case XmlNodeType.None:
                    case XmlNodeType.Notation:
                    case XmlNodeType.Entity:
                        break;
                    case XmlNodeType.EntityReference:
                        writer.WriteEntityRef(reader.Name);
                        break;
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        writer.WriteWhitespace(reader.Value);
                        break;
                    case XmlNodeType.Text:
                        writer.WriteString(reader.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                    case XmlNodeType.XmlDeclaration:
                        writer.WriteProcessingInstruction(reader.Name, reader.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="XmlWriterSettings"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.CheckCharacters"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.CloseOutput"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.ConformanceLevel"/></term>
        ///         <description><see cref="ConformanceLevel.Document"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.Encoding"/></term>
        ///         <description><see cref="Encoding.UTF8"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.Indent"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.IndentChars"/></term>
        ///         <description><see cref="StringUtility.Tab"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.NewLineChars"/></term>
        ///         <description><see cref="StringUtility.NewLine"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.NewLineHandling"/></term>
        ///         <description><see cref="NewLineHandling.None"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.NewLineOnAttributes"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.OmitXmlDeclaration"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="XmlWriterSettings.NewLineOnAttributes"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static XmlWriterSettings CreateSettings()
        {
            return CreateSettings(Encoding.UTF8);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding)
        {
            return CreateSettings(encoding, false);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="omitXmlDeclaration"><c>true</c> to omit the XML declaration; otherwise, <c>false</c>. The default is <c>false</c>, specifying that an XML declaration is written.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding, bool omitXmlDeclaration)
        {
            return CreateSettings(encoding, omitXmlDeclaration, ConformanceLevel.Document);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="omitXmlDeclaration"><c>true</c> to omit the XML declaration; otherwise, <c>false</c>. The default is <c>false</c>, specifying that an XML declaration is written.</param>
        /// <param name="conformanceLevel">One of the <see cref="ConformanceLevel"/> values. The default is <see cref="ConformanceLevel.Document"/>.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding, bool omitXmlDeclaration, ConformanceLevel conformanceLevel)
        {
            return CreateSettings(encoding, omitXmlDeclaration, conformanceLevel, NewLineHandling.None);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="omitXmlDeclaration"><c>true</c> to omit the XML declaration; otherwise, <c>false</c>. The default is <c>false</c>, specifying that an XML declaration is written.</param>
        /// <param name="conformanceLevel">One of the <see cref="ConformanceLevel"/> values. The default is <see cref="ConformanceLevel.Document"/>.</param>
        /// <param name="newLineHandling">One of the <see cref="NewLineHandling"/> values. The default is <see cref="NewLineHandling.None"/>.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding, bool omitXmlDeclaration, ConformanceLevel conformanceLevel, NewLineHandling newLineHandling)
        {
            return CreateSettings(encoding, omitXmlDeclaration, conformanceLevel, newLineHandling, false);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="omitXmlDeclaration"><c>true</c> to omit the XML declaration; otherwise, <c>false</c>. The default is <c>false</c>, specifying that an XML declaration is written.</param>
        /// <param name="conformanceLevel">One of the <see cref="ConformanceLevel"/> values. The default is <see cref="ConformanceLevel.Document"/>.</param>
        /// <param name="newLineHandling">One of the <see cref="NewLineHandling"/> values. The default is <see cref="NewLineHandling.None"/>.</param>
        /// <param name="indent"><c>true</c> to write individual elements on new lines and indent; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding, bool omitXmlDeclaration, ConformanceLevel conformanceLevel, NewLineHandling newLineHandling, bool indent)
        {
            return CreateSettings(encoding, omitXmlDeclaration, conformanceLevel, newLineHandling, indent, false);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="omitXmlDeclaration"><c>true</c> to omit the XML declaration; otherwise, <c>false</c>. The default is <c>false</c>, specifying that an XML declaration is written.</param>
        /// <param name="conformanceLevel">One of the <see cref="ConformanceLevel"/> values. The default is <see cref="ConformanceLevel.Document"/>.</param>
        /// <param name="newLineHandling">One of the <see cref="NewLineHandling"/> values. The default is <see cref="NewLineHandling.None"/>.</param>
        /// <param name="indent"><c>true</c> to write individual elements on new lines and indent; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <param name="newLineOnAttributes"><c>true</c> to write attributes on individual lines; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding, bool omitXmlDeclaration, ConformanceLevel conformanceLevel, NewLineHandling newLineHandling, bool indent, bool newLineOnAttributes)
        {
            return CreateSettings(encoding, omitXmlDeclaration, conformanceLevel, newLineHandling, indent, newLineOnAttributes, StringUtility.NewLine);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="omitXmlDeclaration"><c>true</c> to omit the XML declaration; otherwise, <c>false</c>. The default is <c>false</c>, specifying that an XML declaration is written.</param>
        /// <param name="conformanceLevel">One of the <see cref="ConformanceLevel"/> values. The default is <see cref="ConformanceLevel.Document"/>.</param>
        /// <param name="newLineHandling">One of the <see cref="NewLineHandling"/> values. The default is <see cref="NewLineHandling.None"/>.</param>
        /// <param name="indent"><c>true</c> to write individual elements on new lines and indent; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <param name="newLineOnAttributes"><c>true</c> to write attributes on individual lines; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <param name="newLineChars">The character string to use for line breaks. The default is <see cref="StringUtility.NewLine"/>.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding, bool omitXmlDeclaration, ConformanceLevel conformanceLevel, NewLineHandling newLineHandling, bool indent, bool newLineOnAttributes, string newLineChars)
        {
            return CreateSettings(encoding, omitXmlDeclaration, conformanceLevel, newLineHandling, indent, newLineOnAttributes, newLineChars, StringUtility.Tab);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="XmlWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="omitXmlDeclaration"><c>true</c> to omit the XML declaration; otherwise, <c>false</c>. The default is <c>false</c>, specifying that an XML declaration is written.</param>
        /// <param name="conformanceLevel">One of the <see cref="ConformanceLevel"/> values. The default is <see cref="ConformanceLevel.Document"/>.</param>
        /// <param name="newLineHandling">One of the <see cref="NewLineHandling"/> values. The default is <see cref="NewLineHandling.None"/>.</param>
        /// <param name="indent"><c>true</c> to write individual elements on new lines and indent; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <param name="newLineOnAttributes"><c>true</c> to write attributes on individual lines; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <param name="newLineChars">The character string to use for line breaks. The default is <see cref="StringUtility.NewLine"/>.</param>
        /// <param name="indentChars">The character string to use when indenting. The default is <see cref="StringUtility.Tab"/>.</param>
        /// <returns>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlWriter"/> object.</returns>
        public static XmlWriterSettings CreateSettings(Encoding encoding, bool omitXmlDeclaration, ConformanceLevel conformanceLevel, NewLineHandling newLineHandling, bool indent, bool newLineOnAttributes, string newLineChars, string indentChars)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = encoding;
            settings.ConformanceLevel = conformanceLevel;
            settings.OmitXmlDeclaration = omitXmlDeclaration;
            settings.Indent = indent;
            settings.IndentChars = indentChars;
            settings.NewLineHandling = newLineHandling;
            settings.NewLineChars = newLineChars;
            settings.NewLineOnAttributes = newLineOnAttributes;
            settings.CheckCharacters = true;
            settings.CloseOutput = false;
            return settings;
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml(Action<XmlWriter> writer)
        {
            return CreateXml(writer, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml(Action<XmlWriter> writer, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T>(Action<XmlWriter, T> writer, T arg)
        {
            return CreateXml(writer, arg, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T>(Action<XmlWriter, T> writer, T arg, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2>(Action<XmlWriter, T1, T2> writer, T1 arg1, T2 arg2)
        {
            return CreateXml(writer, arg1, arg2, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2>(Action<XmlWriter, T1, T2> writer, T1 arg1, T2 arg2, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3>(Action<XmlWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3)
        {
            return CreateXml(writer, arg1, arg2, arg3, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3>(Action<XmlWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3, T4>(Action<XmlWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return CreateXml(writer, arg1, arg2, arg3, arg4, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3, T4>(Action<XmlWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3, T4, T5>(Action<XmlWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return CreateXml(writer, arg1, arg2, arg3, arg4, arg5, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3, T4, T5>(Action<XmlWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6>(Action<XmlWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return CreateXml(writer, arg1, arg2, arg3, arg4, arg5, arg6, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6>(Action<XmlWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return CreateXml(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7, T8>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return CreateXml(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7, T8>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return CreateXml(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return CreateXmlCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="XmlWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return CreateXml(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a XML stream by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory XML stream.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The XML settings that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the XML created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateXml<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, XmlWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return CreateXmlCore(factory, settings);
        }

        private static Stream CreateXmlCore<TTuple>(ActionFactory<TTuple> factory, XmlWriterSettings settings) where TTuple : Template<XmlWriter>
        {
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            Stream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(tempOutput, settings))
                {
                    factory.GenericArguments.Arg1 = writer;
                    factory.ExecuteMethod();
                    writer.Flush();
                }
                tempOutput.Flush();
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            catch (Exception ex)
            {
                List<object> parameters = new List<object>();
                parameters.AddRange(factory.GenericArguments.ToArray());
                parameters.Add(settings);
                throw ExceptionUtility.Refine(new InvalidOperationException("There is an error in the XML document.", ex), factory.DelegateInfo, parameters.ToArray());
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            return output;
        }
    }
}