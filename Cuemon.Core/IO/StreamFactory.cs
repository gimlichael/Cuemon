using System;
using System.Collections.Generic;
using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// Provides access to factory methods for creating <see cref="Stream"/> instances.
    /// </summary>
    public static class StreamFactory
    {
        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create(Action<StreamWriter> writer, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null);
            return CreateStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T>(Action<StreamWriter, T> writer, T arg, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg);
            return CreateStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2>(Action<StreamWriter, T1, T2> writer, T1 arg1, T2 arg2, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3>(Action<StreamWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4>(Action<StreamWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5>(Action<StreamWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5, T6>(Action<StreamWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5, T6, T7>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5, T6, T7, T8>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return CreateStreamCore(factory, setup);
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
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<StreamWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, Action<StreamWriterOptions> setup = null)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return CreateStreamCore(factory, setup);
        }

        private static Stream CreateStreamCore<TTuple>(ActionFactory<TTuple> factory, Action<StreamWriterOptions> setup = null) where TTuple : Template<StreamWriter>
        {
            var options = setup.Configure();
            return Disposable.SafeInvoke(() =>
            {
                var stream = new MemoryStream(options.BufferSize);
                var writer = new InternalStreamWriter(stream, options);
                {
                    factory.GenericArguments.Arg1 = writer;
                    factory.ExecuteMethod();
                    writer.Flush();
                }
                stream.Flush();
                stream.Position = 0;
                if (options.Preamble == PreambleSequence.Remove)
                {
                    var preamble = options.Encoding.GetPreamble();
                    if (preamble.Length > 0)
                    {
                        return StreamConverter.RemovePreamble(stream, options.Encoding);
                    }
                }
                return stream;
            }, ex =>
            {
                var parameters = new List<object>();
                parameters.AddRange(factory.GenericArguments.ToArray());
                parameters.Add(options);
                throw ExceptionUtility.Refine(new InvalidOperationException("There is an error in the Stream being written.", ex), factory.DelegateInfo, parameters.ToArray()).Unwrap();
            });
        }
    }
}