using System;
using System.IO;
using System.Xml;

namespace Cuemon.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlWriterUtility"/> class.
    /// </summary>
    public static class XmlWriterUtilityExtensions
    {
        /// <summary>
        /// Copies everything from the specified <paramref name="reader"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding an exact copy of the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public static Stream Copy(this XmlReader reader, Action<XmlCopyOptions> setup = null)
        {
            return XmlWriterUtility.Copy(reader, setup);
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
        public static Stream Copy(this XmlReader reader, Action<XmlWriter, XmlReader> copier, Action<XmlCopyOptions> setup = null)
        {
            return XmlWriterUtility.Copy(reader, copier, setup);
        }


        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="copier"/>.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> object that contains the XML data.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="copier"/>.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T>(this XmlReader reader, Action<XmlWriter, XmlReader, T> copier, T arg, Action<XmlCopyOptions> setup = null)
        {
            return XmlWriterUtility.Copy(reader, copier, arg, setup);
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
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2>(this XmlReader reader, Action<XmlWriter, XmlReader, T1, T2> copier, T1 arg1, T2 arg2, Action<XmlCopyOptions> setup = null)
        {
            return XmlWriterUtility.Copy(reader, copier, arg1, arg2, setup);
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
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3>(this XmlReader reader, Action<XmlWriter, XmlReader, T1, T2, T3> copier, T1 arg1, T2 arg2, T3 arg3, Action<XmlCopyOptions> setup = null)
        {
            return XmlWriterUtility.Copy(reader, copier, arg1, arg2, arg3, setup);
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
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3, T4>(this XmlReader reader, Action<XmlWriter, XmlReader, T1, T2, T3, T4> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<XmlCopyOptions> setup = null)
        {
            return XmlWriterUtility.Copy(reader, copier, arg1, arg2, arg3, arg4, setup);
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
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream Copy<T1, T2, T3, T4, T5>(this XmlReader reader, Action<XmlWriter, XmlReader, T1, T2, T3, T4, T5> copier, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<XmlCopyOptions> setup = null)
        {
            return XmlWriterUtility.Copy(reader, copier, arg1, arg2, arg3, arg4, arg5, setup);
        }
    }
}