using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make <see cref="StreamWriter"/> related operations easier to work with.
    /// </summary>
    public static class StreamWriterUtility
    {
        /// <summary>
        /// Specifies a set of features to support the <see cref="StreamWriter"/> object.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="StreamWriterSettings"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="StreamWriterSettings.AutoFlush"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StreamWriterSettings.BufferSize"/></term>
        ///         <description><c>1024</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StreamWriterSettings.Encoding"/></term>
        ///         <description><see cref="Encoding.UTF8"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StreamWriterSettings.FormatProvider"/></term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StreamWriterSettings.NewLine"/></term>
        ///         <description><c>\r\n</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <returns>A <see cref="StreamWriterSettings"/> instance that specifies a set of features to support the <see cref="StreamWriter"/> object.</returns>
        public static StreamWriterSettings CreateSettings()
        {
            return CreateSettings(Encoding.UTF8);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="StreamWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>A <see cref="StreamWriterSettings"/> instance that specifies a set of features to support the <see cref="StreamWriter"/> object.</returns>
        public static StreamWriterSettings CreateSettings(Encoding encoding)
        {
            return CreateSettings(encoding, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="StreamWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information. The default is <see cref="CultureInfo.InvariantCulture"/>.</param>
        /// <returns>A <see cref="StreamWriterSettings"/> instance that specifies a set of features to support the <see cref="StreamWriter"/> object.</returns>
        public static StreamWriterSettings CreateSettings(Encoding encoding, IFormatProvider provider)
        {
            return CreateSettings(encoding, provider, false);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="StreamWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information. The default is <see cref="CultureInfo.InvariantCulture"/>.</param>
        /// <param name="autoFlush"><c>true</c> to force <see cref="StreamWriter"/> to flush its buffer; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <returns>A <see cref="StreamWriterSettings"/> instance that specifies a set of features to support the <see cref="StreamWriter"/> object.</returns>
        public static StreamWriterSettings CreateSettings(Encoding encoding, IFormatProvider provider, bool autoFlush)
        {
            return CreateSettings(encoding, provider, autoFlush, 1024);
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="StreamWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information. The default is <see cref="CultureInfo.InvariantCulture"/>.</param>
        /// <param name="autoFlush"><c>true</c> to force <see cref="StreamWriter"/> to flush its buffer; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <param name="bufferSize">The size of the buffer in bytes. The default is <c>1024</c>.</param>
        /// <returns>A <see cref="StreamWriterSettings"/> instance that specifies a set of features to support the <see cref="StreamWriter"/> object.</returns>
        public static StreamWriterSettings CreateSettings(Encoding encoding, IFormatProvider provider, bool autoFlush, int bufferSize)
        {
            return CreateSettings(encoding, provider, autoFlush, bufferSize, "\r\n");
        }

        /// <summary>
        /// Specifies a set of features to support the <see cref="StreamWriter"/> object.
        /// </summary>
        /// <param name="encoding">The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information. The default is <see cref="CultureInfo.InvariantCulture"/>.</param>
        /// <param name="autoFlush"><c>true</c> to force <see cref="StreamWriter"/> to flush its buffer; otherwise, <c>false</c>. The default is <c>false</c>.</param>
        /// <param name="bufferSize">The size of the buffer in bytes. The default is <c>1024</c>.</param>
        /// <param name="newLine">The line terminator string for the output of the <see cref="StreamWriter"/>. The default is  is a carriage return followed by a line feed ("\r\n").</param>
        /// <returns>A <see cref="StreamWriterSettings"/> instance that specifies a set of features to support the <see cref="StreamWriter"/> object.</returns>
        public static StreamWriterSettings CreateSettings(Encoding encoding, IFormatProvider provider, bool autoFlush, int bufferSize, string newLine)
        {
            StreamWriterSettings settings = new StreamWriterSettings();
            settings.Encoding = encoding;
            settings.FormatProvider = provider;
            settings.AutoFlush = autoFlush;
            settings.BufferSize = bufferSize;
            settings.NewLine = newLine;
            return settings;
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream(Action<StreamWriter> writer)
        {
            return CreateStream(writer, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T>(Action<StreamWriter, T> writer, T arg)
        {
            return CreateStream(writer, arg, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2>(Action<StreamWriter, T1, T2> writer, T1 arg1, T2 arg2)
        {
            return CreateStream(writer, arg1, arg2, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3>(Action<StreamWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3)
        {
            return CreateStream(writer, arg1, arg2, arg3, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3, T4>(Action<StreamWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return CreateStream(writer, arg1, arg2, arg3, arg4, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3, T4, T5>(Action<StreamWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return CreateStream(writer, arg1, arg2, arg3, arg4, arg5, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6>(Action<StreamWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return CreateStream(writer, arg1, arg2, arg3, arg4, arg5, arg6, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return CreateStream(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7, T8>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return CreateStream(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
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
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return CreateStream(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
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
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
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
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        /// <remarks>This method uses a default implementation of <see cref="StreamWriterSettings"/> as specified by <see cref="CreateSettings()"/>.</remarks>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return CreateStream(writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, CreateSettings());
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream(Action<StreamWriter> writer, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T>(Action<StreamWriter, T> writer, T arg, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2>(Action<StreamWriter, T1, T2> writer, T1 arg1, T2 arg2, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3>(Action<StreamWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3, T4>(Action<StreamWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3, T4, T5>(Action<StreamWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6>(Action<StreamWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7, T8>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
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
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return CreateCore(factory, settings);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
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
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
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
        /// <param name="settings">The <see cref="StreamWriterSettings"/> that will be applied to the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream CreateStream<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, StreamWriterSettings settings)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return CreateCore(factory, settings);
        }

        private static Stream CreateCore<TTuple>(ActionFactory<TTuple> factory, StreamWriterSettings settings) where TTuple : Template<StreamWriter>
        {
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }
            Stream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream(settings.BufferSize);
                StreamWriter writer = new InternalStreamWriter(tempOutput, settings);
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
                throw ExceptionUtility.Refine(new InvalidOperationException("There is an error in the Stream being written.", ex), factory.DelegateInfo, parameters.ToArray());
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            return output;
        }
    }
}